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

		[HideInInspector] public int secretCount;
		[HideInInspector] public int killCount;
		[HideInInspector] public int totalKillCount;
		[HideInInspector] public int totalSecretCount;
		[HideInInspector] public bool hasDialogue;
		
		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public List<GameObject> enemyList;		

		public int LevelReached { get; private set; }
		public int SecretLevelsUnlocked { get; private set; }
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
				gameData.LoadGameData();
				GameObject.Find("DialogueTrigger").SetActive(!hasDialogue);
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

			Screen.lockCursor = gameState == GameState.Paused ? false : true;
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

		public void SaveData() // Called by Event
		{
			gameData.SavePlayerData();
		}
	}
}
