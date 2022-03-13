using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Menus;
using DungeonQuest.DebugConsole;

namespace DungeonQuest.Player
{
	public class PlayerManager : MonoBehaviour
	{
		[HideInInspector] public BoxCollider2D boxCollider;
		[HideInInspector] public PlayerAttack playerAttack;
		[HideInInspector] public PlayerAnimationHandler playerAnim;

		private const float MOVE_LIMITER = 0.7f;
		private float x, y;

		[Header("Player Config:")]
		public float playerSpeed;
		public int playerHealth;
		[Space(10f)]
		[SerializeField] private AudioClip deathSFX;
		[SerializeField] private Slider[] healthBars;

		public bool HasMovementInput { get; private set; }
		public bool IsMoveing { get; private set; }
		public bool GodMode { private get; set; }
		public bool IsDead { get; private set; }

		public Vector2 LastMoveDirection { get; private set; }
		public Vector2 FaceingDirection { get; private set; }
		public Vector2 MoveDirection { get; private set; }

		void Awake()
		{
			playerAnim = GetComponent<PlayerAnimationHandler>();
			playerAttack = GetComponent<PlayerAttack>();
			boxCollider = GetComponent<BoxCollider2D>();

			foreach (var healthBar in healthBars) healthBar.maxValue = playerHealth;
		}

		void Update()
		{
			HasMovementInput = x != 0 || y != 0;
			IsMoveing = MoveDirection.x != 0 || MoveDirection.y != 0;

			foreach (var healthBar in healthBars) healthBar.value = playerHealth;

			if (playerAttack.IsAttacking) MoveDirection = Vector2.zero;

			if (playerHealth <= 0)
			{
				playerHealth = 0;
				Die();
			}

			if (HasMovementInput && IsMoveing)
			{
				FaceingDirection = MoveDirection;
			}
			else
			{
				FaceingDirection = LastMoveDirection;
			}

			MovementInputs();
			playerAnim.Animate();
		}

		void FixedUpdate()
		{
			Move();
		}

		public void DamagePlayer(int damage)
		{
			if (GodMode) return;

			if (playerHealth > 0)
			{
				playerHealth -= damage;
			}
		}

		private void MovementInputs()
		{
			if (PauseMenu.IS_GAME_PAUSED || DebugController.IS_CONSOLE_ON) return;

			x = Input.GetAxisRaw("Horizontal");
			y = Input.GetAxisRaw("Vertical");

			if (playerAttack.IsAttacking) return;

			if (IsMoveing)
			{
				LastMoveDirection = MoveDirection;
			}

			MoveDirection = new Vector2(x, y).normalized;
		}

		private void Move()
		{
			bool hasInput = x != 0 && y != 0;

			if (hasInput)
			{
				x *= MOVE_LIMITER;
				y *= MOVE_LIMITER;
			}
			
			rigidbody2D.velocity = new Vector2(MoveDirection.x * playerSpeed, MoveDirection.y * playerSpeed);
		}

		private void Die()
		{
			if (DebugController.IS_CONSOLE_ON || PauseMenu.IS_GAME_PAUSED) return;

			IsDead = true;
			rigidbody2D.isKinematic = true;

			foreach (var healthBar in healthBars) healthBar.fillRect.GetComponent<Image>().color = Color.black;

			var deathSound = GetComponent<AudioSource>();
			deathSound.clip = deathSFX;
			deathSound.Play();

			Destroy(boxCollider);
			Destroy(playerAttack);
			Destroy(GetComponentInChildren<PlayerFootsteps>());
			Destroy(this);
		}
	}
}
