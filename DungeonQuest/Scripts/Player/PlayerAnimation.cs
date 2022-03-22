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

		private Animator playerAnimation;
		private PlayerManager playerManager;

		void Awake()
		{
			playerAnimation = GetComponent<Animator>();
			playerManager = GetComponent<PlayerManager>();
		}
		
		void Update()
		{
			if (playerManager.IsDead)
			{
				playerAnimation.Play(PLAYER_DEATH);
				return;
			}

			if (playerManager.playerAttack.IsAttacking)
			{
				switch (playerManager.faceingDir)
				{
					case PlayerManager.FaceingDirection.DOWN:
						playerAnimation.Play(PLAYER_ATTACK_DOWN);
						break;
					case PlayerManager.FaceingDirection.UP:
						playerAnimation.Play(PLAYER_ATTACK_UP);
						break;
					case PlayerManager.FaceingDirection.LEFT:
						playerAnimation.Play(PLAYER_ATTACK_LEFT);
						break;
					case PlayerManager.FaceingDirection.RIGHT:
						playerAnimation.Play(PLAYER_ATTACK_RIGHT);
						break;
				}
			}

			if (playerManager.HasMovementInput && !playerManager.playerAttack.IsAttacking)
			{
				switch (playerManager.moveDir)
				{
					case PlayerManager.MoveDirection.DOWN:
						playerAnimation.Play(PLAYER_MOVEMENT_DOWN);
						break;
					case PlayerManager.MoveDirection.UP:
						playerAnimation.Play(PLAYER_MOVEMENT_UP);
						break;
					case PlayerManager.MoveDirection.LEFT:
						playerAnimation.Play(PLAYER_MOVEMENT_LEFT);
						break;
					case PlayerManager.MoveDirection.RIGHT:
						playerAnimation.Play(PLAYER_MOVEMENT_RIGHT);
						break;
				}
			}
			else if (!playerManager.HasMovementInput && !playerManager.playerAttack.IsAttacking)
			{
				switch (playerManager.lastMoveDir)
				{
					case PlayerManager.LastMoveDirection.DOWN:
						playerAnimation.Play(PLAYER_IDLE_DOWN);
						break;
					case PlayerManager.LastMoveDirection.UP:
						playerAnimation.Play(PLAYER_IDLE_UP);
						break;
					case PlayerManager.LastMoveDirection.LEFT:
						playerAnimation.Play(PLAYER_IDLE_LEFT);
						break;
					case PlayerManager.LastMoveDirection.RIGHT:
						playerAnimation.Play(PLAYER_IDLE_RIGHT);
						break;
				}
			}
		}
	}
}
