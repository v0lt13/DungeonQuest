using UnityEngine;

namespace DungeonQuest.Player
{
	public class PlayerMovement : MonoBehaviour
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

		[HideInInspector] public float playerSpeed;
		[HideInInspector] public bool isWalkingOnIce;
		[HideInInspector] public LastMoveDirection lastMoveDir;
		[HideInInspector] public FaceingDirection faceingDir;
		[HideInInspector] public MoveDirection moveDir;

		public float defaultPlayerSpeed;

		private const float MOVE_LIMITER = 0.7f;
		private float x, y;

		private PlayerManager playerManager;

		private Vector2 unmodifiedMoveDirection;
		private Vector2 faceingDirection;
		private Vector2 lastMoveDirection;
		private Vector2 moveDirection;

		public bool HasMovementInput { get; private set; }
		public bool IsMoveing { get; private set; }

		void Awake()
		{
			playerManager = GetComponent<PlayerManager>();
		}

		void Update()
		{
			HasMovementInput = x != 0 || y != 0;
			IsMoveing = moveDirection.x != 0 || moveDirection.y != 0;

			if (playerManager.playerAttack.IsAttacking) moveDirection = Vector2.zero;

			if (HasMovementInput && IsMoveing)
			{
				faceingDirection = unmodifiedMoveDirection;
			}
			else
			{
				faceingDirection = lastMoveDirection;
			}

			// Cast the return value of DirectionCheck() to enums
			lastMoveDir = (LastMoveDirection)DirectionCheck(lastMoveDirection);
			faceingDir = (FaceingDirection)DirectionCheck(faceingDirection);
			moveDir = (MoveDirection)DirectionCheck(unmodifiedMoveDirection);

			MovementInputs();
		}

		void FixedUpdate()
		{
			bool hasInput = x != 0 && y != 0;

			if (hasInput)
			{
				x *= MOVE_LIMITER;
				y *= MOVE_LIMITER;
			}

			if (isWalkingOnIce)
			{
				// Make the player slide on ice
				rigidbody2D.AddForce(new Vector2(moveDirection.x * playerSpeed, moveDirection.y * playerSpeed));
			}
			else
			{
				rigidbody2D.velocity = new Vector2(moveDirection.x * playerSpeed, moveDirection.y * playerSpeed);
			}
		}

		private void MovementInputs()
		{
			if (GameManager.INSTANCE.CurrentGameState == GameManager.GameState.Paused) return;

			x = Input.GetAxisRaw("Horizontal");
			y = Input.GetAxisRaw("Vertical");

			if (playerManager.playerAttack.IsAttacking) return;

			if (IsMoveing) lastMoveDirection = moveDirection;

			moveDirection = new Vector2(x, y).normalized;
			unmodifiedMoveDirection = moveDirection;
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
