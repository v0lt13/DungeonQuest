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

		[Header("Audio Config:")]
		[SerializeField] private AudioClip deathSFX;
		[SerializeField] private AudioClip damagedSFX;
		public AudioClip shootSFX;

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
			// Set the enemy health and damage depending on it's level
			enemyHealth += enemyLevel * 20;
			enemyAI.damage += enemyLevel * 5;

			healthBar.maxValue = enemyHealth;
			levelText.text = "Lvl " + enemyLevel;

			GameManager.INSTANCE.enemyList.Add(gameObject);
			GameManager.INSTANCE.totalKillCount++;

			gameObject.transform.SetParent(GameObject.Find("EnemyHolder").transform);
		}
		
		void Update()
		{
			healthBar.value = enemyHealth;

			if (enemyHealth <= 0)
			{
				enemyHealth = 0;
				Die();
			}

			switch (playerDir)
			{
				case PlayerDirection.DOWN:
					renderer.sortingOrder = 0;
					break;

				case PlayerDirection.UP:
					renderer.sortingOrder = 3;
					break;
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
			
			// Cast the return value of DirectionCheck() to enums
			lastMoveDir = (LastMoveDirection)DirectionCheck(lastMoveDirection);
			playerDir = (PlayerDirection)DirectionCheck(playerDirection.normalized);
			moveDir = (MoveDirection)DirectionCheck(moveDirection);

			SetAIState();
		}

		public void DamageEnemy(int damage)
		{
			if (enemyHealth > 0)
			{
				enemyHealth -= damage;

				audio.clip = damagedSFX;
				audio.pitch = Random.Range(0.7f, 1.3f);
				audio.Play();
			}
		}

		private void Die()
		{
			IsDead = true;
			rigidbody2D.isKinematic = true;
			renderer.sortingOrder = 0;

			GameManager.INSTANCE.killCount++;
			GameManager.INSTANCE.enemyList.Remove(gameObject);

			audio.clip = deathSFX;
			audio.pitch = 1f;
			audio.Play();

			healthBar.gameObject.SetActive(false);
			levelText.gameObject.SetActive(false);

			enemyDrops.DropLoot();
			Destroy(collider2D);
			Destroy(enemyAI);
			Destroy(this);
			Destroy(gameObject, 5f);
		}

		private void SetAIState()
		{
			float distanceFromPlayer = Vector2.Distance(transform.position, playerManager.transform.position);

			if (!playerManager.Invisible)
			{
				if (distanceFromPlayer <= followDistance && distanceFromPlayer > attackDistance && enemyAI.StunTime == 0f)
				{
					enemyAI.state = EnemyAI.AIstate.Chase;
				}
				else if (distanceFromPlayer <= attackDistance && enemyAI.StunTime == 0f)
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
