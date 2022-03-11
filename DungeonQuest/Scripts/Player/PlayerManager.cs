using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Menus;
using DungeonQuest.DebugConsole;

namespace DungeonQuest.Player
{
	public class PlayerManager : MonoBehaviour
	{
		[HideInInspector] public Vector2 moveDirection;
		[HideInInspector] public Animator playerAnimation;
		[HideInInspector] public Vector2 faceingDirection;
		[HideInInspector] public BoxCollider2D boxCollider;

		private const float MOVE_LIMITER = 0.7f;
		private float x, y;
		private bool hasMovementInput;

		private Vector2 lastMoveDirection;
		private PlayerAttack playerAttack;
		private Slider healthBar;

		[Header("PlayerConfig:")]
		public float playerSpeed;
		public float playerHealth;

		[SerializeField] private AudioClip deathSFX;

		public bool PlayerDied { get; private set; }
		public bool IsMoveing { get; private set; }
		public bool GodMode { private get; set; }

		void Awake()
		{
			playerAnimation = GetComponent<Animator>();
			playerAttack = GetComponent<PlayerAttack>();
			boxCollider = GetComponent<BoxCollider2D>();
			healthBar = GetComponentInChildren<Slider>();

			healthBar.maxValue = playerHealth;
		}

		void Update()
		{
			hasMovementInput = x != 0 || y != 0;
			IsMoveing = moveDirection.x != 0 || moveDirection.y != 0;
			healthBar.value = playerHealth;

			if (playerHealth <= 0)
			{
				playerHealth = 0;
				PlayerDied = true;
				healthBar.fillRect.GetComponent<Image>().color = Color.black;
				Die();
			}

			if (hasMovementInput && IsMoveing)
			{
				faceingDirection = moveDirection;
			}
			else
			{
				faceingDirection = lastMoveDirection;
			}

			MovementInputs();
			AnimateMovement();
		}

		void FixedUpdate()
		{
			Move();
		}

		public void DamagePlayer(float damage)
		{
			if (GodMode) return;

			if (playerHealth > 0)
			{
				playerHealth -= damage;
			}
		}

		public void Die()
		{
			if (DebugController.IS_CONSOLE_ON || PauseMenu.IS_GAME_PAUSED) return;

			playerAnimation.SetBool("AnimDeath", PlayerDied);
			rigidbody2D.isKinematic = true;

			var deathSound = GetComponent<AudioSource>();
			deathSound.clip = deathSFX;
			deathSound.Play();

			Destroy(boxCollider);
			Destroy(playerAttack);
			Destroy(this);
		}

		private void MovementInputs()
		{
			if (PauseMenu.IS_GAME_PAUSED || DebugController.IS_CONSOLE_ON) return;

			x = Input.GetAxisRaw("Horizontal");
			y = Input.GetAxisRaw("Vertical");

			if (playerAttack.IsAttacking) return;

			if (IsMoveing)
			{
				lastMoveDirection = moveDirection;
			}

			moveDirection = new Vector2(x, y).normalized;
		}

		private void Move()
		{
			bool hasInput = x != 0 && y != 0;

			if (hasInput)
			{
				x *= MOVE_LIMITER;
				y *= MOVE_LIMITER;
			}
			
			rigidbody2D.velocity = new Vector2(moveDirection.x * playerSpeed, moveDirection.y * playerSpeed);
		}

		private void AnimateMovement()
		{
			playerAnimation.SetFloat("AnimMoveX", moveDirection.x);
			playerAnimation.SetFloat("AnimMoveY", moveDirection.y);

			playerAnimation.SetFloat("AnimLastMoveX", lastMoveDirection.x);
			playerAnimation.SetFloat("AnimLastMoveY", lastMoveDirection.y);

			playerAnimation.SetFloat("AnimAttackDirX", faceingDirection.x);
			playerAnimation.SetFloat("AnimAttackDirY", faceingDirection.y);

			playerAnimation.SetFloat("AnimMoveMagnitude", moveDirection.magnitude);

			playerAnimation.SetBool("AnimIsMoveing", hasMovementInput);
		}
	}
}
