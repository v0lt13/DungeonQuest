using UnityEngine;
using DungeonQuest.Grid;
using DungeonQuest.Menus;
using DungeonQuest.DebugConsole;
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

		public enum AIType
		{
			Melee,
			Ranged
		}

		[Header("AI Config:")]
		public AIType type;
		public int damage;
		[SerializeField] private float defaultTimeBetweenAttacks;
		[SerializeField] private float enemySpeed;
		[SerializeField] private GameObject projectilePrefab;
		[Space(10f)]
		public bool showPath;

		[HideInInspector] public AIstate state;
		[HideInInspector] public List<PathNode> path;

		private EnemyManager enemyManager;
		private GridGenerator grid;

		public float TimeBetweenAttacks { get; private set; }
		public float StunTime { get; set; }
		public float GetDefaultTimeBetweenAttacks { get { return defaultTimeBetweenAttacks; } }

		void Awake()
		{
			grid = GameObject.Find("Grid").GetComponent<GridGenerator>();
			enemyManager = GetComponent<EnemyManager>();
		}

		void Update()
		{
			if (StunTime <= 0f)
			{
				StunTime = 0f;
				enemyManager.rigidbody2D.velocity = Vector2.zero;
			}
			else if (StunTime > 0f)
			{
				StunTime -= Time.deltaTime;
			}

			switch (state)
			{
				default:
				case AIstate.Idle:
					Idle(enemyManager.player.transform.position);
					break;

				case AIstate.Chase:
					Chase(enemyManager.player.transform.position);
					break;

				case AIstate.Attack:
					Attack();
					break;
			}
		}

		private void Idle(Vector2 targetPosition) 
		{
			TimeBetweenAttacks = 0.3f;
			FindPathToPlayer(targetPosition, out path);
		}

		private void Chase(Vector2 targetPosition)
		{			
			FindPathToPlayer(targetPosition, out path);

			if (PauseMenu.IS_GAME_PAUSED || DebugController.IS_CONSOLE_ON) return;

			if (path != null && StunTime == 0f)
			{
				try
				{
					transform.position = Vector2.MoveTowards(transform.position, new Vector2(path[1].x, path[1].y) * 10f + Vector2.one * 5f, enemySpeed * Time.deltaTime);
				}
				catch (System.ArgumentOutOfRangeException)
				{
					transform.position = Vector2.MoveTowards(transform.position, enemyManager.player.transform.position, enemySpeed * Time.deltaTime);				
				}
			}
		}

		private void Attack()
		{
			if (TimeBetweenAttacks <= 0f)
			{
				TimeBetweenAttacks = defaultTimeBetweenAttacks;
				enemyManager.IsAttacking = true;

				switch (type)
				{
					case AIType.Melee:
						enemyManager.playerManager.DamagePlayer(damage);
						break;
					case AIType.Ranged:
						if (StunTime != 0f) return;
						Invoke("ShootArrow", 0.3f);
						break;
				}
			}
			else
			{
				TimeBetweenAttacks -= Time.deltaTime;
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

		private void ShootArrow()
		{
			Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		}
	}
}
