using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy
{
	public class EnemyManager : MonoBehaviour
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

		[Header("Enemy Config:")]
		public int enemyLevel = 1;
		[SerializeField] private float followDistance;
		[SerializeField] private float attackDistance;
		[SerializeField] private int enemyHealth;

		[Space(10f)]
		[SerializeField] private AudioClip deathSFX;

		[HideInInspector] public LastMoveDirection lastMoveDir;
		[HideInInspector] public PlayerDirection playerDir;
		[HideInInspector] public MoveDirection moveDir;

		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public EnemyDrops enemyDrops;
		[HideInInspector] public EnemyAI enemyAI;

		private Slider healthBar;
		private Text levelText;

		private Vector2 lastMoveDirection;
		private Vector2 playerDirection;
		private Vector2 moveDirection;

		public int GetEnemyHealth {	get { return enemyHealth; } }

		public bool IsDead { get; private set; }
		public bool IsAttacking { get; set; }

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

			enemyAI = GetComponent<EnemyAI>();
			enemyDrops = GetComponent<EnemyDrops>();
			healthBar = GetComponentInChildren<Slider>();
			levelText = GetComponentInChildren<Text>();
		}

		void Start()
		{
			enemyHealth += enemyLevel * 10;
			enemyAI.damage += enemyLevel * 5;

			healthBar.maxValue = enemyHealth;
			levelText.text = "Lvl " + enemyLevel;
		}
		
		void Update()
		{
			healthBar.value = enemyHealth;

			if (enemyHealth <= 0)
			{
				enemyHealth = 0;
				Die();
			}

			if (playerManager.isDead)
			{
				enemyAI.state = EnemyAI.AIstate.Idle;
				return;
			}
			
			playerDirection = transform.InverseTransformPoint(playerManager.transform.position);

			if (enemyAI.path != null)
			{
				try
				{
					moveDirection = transform.InverseTransformPoint(new Vector2(enemyAI.path[1].x, enemyAI.path[1].y) * 10f + Vector2.one * 5f).normalized;
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
			
			lastMoveDir = (LastMoveDirection)DirectionCheck(lastMoveDirection);
			playerDir = (PlayerDirection)DirectionCheck(playerDirection.normalized);
			moveDir = (MoveDirection)DirectionCheck(moveDirection);

			SetAIState();
		}

		public void DamageEnemy(int damage)
		{
			if (enemyHealth > 0) enemyHealth -= damage;
		}

		private void Die()
		{
			IsDead = true;
			rigidbody2D.isKinematic = true;
			GetComponent<SpriteRenderer>().sortingOrder = 0;

			GameManager.INSTANCE.KillCount++;
			GameManager.INSTANCE.enemyList.Remove(gameObject);

			var deathSound = GetComponent<AudioSource>();
			deathSound.clip = deathSFX;
			deathSound.Play();

			healthBar.gameObject.SetActive(false);
			levelText.gameObject.SetActive(false);

			enemyDrops.DropLoot();
			Destroy(GetComponent<CircleCollider2D>());
			Destroy(enemyAI);
			Destroy(this);
			Destroy(gameObject, 5f);
		}

		private void SetAIState()
		{
			float distanceFromPlayer = Vector2.Distance(transform.position, playerManager.transform.position);

			if (!playerManager.invisible && !playerManager.noClip)
			{
				if (distanceFromPlayer <= followDistance && distanceFromPlayer > attackDistance)
				{
					enemyAI.state = EnemyAI.AIstate.Chase;
				}
				else if (distanceFromPlayer <= attackDistance)
				{
					enemyAI.state = EnemyAI.AIstate.Attack;
				}
				else
				{
					enemyAI.state = EnemyAI.AIstate.Idle;
				}
			}
			else
			{
				enemyAI.state = EnemyAI.AIstate.Idle;
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
