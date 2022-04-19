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

		private float stunTime;

		private EnemyManager enemyManager;
		private GridGenerator grid;

		public float TimeBetweenAttacks { get; private set; }

		void Awake()
		{
			grid = GameObject.Find("GameManager").GetComponent<GridGenerator>();
			enemyManager = GetComponent<EnemyManager>();
		}

		void Update()
		{
			// Wait for the stun to wear off, then stop the knockback
			if (stunTime <= 0f)
			{
				stunTime = 0f;
				enemyManager.rigidbody2D.velocity = Vector2.zero;
			}
			else if (stunTime > 0f)
			{
				stunTime -= Time.deltaTime;
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
			stunTime = duration;
		}

		private void Idle() 
		{
			TimeBetweenAttacks = 0.1f; 
			enemyManager.IsAttacking = false;
		}

		private void Chase()
		{
			enemyManager.IsAttacking = false;

			if (GameManager.INSTANCE.CurrentGameState == GameManager.GameState.Paused) return;

			if (stunTime == 0f)
			{
				TimeBetweenAttacks = 0.1f;
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
			if (state == AIstate.Idle) return;

			if (stunTime == 0f)
			{
				if (TimeBetweenAttacks <= 0f)
				{
					enemyManager.IsAttacking = true;
					TimeBetweenAttacks = defaultTimeBetweenAttacks;
				}
				else
				{
					enemyManager.IsAttacking = false;
					TimeBetweenAttacks -= Time.deltaTime;
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

		private void HitPlayer() // Animation event
		{
			enemyManager.playerManager.DamagePlayer(damage);
		}

		private void ShootArrow() // Animation event
		{
			var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;

			projectile.GetComponent<EnemyProjectile>().ProjectileDamage = damage;
		}
	}
}
