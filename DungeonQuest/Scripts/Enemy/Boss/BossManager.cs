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
		[SerializeField] private int bossID;
		[Space]
		[SerializeField] private Vector2 boxColliderCenterX;
		[SerializeField] private Vector2 boxColliderSizeX;
		[SerializeField] private Vector2 boxColliderCenterY;
		[SerializeField] private Vector2 boxColliderSizeY;
		[Space]
		[SerializeField] private Slider healthBar;
		[SerializeField]  private VoidEvent gameEvent;

		[Header("Audio Config:")]
		[SerializeField] private AudioClip deathSFX;
		[SerializeField] private AudioClip damagedSFX;

		[HideInInspector] public LastMoveDirection lastMoveDir;
		[HideInInspector] public PlayerDirection playerDir;
		[HideInInspector] public MoveDirection moveDir;

		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public AudioSource audioSource;
		[HideInInspector] public Rigidbody2D bossRigidbody;
		[HideInInspector] public BossDrops bossDrops;
		[HideInInspector] public BossAI bossAI;

		private BoxCollider2D boxCollider;
		private SpriteRenderer spriteRenderer;

		private Vector2 lastMoveDirection;
		private Vector2 playerDirection;
		private Vector2 moveDirection;

		public int GetBossID { get { return bossID; } }

		public bool IsAwake { get; private set; }
		public bool IsDead { get; private set; }

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

			bossAI = GetComponent<BossAI>();
			bossDrops = GetComponent<BossDrops>();
			audioSource = GetComponent<AudioSource>();
			boxCollider = GetComponent<BoxCollider2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			bossRigidbody = GetComponent<Rigidbody2D>();
		}

		void Start()
		{
			healthBar.maxValue = bossHealth;

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

			switch (playerDir)
			{
				case PlayerDirection.DOWN:
					spriteRenderer.sortingOrder = 1;
					break;

				case PlayerDirection.UP:
					spriteRenderer.sortingOrder = 4;
					break;
			}

			if (!IsAwake)
			{
				return;
			}

			if (playerManager.isDead)
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

				audioSource.clip = damagedSFX;
				audioSource.pitch = Random.Range(0.7f, 1.3f);
				audioSource.Play();
			}
		}

		private void Die()
		{
			var gameManager = GameManager.INSTANCE;

			IsDead = true;
			bossRigidbody.isKinematic = true;
			spriteRenderer.sortingOrder = 0;

			gameManager.killCount++;
			gameManager.enemyList.Remove(gameObject);

			// Kill all enemies on death
			for (int i = 0; i < gameManager.enemyList.Count; i++) gameManager.enemyList[i].GetComponent<EnemyManager>().DamageEnemy(int.MaxValue);

			gameManager.enemyList.Clear();
			
			audioSource.clip = deathSFX;
			audioSource.pitch = 1f;
			audioSource.Play();

			gameEvent.Invoke();
			bossDrops.DropLoot();

			if (gameManager.bossesCompleted < bossID) gameManager.bossesCompleted = bossID;

			Destroy(boxCollider);
			Destroy(bossAI);
			Destroy(this);
		}

		private void SetAIState()
		{
			float distanceFromPlayer = Vector2.Distance(transform.position, playerManager.transform.position);

			if (!playerManager.Invisible)
			{
				if (bossAI.timeBetweenSpecials <= 0f)
				{
					bossAI.state = BossAI.AIstate.Special;
				}
				else if (distanceFromPlayer <= followDistance && distanceFromPlayer > attackDistance)
				{
					bossAI.state = BossAI.AIstate.Chase;
				}
				else if (distanceFromPlayer <= attackDistance)
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
				boxCollider.size = boxColliderSizeX;

				if (direction.x > 0)
				{
					boxCollider.offset = new Vector2(-boxColliderCenterX.x, boxColliderCenterX.y);
					return 3; // Right
				}
				else
				{
					boxCollider.offset = new Vector2(boxColliderCenterX.x, boxColliderCenterX.y);
					return 2; // Left
				}
			}
			else
			{
				boxCollider.offset = boxColliderCenterY;
				boxCollider.size = boxColliderSizeY;

				if (direction.y > 0)
				{
					return 1; // Up
				}
				else
				{
					return 0; // Down
				}
			}
		}

		public void WakeUp() // Called By Animation Event
		{
			IsAwake = true;
		}
	}
}
