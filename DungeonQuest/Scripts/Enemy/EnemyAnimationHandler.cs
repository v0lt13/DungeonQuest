using UnityEngine;

namespace DungeonQuest.Enemy
{
	public class EnemyAnimationHandler : MonoBehaviour
	{
		private Animator enemyAnimation;
		private EnemyManager enemyManager;

		void Awake() 
		{
			enemyAnimation = GetComponent<Animator>();
			enemyManager = GetComponent<EnemyManager>();
		}

		void Update()
		{
			enemyAnimation.SetBool("AnimAttack", enemyManager.IsAttacking);
			enemyAnimation.SetBool("AnimDeath", enemyManager.IsDead);

			switch (enemyManager.enemyAI.state)
			{
				case EnemyAI.AIstate.Idle:
					enemyAnimation.SetFloat("AnimMoveMagnitude", 0);
					break;
				case EnemyAI.AIstate.Attack:
					enemyAnimation.SetFloat("AnimMoveMagnitude", 0);
					break;
			}

			if (enemyManager.IsAttacking)
			{
				enemyAnimation.SetFloat("AnimAttackDirX", enemyManager.PlayerDirection.x);
				enemyAnimation.SetFloat("AnimAttackDirY", enemyManager.PlayerDirection.y);
			}
		}

		public void Animate()
		{
			enemyAnimation.SetFloat("AnimMoveX", enemyManager.MoveDirection.x);
			enemyAnimation.SetFloat("AnimMoveY", enemyManager.MoveDirection.y);

			enemyAnimation.SetFloat("AnimLastMoveX", enemyManager.LastMoveDirection.x);
			enemyAnimation.SetFloat("AnimLastMoveY", enemyManager.LastMoveDirection.y);

			enemyAnimation.SetFloat("AnimMoveMagnitude", enemyManager.MoveDirection.magnitude);
		}
	}
}
