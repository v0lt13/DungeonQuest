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
		[Space(10f)]
		[SerializeField] private GameObject speedUpgradeItem;
		[SerializeField] private GameObject lifestealItem;

		[HideInInspector] public int secretCount;
		[HideInInspector] public int killCount;
		[HideInInspector] public int totalKillCount;
		[HideInInspector] public int totalSecretCount;
		[HideInInspector] public int bossesCompleted;
		[HideInInspector] public bool hasDialogue;
		
		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public List<GameObject> enemyList;

		public Dictionary<int, bool> secretLevelsUnlocked = new Dictionary<int, bool>
		{
			{1, false}, // S1
			{2, false}, // S2
			{3, false}, // S3
			{4, false}  // S4
		};

		public int LevelReached { get; private set; }
		public float CompletionTime { get; private set; }

		public GameState CurrentGameState { get; private set; }
		
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

			SetGameState(GameState.Running);

			if (Application.loadedLevelName == "Lobby")
			{
				gameData.LoadPlayerData();
				gameData.LoadGameData();

				GameObject.Find("DialogueTrigger").SetActive(!hasDialogue);

				if (LevelReached >= 6)
				{
					speedUpgradeItem.SetActive(true);
				}

				if (LevelReached >= 11)
				{
					lifestealItem.SetActive(true);
				}
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

			Screen.lockCursor = gameState != GameState.Paused;
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
			// We only want to unlock a new level if we complete the last unlocked level
			if (LevelReached < level) LevelReached = level;
		}

		public void UnlockSecretlevel(int level)
		{
			secretLevelsUnlocked[level] = true;
		}

		public void SaveData() // Called by Event
		{
			gameData.SavePlayerData();
		}
	}
}
