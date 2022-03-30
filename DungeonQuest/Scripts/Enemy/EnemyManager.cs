using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
		[SerializeField] private float followDistance;
		[SerializeField] private float attackDistance;
		[SerializeField] private int enemyHealth;
		[SerializeField] private int healthDropChance;
		[SerializeField] private int coinDropChance;
		[SerializeField] private int pileOfCoinsDropChance;
		[SerializeField] private int minXpDrop;
		[SerializeField] private int maxXpDrop;
		[Space(10f)]
		[SerializeField] private GameObject healthPotionPrefab;
		[SerializeField] private GameObject pileOfCoinsPrefab;
		[SerializeField] private GameObject coinsPrefab;
		[SerializeField] private AudioClip deathSFX;

		[HideInInspector] public LastMoveDirection lastMoveDir;
		[HideInInspector] public PlayerDirection playerDir;
		[HideInInspector] public MoveDirection moveDir;

		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public GameObject player;
		[HideInInspector] public EnemyAI enemyAI;

		private Slider healthBar;

		private Vector2 lastMoveDirection;
		private Vector2 playerDirection;
		private Vector2 moveDirection;

		public bool IsDead { get; private set; }
		public bool IsAttacking { get; set; }

		void Awake()
		{
			player = GameObject.Find("Player");

			enemyAI = GetComponent<EnemyAI>();
			playerManager = player.GetComponent<PlayerManager>();
			healthBar = GetComponentInChildren<Slider>();

			healthBar.maxValue = enemyHealth;
		}
		
		void Update()
		{
			healthBar.value = enemyHealth;
			playerDirection = transform.InverseTransformPoint(player.transform.position);

			if (enemyHealth <= 0)
			{
				enemyHealth = 0;
				Die();
			}

			if (enemyAI.path != null)
			{
				try
				{
					moveDirection = transform.InverseTransformPoint(new Vector2(enemyAI.path[1].x, enemyAI.path[1].y) * 10f + Vector2.one * 5f).normalized;
				}
				catch (System.ArgumentOutOfRangeException)
				{
					moveDirection = transform.InverseTransformPoint(player.transform.position).normalized;
				}
			}

			var isMoveing = moveDirection.x != 0 || moveDirection.y != 0;

			if (isMoveing)
			{
				lastMoveDirection = moveDirection;
			}
			else
			{
				lastMoveDirection = playerDirection.normalized;
			}

			SetAIState();

			lastMoveDir = (LastMoveDirection)DirectionCheck(lastMoveDirection);
			playerDir = (PlayerDirection)DirectionCheck(playerDirection.normalized);
			moveDir = (MoveDirection)DirectionCheck(moveDirection);
		}

		public void DamageEnemy(int damage)
		{
			if (enemyHealth > 0) enemyHealth -= damage;
		}

		private void SetAIState()
		{
			float distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);

			if (distanceFromPlayer <= followDistance && distanceFromPlayer > attackDistance && enemyAI.path != null && !playerManager.IsDead && enemyAI.StunTime == 0f)
			{
				enemyAI.state = EnemyAI.AIstate.Chase;
				rigidbody2D.velocity = Vector2.zero;
				IsAttacking = false;
			}
			else if (distanceFromPlayer <= attackDistance && !playerManager.IsDead && !playerManager.Invisible)
			{
				enemyAI.state = EnemyAI.AIstate.Attack;
			}
			else
			{
				enemyAI.state = EnemyAI.AIstate.Idle;

				IsAttacking = false;
			}
		}

		private void Die()
		{
			IsDead = true;
			rigidbody2D.isKinematic = true;
			GetComponent<SpriteRenderer>().sortingOrder = 0;

			GameManager.INSTANCE.KillCount++;

			var deathSound = GetComponent<AudioSource>();
			deathSound.clip = deathSFX;
			deathSound.Play();

			healthBar.gameObject.SetActive(false);

			DropLoot();
			Destroy(GetComponent<CircleCollider2D>());
			Destroy(enemyAI);
			Destroy(this);
			Destroy(gameObject, 5f);
		}

		private void DropLoot()
		{
			playerManager.playerLeveling.PlayerXP += Random.Range(minXpDrop, maxXpDrop);

			var dropChance = Random.Range(1, 100);

			if (dropChance <= healthDropChance) 
				Instantiate(healthPotionPrefab, new Vector2(transform.position.x + Random.Range(-5f, 5f), transform.position.y), Quaternion.identity);

			if (dropChance <= coinDropChance)
				Instantiate(coinsPrefab, new Vector2(transform.position.x + Random.Range(-5f, 5f), transform.position.y), Quaternion.identity);
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
