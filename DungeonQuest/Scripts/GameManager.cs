using UnityEngine;
using DungeonQuest.Player;
using System.Collections.Generic;

namespace DungeonQuest
{
	public class GameManager : MonoBehaviour
	{
		[HideInInspector] public PlayerManager playerManager;

		public List<GameObject> enemyList;

		public static GameManager INSTANCE { get; private set; }

		public int SecretCount { get; set; }
		public int KillCount { get; set; }
		public int TotalKillCount { get; private set; }
		public int TotalSecretCount { get; private set; }
		public float CompletionTime { get; private set; }
		public bool LevelEnded { get; private set; }

		void Awake()
		{
			#region SINGLETON_PATTERN
			if (INSTANCE != null)
				Destroy(gameObject);
			else
				INSTANCE = this;
			#endregion

			playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

			AddEnemies();
			AddSecrets();
			EnableCursor(false);
		}

		void FixedUpdate()
		{
			CompletionTime += Time.fixedDeltaTime;
		}

		public static void EnableCursor(bool toogle)
		{
			Screen.lockCursor = !toogle;
			Screen.showCursor = toogle;
		}

		public static void LoadScene(string sceneName)
		{
			LoadingScreen.SCENE_NAME = sceneName;
			Application.LoadLevel("LoadingScreen");
		}

		public void EndLevel()
		{
			LevelEnded = true;
			Time.timeScale = 0f;

			EnableCursor(true);
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

			TotalKillCount = enemyList.Count;
		}

		private void AddSecrets()
		{
			var secrets = GameObject.FindGameObjectsWithTag("Secret");

			TotalSecretCount = secrets.Length;
		}
	}
}
