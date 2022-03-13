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

		[Header ("AI Config:")]
		[SerializeField] private int damage;
		[SerializeField] private float timeBetweenAttacks;
		[SerializeField] private float enemySpeed;
		public bool showPath;

		[HideInInspector] public AIstate state;
		[HideInInspector] public List<PathNode> path;

		private EnemyManager enemyManager;
		private GridGenerator grid;

		public float TimeBetweenAttacks { get; set; }
		public float StunTime { get; set; }

		void Awake()
		{
			grid = GameObject.Find("Grid").GetComponent<GridGenerator>();
			enemyManager = GetComponent<EnemyManager>();

			enemySpeed /= 10;
		}

		void Update()
		{
			if (StunTime < 0)
			{
				StunTime = 0;
			}
			else if (StunTime > 0)
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

		private void Idle(Vector3 targetPosition) 
		{
			TimeBetweenAttacks = 0;
			FindPathToPlayer(targetPosition, out path);
		}

		private void Chase(Vector3 targetPosition)
		{
			TimeBetweenAttacks = 0;
			FindPathToPlayer(targetPosition, out path);

			if (PauseMenu.IS_GAME_PAUSED || DebugController.IS_CONSOLE_ON) return;

			if (path != null && StunTime == 0)
			{
				try
				{
					transform.position = Vector2.MoveTowards(transform.position, new Vector3(path[1].x, path[1].y) * 10f + Vector3.one * 5f, enemySpeed);
				}
				catch (System.ArgumentOutOfRangeException)
				{
					state = AIstate.Idle;
				}
			}
		}

		private void Attack()
		{
			if (TimeBetweenAttacks <= 0)
			{
				enemyManager.playerManager.DamagePlayer(damage);
				TimeBetweenAttacks = timeBetweenAttacks;
			}
			else
			{
				TimeBetweenAttacks -= Time.deltaTime;
			}
		}

		private void FindPathToPlayer(Vector3 targetPosition, out List<PathNode> path)
		{
			int startX, startY, endX, endY;

			grid.pathfinding.GetGrid.GetXY(transform.position, out startX, out startY);
			grid.pathfinding.GetGrid.GetXY(targetPosition, out endX, out endY);

			path = grid.pathfinding.FindPath(startX, startY, endX, endY);

			if (path != null && showPath)
			{
				for (int i = 0; i < path.Count - 1; i++)
				{
					Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green);
				}
			}
		}
	}
}
