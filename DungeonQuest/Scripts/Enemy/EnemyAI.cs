using UnityEngine;
using DungeonQuest.Grid;
using System.Collections.Generic;

namespace DungeonQuest.Enemy
{
	public class EnemyAI : MonoBehaviour
	{
		public enum AIstate
		{
			Idle,
			Chase,
			Attack
		}

		[Header("AI Config:")]
		public int damage;
		[SerializeField] private float defaultTimeBetweenAttacks;
		[SerializeField] private float enemySpeed;
		[SerializeField] private GameObject projectilePrefab;
		[Space(10f)]
		[SerializeField] private bool showPath;

		[HideInInspector] public AIstate state;
		[HideInInspector] public List<PathNode> path;

		private float timeBetweenAttacks;

		private EnemyManager enemyManager;
		private GridGenerator grid;

		public float StunTime { get; private set; }

		void Awake()
		{
			grid = GameObject.Find("GameManager").GetComponent<GridGenerator>();
			enemyManager = GetComponent<EnemyManager>();
		}

		void Update()
		{
			// Wait for the stun to wear off, then stop the knockback
			if (StunTime <= 0f)
			{
				StunTime = 0f;
				enemyManager.rigidbody2D.velocity = Vector2.zero;
			}
			else if (StunTime > 0f)
			{
				StunTime -= Time.deltaTime;
			}

			if (enemyManager.playerManager.isDead) return;

			switch (state)
			{
				default:
				case AIstate.Idle:
					Idle();
					break;

				case AIstate.Chase:
					Chase();
					break;

				case AIstate.Attack:
					Attack();
					break;
			}
		}

		public void StunEnemy(float duration)
		{
			StunTime = duration;
		}

		private void Idle() 
		{
			timeBetweenAttacks = defaultTimeBetweenAttacks; 
		}

		private void Chase()
		{
			if (GameManager.INSTANCE.CurrentGameState == GameManager.GameState.Paused) return;

			if (StunTime == 0f)
			{
				timeBetweenAttacks = defaultTimeBetweenAttacks;
				FindPathToPlayer(enemyManager.playerManager.transform.position, out path);

				if (path != null)
				{
					try
					{
						transform.position = Vector2.MoveTowards(transform.position, new Vector2(path[1].x, path[1].y) * 10f + Vector2.one * 5f, enemySpeed * Time.deltaTime);
					}
					catch (System.ArgumentOutOfRangeException)
					{
						transform.position = Vector2.MoveTowards(transform.position, enemyManager.playerManager.transform.position, enemySpeed * Time.deltaTime);				
					}
				}
				else
				{
					state = AIstate.Idle;
				}
			}
		}

		private void Attack()
		{
			if (StunTime == 0f)
			{
				if (timeBetweenAttacks <= 0f)
				{
					timeBetweenAttacks = defaultTimeBetweenAttacks;
				}
				else
				{
					timeBetweenAttacks -= Time.deltaTime;
				}
			}
		}

		private void FindPathToPlayer(Vector2 targetPosition, out List<PathNode> path)
		{
			int startX, startY, endX, endY;

			grid.pathfinding.GetGrid.GetXY(transform.position, out startX, out startY);
			grid.pathfinding.GetGrid.GetXY(targetPosition, out endX, out endY);

			path = grid.pathfinding.FindPath(startX, startY, endX, endY);

			if (path != null && showPath)
			{
				for (int i = 0; i < path.Count - 1; i++)
				{
					Debug.DrawLine(new Vector2(path[i].x, path[i].y) * 10f + Vector2.one * 5f, new Vector2(path[i + 1].x, path[i + 1].y) * 10f + Vector2.one * 5f, Color.green);
				}
			}
		}

		private void HitPlayer() // Called by Animation event
		{
			enemyManager.playerManager.DamagePlayer(damage);
		}

		private void ShootArrow() // Called by Animation event
		{
			var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;

			projectile.GetComponent<EnemyProjectile>().ProjectileDamage = damage;

			audio.clip = enemyManager.shootSFX;
			audio.pitch = Random.Range(1f, 1.5f);
			audio.Play();
		}
	}
}
