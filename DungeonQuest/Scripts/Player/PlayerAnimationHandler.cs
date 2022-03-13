using UnityEngine;

namespace DungeonQuest.Player
{
	public class PlayerAnimationHandler : MonoBehaviour
	{
		private Animator playerAnimation;
		private PlayerManager playerManager;

		void Awake()
		{
			playerAnimation = GetComponent<Animator>();
			playerManager = GetComponent<PlayerManager>();
		}
		
		void Update()
		{
			playerAnimation.SetBool("AnimAttack", playerManager.playerAttack.IsAttacking);
			playerAnimation.SetBool("AnimDeath", playerManager.IsDead);
		}

		public void Animate()
		{
			playerAnimation.SetFloat("AnimMoveX", playerManager.MoveDirection.x);
			playerAnimation.SetFloat("AnimMoveY", playerManager.MoveDirection.y);

			playerAnimation.SetFloat("AnimLastMoveX", playerManager.LastMoveDirection.x);
			playerAnimation.SetFloat("AnimLastMoveY", playerManager.LastMoveDirection.y);

			playerAnimation.SetFloat("AnimAttackDirX", playerManager.FaceingDirection.x);
			playerAnimation.SetFloat("AnimAttackDirY", playerManager.FaceingDirection.y);

			playerAnimation.SetFloat("AnimMoveMagnitude", playerManager.MoveDirection.magnitude);

			playerAnimation.SetBool("AnimIsMoveing", playerManager.HasMovementInput);
		}
	}
}
