using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Menus;
using DungeonQuest.DebugConsole;

namespace DungeonQuest.Player
{
	public class PlayerManager : MonoBehaviour
	{
		public enum FaceingDirection
		{
			DOWN = 0,
			UP = 1,
			LEFT = 2,
			RIGHT = 3
		}

		public enum LastMoveDirection
		{
			DOWN = 0,
			UP = 1,
			LEFT = 2,
			RIGHT = 3
		}

		public enum MoveDirection
		{
			DOWN = 0,
			UP = 1,
			LEFT = 2,
			RIGHT = 3
		}

		[Header("Player Config:")]
		public float playerSpeed;
		public int defaultPlayerHealth;
		public int defaultPlayerArmor;
		[Space(10f)]
		public Slider healthBar;
		public Slider armorBar;
		[SerializeField] private AudioClip deathSFX;

		[HideInInspector] public LastMoveDirection lastMoveDir;
		[HideInInspector] public FaceingDirection faceingDir;
		[HideInInspector] public MoveDirection moveDir;

		[HideInInspector] public BoxCollider2D boxCollider;
		[HideInInspector] public PlayerAttack playerAttack;
		[HideInInspector] public PlayerLeveling playerLeveling;

		private const float MOVE_LIMITER = 0.7f;
		private float x, y;

		private Vector2 unmodifiedMoveDirection;
		private Vector2 faceingDirection;
		private Vector2 lastMoveDirection;
		private Vector2 moveDirection;

		public bool HasMovementInput { get; private set; }
		public bool IsMoveing { get; private set; }
		public bool GodMode { private get; set; }
		public bool IsDead { get; private set; }
		public bool Invisible { get; set; }
		public int PlayerHealth { get; private set; }
		public int PlayerArmor { get; private set; }

		void Awake()
		{
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
			IsMoveing = moveDirection.x != 0 || moveDirection.y != 0;

			healthBar.value = PlayerHealth;
			armorBar.value = PlayerArmor;

			lastMoveDir = (LastMoveDirection)DirectionCheck(lastMoveDirection);
			faceingDir = (FaceingDirection)DirectionCheck(faceingDirection);
			moveDir = (MoveDirection)DirectionCheck(unmodifiedMoveDirection);

			if (playerAttack.IsAttacking) moveDirection = Vector2.zero;

			if (PlayerHealth <= 0)
			{
				PlayerHealth = 0;
				Die();
			}

			if (HasMovementInput && IsMoveing)
			{
				faceingDirection = unmodifiedMoveDirection;
			}
			else
			{
				faceingDirection = lastMoveDirection;
			}

			MovementInputs();
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
				lastMoveDirection = moveDirection;
			}

			moveDirection = new Vector2(x, y).normalized;
			unmodifiedMoveDirection = moveDirection;
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

		private int DirectionCheck(Vector2 direction)
		{
			if (direction.x >= -1f && direction.y < 0f) // Down
			{
				return 0;
			}
			else if (direction.x >= -1f && direction.y > 0f) // Up
			{
				return 1;
			}
			else if (direction == new Vector2(-1f, 0f)) // Left
			{
				return 2;
			}
			else if (direction == new Vector2(1f, 0f)) // Right
			{
				return 3;
			}
			else // Defaults to down
			{
				return 0;
			}
		}
	}
}
