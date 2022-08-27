using UnityEngine;

namespace DungeonQuest.Enemy.Boss
{
	public class BossAnimation : MonoBehaviour
	{
		private const string BOSS_ATTACK_DOWN = "BossAttackDown";
		private const string BOSS_ATTACK_UP = "BossAttackUp";
		private const string BOSS_ATTACK_LEFT = "BossAttackLeft";
		private const string BOSS_ATTACK_RIGHT = "BossAttackRight";
		private const string BOSS_IDLE_DOWN = "BossIdleDown";
		private const string BOSS_IDLE_UP = "BossIdleUp";
		private const string BOSS_IDLE_LEFT = "BossIdleLeft";
		private const string BOSS_IDLE_RIGHT = "BossIdleRight";
		private const string BOSS_MOVEMENT_DOWN = "BossMovementDown";
		private const string BOSS_MOVEMENT_UP = "BossMovementUp";
		private const string BOSS_MOVEMENT_LEFT = "BossMovementLeft";
		private const string BOSS_MOVEMENT_RIGHT = "BossMovementRight";
		private const string BOSS_SPECIAL_DOWN = "BossSpecialDown";
		private const string BOSS_SPECIAL_UP = "BossSpecialUp";
		private const string BOSS_SPECIAL_LEFT = "BossSpecialLeft";
		private const string BOSS_SPECIAL_RIGHT = "BossSpecialRight";
		private const string BOSS_DEATH = "BossDeath";
		private const string BOSS_WAKE = "BossWake";

		[HideInInspector] public Animator bossAnimator;

		private BossManager bossManager;

		void Awake()
		{
			bossAnimator = GetComponent<Animator>();
			bossManager = GetComponent<BossManager>();
		}

		void Update()
		{
			if (!bossManager.IsAwake) return;

			if (bossManager.IsDead)
			{
				bossAnimator.Play(BOSS_DEATH);
				return;
			}

			switch (bossManager.bossAI.state)
			{
				default:
				case BossAI.AIstate.Idle:

					switch (bossManager.lastMoveDir)
					{
						case BossManager.LastMoveDirection.DOWN:
							bossAnimator.Play(BOSS_IDLE_DOWN);
							break;
						case BossManager.LastMoveDirection.UP:
							bossAnimator.Play(BOSS_IDLE_UP);
							break;
						case BossManager.LastMoveDirection.LEFT:
							bossAnimator.Play(BOSS_IDLE_LEFT);
							break;
						case BossManager.LastMoveDirection.RIGHT:
							bossAnimator.Play(BOSS_IDLE_RIGHT);
							break;
					}
					break;

				case BossAI.AIstate.Chase:
					if (bossManager.bossAI.path == null) return;

					switch (bossManager.moveDir)
					{
						case BossManager.MoveDirection.DOWN:
							bossAnimator.Play(BOSS_MOVEMENT_DOWN);
							break;
						case BossManager.MoveDirection.UP:
							bossAnimator.Play(BOSS_MOVEMENT_UP);
							break;
						case BossManager.MoveDirection.LEFT:
							bossAnimator.Play(BOSS_MOVEMENT_LEFT);
							break;
						case BossManager.MoveDirection.RIGHT:
							bossAnimator.Play(BOSS_MOVEMENT_RIGHT);
							break;
					}
					break;

				case BossAI.AIstate.Attack:
					// Check if an animation is already playing
					if (bossAnimator.GetCurrentAnimatorClipInfo(0).Length > bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime) return;

					switch (bossManager.playerDir)
					{
						case BossManager.PlayerDirection.DOWN:
							bossAnimator.Play(BOSS_ATTACK_DOWN);
							break;
						case BossManager.PlayerDirection.UP:
							bossAnimator.Play(BOSS_ATTACK_UP);
							break;
						case BossManager.PlayerDirection.LEFT:
							bossAnimator.Play(BOSS_ATTACK_LEFT);
							break;
						case BossManager.PlayerDirection.RIGHT:
							bossAnimator.Play(BOSS_ATTACK_RIGHT);
							break;
					}
					break;

				case BossAI.AIstate.Special:
					// Check if an animation is already playing
					if (bossAnimator.GetCurrentAnimatorClipInfo(0).Length > bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime) return;

					switch (bossManager.playerDir)
					{
						case BossManager.PlayerDirection.DOWN:
							bossAnimator.Play(BOSS_SPECIAL_DOWN);
							break;
						case BossManager.PlayerDirection.UP:
							bossAnimator.Play(BOSS_SPECIAL_UP);
							break;
						case BossManager.PlayerDirection.LEFT:
							bossAnimator.Play(BOSS_SPECIAL_LEFT);
							break;
						case BossManager.PlayerDirection.RIGHT:
							bossAnimator.Play(BOSS_SPECIAL_RIGHT);
							break;
					}
					break;
			}
		}

		public void WakeBoss() // Called By Event
		{
			bossAnimator.Play(BOSS_WAKE);
		}
	}
}
