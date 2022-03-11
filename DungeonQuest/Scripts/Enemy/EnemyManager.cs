using UnityEngine;
using System.Collections;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy
{
	public class EnemyManager : MonoBehaviour
	{
		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public GameObject player;

		private Vector2 moveDirection;
		private Vector2 pathDirection;
		private Vector2 lastMoveDirection;
		private Vector2 playerDirection;
		private Animator enemyAnimation;
		private EnemyAI enemyAI;

		[Header("EnemyConfig:")]
		[SerializeField] private float followDistance;
		[SerializeField] private float attackDistance;
		[SerializeField] private float enemyHealth;

		private bool isAttacking;

		void Awake()
		{
			player = GameObject.Find("Player");

			enemyAnimation = GetComponent<Animator>();
			enemyAI = GetComponent<EnemyAI>();
			playerManager = player.GetComponent<PlayerManager>();			
		}
		
		void Update()
		{
			SetAIState();

			playerDirection = transform.InverseTransformPoint(player.transform.position);

			if (moveDirection.x != 0 || moveDirection.y != 0)
			{
				lastMoveDirection = moveDirection;
			}

			if (enemyHealth <= 0)
			{
				enemyHealth = 0;
				Die();
			}

			if (playerDirection.y > 0)
			{
				GetComponent<SpriteRenderer>().sortingOrder = 2;
			}
			else if (playerDirection.y < 0)
			{
				GetComponent<SpriteRenderer>().sortingOrder = 0;
			}

			if (enemyAI.path != null)
			{
				try
				{
					moveDirection = transform.InverseTransformPoint(new Vector3(enemyAI.path[1].x, enemyAI.path[1].y) * 10f + Vector3.one * 5f).normalized;
				}
				catch (System.ArgumentOutOfRangeException)
				{
					moveDirection = transform.InverseTransformPoint(player.transform.position);
				}
			}
		}

		public void DamageEnemy(float damage)
		{
			if (enemyHealth > 0)
			{
				enemyHealth -= damage;
			}
		}

		private void SetAIState()
		{
			float distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);

			if (distanceFromPlayer <= followDistance && distanceFromPlayer >= attackDistance && enemyAI.path != null && !playerManager.PlayerDied && enemyAI.StunTime == 0)
			{
				enemyAI.state = EnemyAI.AIstate.Chase;
				rigidbody2D.velocity = Vector2.zero;

				enemyAnimation.SetBool("AnimAttack", false);
				Animate();
			}
			else if (distanceFromPlayer <= attackDistance && !playerManager.PlayerDied && enemyAI.StunTime == 0)
			{
				enemyAI.state = EnemyAI.AIstate.Attack;

				StartCoroutine(AttackAnim());
				enemyAnimation.SetFloat("AnimMoveMagnitude", 0);
			}
			else
			{
				enemyAI.state = EnemyAI.AIstate.Idle;

				enemyAnimation.SetBool("AnimAttack", false);
				enemyAnimation.SetFloat("AnimMoveMagnitude", 0);
			}
		}

		private void Animate()
		{
			enemyAnimation.SetFloat("AnimMoveX", moveDirection.x);
			enemyAnimation.SetFloat("AnimMoveY", moveDirection.y);

			enemyAnimation.SetFloat("AnimLastMoveX", lastMoveDirection.x);
			enemyAnimation.SetFloat("AnimLastMoveY", lastMoveDirection.y);

			enemyAnimation.SetFloat("AnimMoveMagnitude", moveDirection.magnitude);
		}

		private void Die()
		{
			enemyAnimation.SetBool("AnimDeath", true);
			rigidbody2D.isKinematic = true;

			Destroy(GetComponent<BoxCollider2D>());
			Destroy(enemyAI);
			Destroy(gameObject, 5f);
		}

		private IEnumerator AttackAnim()
		{
			enemyAnimation.SetBool("AnimAttack", true);
			enemyAnimation.SetFloat("AnimAttackDirX", playerDirection.x);
			enemyAnimation.SetFloat("AnimAttackDirY", playerDirection.y);

			yield return new WaitForSeconds(enemyAI.TimeBetweenAttacks);

			enemyAnimation.SetBool("AnimAttack", false);
		}
	}
}
