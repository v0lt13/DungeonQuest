using UnityEngine;
using DungeonQuest.Data;
using DungeonQuest.Shop;
using System.Collections;
using DungeonQuest.Player;
using DungeonQuest.UI.Menus;
using DungeonQuest.Achievements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
		[HideInInspector] public int bossesCompleted;
		[HideInInspector] public bool hasDialogue;
		
		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public AchievementManager achievementManager;

		[HideInInspector] public List<GameObject> enemyList;

		private bool isCheckingUpgrades;

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
			achievementManager = GetComponent<AchievementManager>();
			
			AudioListener.pause = false;
			SetGameState(GameState.Running);

			if (SceneManager.GetActiveScene().name == "Lobby")
			{
				gameData.LoadPlayerData();
				gameData.LoadGameData();

				GameObject.Find("DialogueTrigger").SetActive(!hasDialogue);

				if (LevelReached >= 6)
				{
					shopItems[3].gameObject.SetActive(true);
				}

				if (LevelReached >= 11)
				{
					shopItems[4].gameObject.SetActive(true);
				}
			}
		}

		void Start()
		{
			// Unlock secret level achievements
			switch (SceneManager.GetActiveScene().buildIndex)
			{
				case 9:
					achievementManager.UnlockAchivement(9);
					break;

				case 15:
					achievementManager.UnlockAchivement(10);
					break;

				case 21:
					achievementManager.UnlockAchivement(11);
					break;

				case 27:
					achievementManager.UnlockAchivement(12);
					break;
			}
		}

		void Update()
		{
			if (SceneManager.GetActiveScene().name == "Lobby")
			{
				if (!isCheckingUpgrades) StartCoroutine(CheckUpgrades());
			}
		}

		void FixedUpdate()
		{
			CompletionTime += Time.fixedDeltaTime;
		}

		public static void LoadScene(string sceneName)
		{
			LoadingScreen.SCENE_NAME = sceneName;
			SceneManager.LoadScene("LoadingScreen");
		}

		public void LoadScene(int index) // For buttons, events and debug console
		{
			LoadingScreen.SCENE_INDEX = index;
			SceneManager.LoadScene("LoadingScreen");
		}

		public void SetGameState(GameState gameState)
		{
			CurrentGameState = gameState;

			Cursor.visible = gameState == GameState.Paused;
			Cursor.lockState = gameState == GameState.Paused ? CursorLockMode.None : CursorLockMode.Locked;

			Time.timeScale = gameState == GameState.Paused ? 0f : 1f;
		}

		public void EndLevel() // Called by Event
		{
			SetGameState(GameState.Paused);

			playerManager.spriteRenderer.enabled = false;
			playerManager.playerAttack.enabled = false;
			playerManager.playerCollider.enabled = false;
			playerManager.enabled = false;

			gameData.SaveData(GameDataHandler.DataType.Player);
		}

		public void CompleteGame() // Called by Event
		{
			if (PlayerManager.ROGUE_MODE)
			{
				achievementManager.UnlockAchivement(23);
			}
			else
			{
				MenuManager.GAME_COMPLETED = true;
			}

			gameData.SaveData(GameDataHandler.DataType.Menu);
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
			gameData.SaveData(GameDataHandler.DataType.Player);
		}

		private IEnumerator CheckUpgrades()
		{
			isCheckingUpgrades = true;

			yield return new WaitForSeconds(1f);

			if (shopItems.TrueForAll((ShopItem item) => item.isUpgradeMaxed))
			{
				achievementManager.UnlockAchivement(19);
			}

			isCheckingUpgrades = false;
		}
	}
}
