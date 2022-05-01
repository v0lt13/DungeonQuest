using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Player;
using DungeonQuest.GameEvents;

namespace DungeonQuest.Enemy.Boss
{
	public class BossManager : MonoBehaviour
	{
		public enum PlayerDirection
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

		[Header("Boss Config:")]
		[SerializeField] private float followDistance;
		[SerializeField] private float attackDistance;
		[SerializeField] private int bossHealth;
		[Space(10f)]
		[SerializeField] private Slider healthBar;
		[SerializeField]  private VoidEvent gameEvent;

		[Header("Audio Config:")]
		[SerializeField] private AudioClip deathSFX;
		[SerializeField] private AudioClip damagedSFX;

		[HideInInspector] public LastMoveDirection lastMoveDir;
		[HideInInspector] public PlayerDirection playerDir;
		[HideInInspector] public MoveDirection moveDir;

		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public BossDrops bossDrops;
		[HideInInspector] public BossAI bossAI;

		private Vector2 lastMoveDirection;
		private Vector2 playerDirection;
		private Vector2 moveDirection;

		public int GetEnemyHealth { get { return bossHealth; } }

		public bool IsAwake { get; set; }
		public bool IsDead { get; private set; }

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

			bossAI = GetComponent<BossAI>();
			bossDrops = GetComponent<BossDrops>();
		}

		void Start()
		{
			healthBar.maxValue = bossHealth;

			GameManager.INSTANCE.enemyList.Add(gameObject);
			GameManager.INSTANCE.totalKillCount++;

			gameObject.transform.SetParent(GameObject.Find("EnemyHolder").transform);
		}

		void Update()
		{
			healthBar.value = bossHealth;

			if (bossHealth <= 0)
			{
				bossHealth = 0;
				Die();
			}

			if (playerManager.isDead || !IsAwake)
			{
				bossAI.state = BossAI.AIstate.Idle;
				return;
			}

			playerDirection = transform.InverseTransformPoint(playerManager.transform.position);

			if (bossAI.path != null)
			{
				try
				{
					moveDirection = transform.InverseTransformPoint(new Vector2(bossAI.path[1].x, bossAI.path[1].y) * 10f + Vector2.one * 5f).normalized;
				}
				catch (System.ArgumentOutOfRangeException)
				{
					moveDirection = transform.InverseTransformPoint(playerManager.transform.position).normalized;
				}
			}

			var isMoveing = moveDirection.x != 0 || moveDirection.y != 0;

			if (isMoveing)
			{
				lastMoveDirection = moveDirection;
			}

			// Cast the return value of DirectionCheck() to enums
			lastMoveDir = (LastMoveDirection)DirectionCheck(lastMoveDirection);
			playerDir = (PlayerDirection)DirectionCheck(playerDirection.normalized);
			moveDir = (MoveDirection)DirectionCheck(moveDirection);

			SetAIState();
		}

		public void DamageBoss(int damage)
		{
			if (bossHealth > 0)
			{
				bossHealth -= damage;

				audio.clip = damagedSFX;
				audio.pitch = Random.Range(0.7f, 1.3f);
				audio.Play();
			}
		}

		private void Die()
		{
			IsDead = true;
			rigidbody2D.isKinematic = true;
			GetComponent<SpriteRenderer>().sortingOrder = 0;

			GameManager.INSTANCE.killCount++;
			GameManager.INSTANCE.enemyList.Remove(gameObject);

			audio.clip = deathSFX;
			audio.pitch = 1f;
			audio.Play();

			gameEvent.Invoke();
			bossDrops.DropLoot();

			Destroy(collider2D);
			Destroy(bossAI);
			Destroy(this);
			Destroy(gameObject, 5f);
		}

		private void SetAIState()
		{
			float distanceFromPlayer = Vector2.Distance(transform.position, playerManager.transform.position);

			if (!playerManager.invisible)
			{
				if (bossAI.timeBetweenSpecials <= 0f)
				{
					bossAI.state = BossAI.AIstate.Special;
				}
				else if (distanceFromPlayer <= followDistance && distanceFromPlayer > attackDistance && bossAI.StunTime == 0f)
				{
					bossAI.state = BossAI.AIstate.Chase;
				}
				else if (distanceFromPlayer <= attackDistance && bossAI.StunTime == 0f)
				{
					bossAI.state = BossAI.AIstate.Attack;
				}
				else
				{
					bossAI.state = BossAI.AIstate.Idle;
				}
			}
			else
			{
				bossAI.state = BossAI.AIstate.Idle;
			}
		}


		private int DirectionCheck(Vector2 direction)
		{
			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				if (direction.x > 0)
				{
					return 3;
				}
				else
				{
					return 2;
				}
			}
			else
			{
				if (direction.y > 0)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
		}
	}
}
