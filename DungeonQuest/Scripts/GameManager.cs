using UnityEngine;
using DungeonQuest.Data;
using DungeonQuest.Shop;
using DungeonQuest.Player;
using System.Collections.Generic;

namespace DungeonQuest
{
	public class GameManager : MonoBehaviour
	{
		public enum GameState
		{
			Running,
			Paused
		}

		public GameDataHandler gameData = new GameDataHandler();
		public List<ShopItem> shopItems;

		[HideInInspector] public bool hasDialogue;
		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public List<GameObject> enemyList;
		
		public GameState CurrentGameState { get; private set; }
		
		public int SecretCount { get; set; }
		public int KillCount { get; set; }
		public int LevelReached { get; set; }
		public int SecretLevelsUnlocked { get; set; }
		public int TotalKillCount { get; private set; }
		public int TotalSecretCount { get; private set; }
		public float CompletionTime { get; private set; }
		
		public static GameManager INSTANCE { get; private set; }

		void Awake()
		{
			#region SINGLETON_PATTERN
			if (INSTANCE != null)
			{
				Destroy(gameObject);
			}
			else
			{
				INSTANCE = this;
			}
			#endregion

			playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
			
			AudioListener.pause = false;

			AddEnemies();
			AddSecrets();
			SetGameState(GameState.Running);

			TotalKillCount = enemyList.Count;

			if (Application.loadedLevelName == "Lobby")
			{
				gameData.LoadGameData();
				GameObject.Find("DialogueTrigger").SetActive(!hasDialogue);
			}
		}

		void Update()
		{
			switch (CurrentGameState)
			{
				case GameState.Running:
					Screen.lockCursor = true;
					break;

				case GameState.Paused:
					Screen.lockCursor = false;
					break;
			}
		}

		void FixedUpdate()
		{
			CompletionTime += Time.fixedDeltaTime;
		}

		public static void LoadScene(string sceneName)
		{
			LoadingScreen.SCENE_NAME = sceneName;
			Application.LoadLevel("LoadingScreen");
		}

		public void LoadScene(int index) // For buttons, events and debug console
		{
			LoadingScreen.SCENE_INDEX = index;
			Application.LoadLevel("LoadingScreen");
		}

		public void SetGameState(GameState gameState)
		{
			CurrentGameState = gameState;
			Time.timeScale = gameState == GameState.Paused ? 0f : 1f;
		}

		public void EndLevel() // Called by Event
		{
			SetGameState(GameState.Paused);

			playerManager.renderer.enabled = false;
			playerManager.playerAttack.enabled = false;
			playerManager.collider2D.enabled = false;
			playerManager.enabled = false;

			gameData.SavePlayerData();
		}

		public void UnlockLevel(int level)
		{
			// We only want to set the reached level if we complete the last unlocked level
			if (LevelReached < level) LevelReached = level;
		}

		public void UnlockSecretlevel(int level)
		{
			if (SecretLevelsUnlocked < level) SecretLevelsUnlocked = level;
		}

		public void SaveData() // Called by Event and Debug console
		{
			gameData.SavePlayerData();
		}

		public void AddEnemies() // For spawning enemies from the debug console
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

			TotalKillCount++;
		}

		private void AddSecrets()
		{
			var secrets = GameObject.FindGameObjectsWithTag("Secret");

			TotalSecretCount = secrets.Length;
		}
	}
}
