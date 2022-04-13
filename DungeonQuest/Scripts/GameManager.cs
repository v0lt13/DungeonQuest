﻿using UnityEngine;
using UnityEngine.UI;
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
		public List<Text> killCountTexts;
		public List<Text> secretCountTexts;

		[HideInInspector] public PlayerManager playerManager;
		[HideInInspector] public List<GameObject> enemyList;

		public GameState CurrentGameState { get; private set; }
		
		public int SecretCount { get; set; }
		public int KillCount { get; set; }
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
			EnableCursor(false);
			SetGameState(GameState.Running);

			if (Application.loadedLevelName == "Lobby")
			{
				gameData.LoadGameData();
			}

			TotalKillCount = enemyList.Count;
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

			gameData.SaveData();
			EnableCursor(true);
		}

		public void SaveData() // Called by Event
		{
			gameData.SaveData();
		}

		public void AddEnemies() // For debug console
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
