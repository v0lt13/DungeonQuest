using UnityEngine;

namespace DungeonQuest.Player
{
	public class PlayerAnimation : MonoBehaviour
	{
		private const string PLAYER_ATTACK_DOWN = "PlayerAttackDown";
		private const string PLAYER_ATTACK_UP = "PlayerAttackUp";
		private const string PLAYER_ATTACK_LEFT = "PlayerAttackLeft";
		private const string PLAYER_ATTACK_RIGHT = "PlayerAttackRight";		
		private const string PLAYER_IDLE_DOWN = "PlayerIdleDown";
		private const string PLAYER_IDLE_UP = "PlayerIdleUp";
		private const string PLAYER_IDLE_LEFT = "PlayerIdleLeft";
		private const string PLAYER_IDLE_RIGHT = "PlayerIdleRight";
		private const string PLAYER_MOVEMENT_DOWN = "PlayerMovementDown";
		private const string PLAYER_MOVEMENT_UP = "PlayerMovementUp";
		private const string PLAYER_MOVEMENT_LEFT = "PlayerMovementLeft";
		private const string PLAYER_MOVEMENT_RIGHT = "PlayerMovementRight";	
		private const string PLAYER_DEATH = "PlayerDeath";

		private PlayerManager playerManager;
		private Animator playerAnimation;

		void Awake()
		{
			playerManager = GetComponent<PlayerManager>();
			playerAnimation = GetComponent<Animator>();
		}
		
		void Update()
		{
			if (playerManager.isDead)
			{
				playerAnimation.Play(PLAYER_DEATH);
				return;
			}

			if (playerManager.playerAttack.IsAttacking)
			{
				switch (playerManager.playerMovement.faceingDir)
				{
					case PlayerMovement.FaceingDirection.DOWN:
						playerAnimation.Play(PLAYER_ATTACK_DOWN);
						break;

					case PlayerMovement.FaceingDirection.UP:
						playerAnimation.Play(PLAYER_ATTACK_UP);
						break;

					case PlayerMovement.FaceingDirection.LEFT:
						playerAnimation.Play(PLAYER_ATTACK_LEFT);
						break;

					case PlayerMovement.FaceingDirection.RIGHT:
						playerAnimation.Play(PLAYER_ATTACK_RIGHT);
						break;
				}
			}

			if (playerManager.playerMovement.HasMovementInput && !playerManager.playerAttack.IsAttacking)
			{
				switch (playerManager.playerMovement.moveDir)
				{
					case PlayerMovement.MoveDirection.DOWN:
						playerAnimation.Play(PLAYER_MOVEMENT_DOWN);
						break;

					case PlayerMovement.MoveDirection.UP:
						playerAnimation.Play(PLAYER_MOVEMENT_UP);
						break;

					case PlayerMovement.MoveDirection.LEFT:
						playerAnimation.Play(PLAYER_MOVEMENT_LEFT);
						break;

					case PlayerMovement.MoveDirection.RIGHT:
						playerAnimation.Play(PLAYER_MOVEMENT_RIGHT);
						break;
				}
			}
			else if (!playerManager.playerMovement.HasMovementInput && !playerManager.playerAttack.IsAttacking)
			{
				switch (playerManager.playerMovement.lastMoveDir)
				{
					case PlayerMovement.LastMoveDirection.DOWN:
						playerAnimation.Play(PLAYER_IDLE_DOWN);
						break;

					case PlayerMovement.LastMoveDirection.UP:
						playerAnimation.Play(PLAYER_IDLE_UP);
						break;

					case PlayerMovement.LastMoveDirection.LEFT:
						playerAnimation.Play(PLAYER_IDLE_LEFT);
						break;

					case PlayerMovement.LastMoveDirection.RIGHT:
						playerAnimation.Play(PLAYER_IDLE_RIGHT);
						break;
				}
			}
		}
	}
}
