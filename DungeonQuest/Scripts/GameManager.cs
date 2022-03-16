using UnityEngine;
using DungeonQuest.Player;
using System.Collections.Generic;

namespace DungeonQuest
{
	public class GameManager : MonoBehaviour
	{
		public List<GameObject> enemyList;
		[HideInInspector] public PlayerManager playerManager;

		public static GameManager INSTANCE { get; private set; }

		void Awake()
		{
			#region SINGLETON_PATTERN
			if (INSTANCE != null)
				Destroy(gameObject);
			else
				INSTANCE = this;
			#endregion

			var playerObject = GameObject.FindGameObjectWithTag("Player");
			if (playerObject != null) playerManager = playerObject.GetComponent<PlayerManager>();

			AddEnemies();
			EnableCursor(false);
		}

		public void EnableCursor(bool toogle)
		{
			Screen.lockCursor = !toogle;
			Screen.showCursor = toogle;
		}

		public void AddEnemies()
		{
			var enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
			var enemyHolder = GameObject.Find("EnemyHolder");

			if (enemyObjects == null && enemyHolder == null) return;

			enemyList.Clear();

			foreach (var enemyObject in enemyObjects)
			{
				if (enemyObject.GetComponent<Enemy.EnemyManager>() == null) continue;

				enemyObject.transform.SetParent(enemyHolder.transform);
				enemyList.Add(enemyObject);
			}
		}
	}
}
