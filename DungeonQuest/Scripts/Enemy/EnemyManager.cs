using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy
{
	public class EnemyManager : MonoBehaviour
	{
		[Header("Enemy Config:")]
		[SerializeField] private float followDistance;
		[SerializeField] private float attackDistance;
		[SerializeField] private int enemyHealth;
		[Space(10f)]
		[SerializeField] private AudioClip deathSFX;

		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public GameObject player;
		[HideInInspector] public EnemyAI enemyAI;

		private Slider healthBar;

		public Vector2 LastMoveDirection { get; private set; }
		public Vector2 PlayerDirection { get; private set; }
		public Vector2 MoveDirection { get; private set; }

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
			PlayerDirection = transform.InverseTransformPoint(player.transform.position);

			if (MoveDirection.x != 0 || MoveDirection.y != 0)
			{
				LastMoveDirection = MoveDirection;
			}

			if (enemyHealth <= 0)
			{
				enemyHealth = 0;
				Die();
			}

			if (enemyAI.path != null)
			{
				try
				{
					MoveDirection = transform.InverseTransformPoint(new Vector2(enemyAI.path[1].x, enemyAI.path[1].y) * 10f + Vector2.one * 5f).normalized;
				}
				catch (System.ArgumentOutOfRangeException)
				{
					MoveDirection = transform.InverseTransformPoint(player.transform.position).normalized;
				}
			}

			SetAIState();
		}

		public void DamageEnemy(int damage)
		{
			if (enemyHealth > 0)
			{
				enemyHealth -= damage;
			}
		}

		private void SetAIState()
		{
			float distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);

			if (distanceFromPlayer <= followDistance && distanceFromPlayer > attackDistance && enemyAI.path != null && !playerManager.IsDead && enemyAI.StunTime == 0)
			{
				enemyAI.state = EnemyAI.AIstate.Chase;
				rigidbody2D.velocity = Vector2.zero;
				IsAttacking = false;

				GetComponent<EnemyAnimationHandler>().Animate();
			}
			else if (distanceFromPlayer <= attackDistance && !playerManager.IsDead && enemyAI.StunTime == 0)
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

			var deathSound = GetComponent<AudioSource>();
			deathSound.clip = deathSFX;
			deathSound.Play();

			healthBar.gameObject.SetActive(false);

			Destroy(GetComponent<BoxCollider2D>());
			Destroy(enemyAI);
			Destroy(this);
			Destroy(gameObject, 5f);
		}
	}
}
