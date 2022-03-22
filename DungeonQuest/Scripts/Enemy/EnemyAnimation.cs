using UnityEngine;

namespace DungeonQuest.Enemy
{
	public class EnemyAnimation : MonoBehaviour
	{
		private const string ENEMY_ATTACK_DOWN = "EnemyAttackDown";
		private const string ENEMY_ATTACK_UP = "EnemyAttackUp";
		private const string ENEMY_ATTACK_LEFT = "EnemyAttackLeft";
		private const string ENEMY_ATTACK_RIGHT = "EnemyAttackRight";
		private const string ENEMY_IDLE_DOWN = "EnemyIdleDown";
		private const string ENEMY_IDLE_UP = "EnemyIdleUp";
		private const string ENEMY_IDLE_LEFT = "EnemyIdleLeft";
		private const string ENEMY_IDLE_RIGHT = "EnemyIdleRight";
		private const string ENEMY_MOVEMENT_DOWN = "EnemyMovementDown";
		private const string ENEMY_MOVEMENT_UP = "EnemyMovementUp";
		private const string ENEMY_MOVEMENT_LEFT = "EnemyMovementLeft";
		private const string ENEMY_MOVEMENT_RIGHT = "EnemyMovementRight";
		private const string ENEMY_DEATH = "EnemyDeath";

		[HideInInspector] public Animator enemyAnimator;
		private EnemyManager enemyManager;

		void Awake() 
		{
			enemyAnimator = GetComponent<Animator>();
			enemyManager = GetComponent<EnemyManager>();
		}

		void Update()
		{
			if (enemyManager.IsDead)
			{
				enemyAnimator.Play(ENEMY_DEATH);
				return;
			}

			switch (enemyManager.enemyAI.state)
			{
				case EnemyAI.AIstate.Idle:
					switch (enemyManager.lastMoveDir)
					{
						case EnemyManager.LastMoveDirection.DOWN:
							enemyAnimator.Play(ENEMY_IDLE_DOWN);
							break;
						case EnemyManager.LastMoveDirection.UP:
							enemyAnimator.Play(ENEMY_IDLE_UP);
							break;
						case EnemyManager.LastMoveDirection.LEFT:
							enemyAnimator.Play(ENEMY_IDLE_LEFT);
							break;
						case EnemyManager.LastMoveDirection.RIGHT:
							enemyAnimator.Play(ENEMY_IDLE_RIGHT);
							break;
					}
					break;

				case EnemyAI.AIstate.Chase:
					if (enemyManager.IsAttacking) return;

					switch (enemyManager.moveDir)
					{
						case EnemyManager.MoveDirection.DOWN:
							enemyAnimator.Play(ENEMY_MOVEMENT_DOWN);
							break;
						case EnemyManager.MoveDirection.UP:
							enemyAnimator.Play(ENEMY_MOVEMENT_UP);
							break;
						case EnemyManager.MoveDirection.LEFT:
							enemyAnimator.Play(ENEMY_MOVEMENT_LEFT);
							break;
						case EnemyManager.MoveDirection.RIGHT:
							enemyAnimator.Play(ENEMY_MOVEMENT_RIGHT);
							break;
					}
					break;

				case EnemyAI.AIstate.Attack:

					if (!enemyManager.IsAttacking) return;

					switch (enemyManager.playerDir)
					{
						case EnemyManager.PlayerDirection.DOWN:
							enemyAnimator.Play(ENEMY_ATTACK_DOWN);
							break;
						case EnemyManager.PlayerDirection.UP:
							enemyAnimator.Play(ENEMY_ATTACK_UP);
							break;
						case EnemyManager.PlayerDirection.LEFT:
							enemyAnimator.Play(ENEMY_ATTACK_LEFT);
							break;
						case EnemyManager.PlayerDirection.RIGHT:
							enemyAnimator.Play(ENEMY_ATTACK_RIGHT);
							break;
					}
					break;
			}
		}
	}
}
