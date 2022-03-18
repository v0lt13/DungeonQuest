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
		[HideInInspector] public PlayerLeveling playerLeveling;

		private const float MOVE_LIMITER = 0.7f;
		private float x, y;
		private Vector2 unmodifiedMoveDirection;

		[Header("Player Config:")]
		public float playerSpeed;
		public int defaultPlayerHealth;
		public int defaultPlayerArmor;
		[Space(10f)]
		public Slider healthBar;
		public Slider armorBar;
		[SerializeField] private AudioClip deathSFX;

		public bool HasMovementInput { get; private set; }
		public bool IsMoveing { get; private set; }
		public bool GodMode { private get; set; }
		public bool IsDead { get; private set; }
		public bool Invisible { get; set; }
		public int PlayerHealth { get; private set; }
		public int PlayerArmor { get; private set; }

		public Vector2 LastMoveDirection { get; private set; }
		public Vector2 FaceingDirection { get; private set; }
		public Vector2 MoveDirection { get; private set; }

		void Awake()
		{
			playerAnim = GetComponent<PlayerAnimationHandler>();
			playerLeveling = GetComponent<PlayerLeveling>();
			playerAttack = GetComponent<PlayerAttack>();
			boxCollider = GetComponent<BoxCollider2D>();

			PlayerHealth = defaultPlayerHealth;
			PlayerArmor = defaultPlayerArmor;
			healthBar.maxValue = defaultPlayerHealth;
			armorBar.maxValue = defaultPlayerArmor;
		}

		void Update()
		{
			HasMovementInput = x != 0 || y != 0;
			IsMoveing = MoveDirection.x != 0 || MoveDirection.y != 0;

			healthBar.value = PlayerHealth;
			armorBar.value = PlayerArmor;

			if (playerAttack.IsAttacking) MoveDirection = Vector2.zero;

			if (PlayerHealth <= 0)
			{
				PlayerHealth = 0;
				Die();
			}

			if (HasMovementInput && IsMoveing)
			{
				FaceingDirection = unmodifiedMoveDirection;
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

			if (PlayerHealth > 0 && PlayerArmor == 0)
			{
				PlayerHealth -= damage;
			}

			if (PlayerArmor > 0)
			{
				PlayerArmor -= damage;
			}

			if (PlayerArmor < 0)
			{
				// The player armor will be a negative number so we have to add it to the player health to substract from it cuz M A T H
				PlayerHealth += PlayerArmor;
				PlayerArmor = 0;
			}
		}

		public void HealPlayer(int amount)
		{
			PlayerHealth += amount;

			if (PlayerHealth > defaultPlayerHealth) PlayerHealth = defaultPlayerHealth;
		}

		public void ArmorPlayer(int amount)
		{
			PlayerArmor += amount;

			if (PlayerArmor > defaultPlayerArmor) PlayerArmor = defaultPlayerArmor;
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
			unmodifiedMoveDirection = MoveDirection;
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
