using System.IO;
using UnityEngine;
using DungeonQuest.Player;
using DungeonQuest.UI.Menus;
using DungeonQuest.Achievements;
using System.Runtime.Serialization.Formatters.Binary;

namespace DungeonQuest.Data
{
	public class GameDataHandler
	{
		public enum DataType
		{
			Player,
			Game,
			Menu
		}

		private BinaryFormatter binaryFormatter = new BinaryFormatter();

		public void SaveData(DataType dataType)
		{
			FileStream fileStream;
			var binaryFormatter = new BinaryFormatter();

			if (!Directory.Exists(Application.persistentDataPath + "/Data"))
			{
				Directory.CreateDirectory(Application.persistentDataPath + "/Data");
			}

			switch (dataType)
			{
				case DataType.Player:
					var playerData = PlayerData();

					fileStream = File.Create(Application.persistentDataPath + "/Data/PlayerData.dat");

					binaryFormatter.Serialize(fileStream, playerData);
					fileStream.Close();
					break;

				case DataType.Game:
					var gameData = GameData();

					fileStream = File.Create(Application.persistentDataPath + "/Data/GameData.dat");

					binaryFormatter.Serialize(fileStream, gameData);
					fileStream.Close();
					break;

				case DataType.Menu:
					var menuData = MenuData();

					fileStream = File.Create(Application.persistentDataPath + "/Data/MenuData.dat");

					binaryFormatter.Serialize(fileStream, menuData);
					fileStream.Close();
					break;
			}
		}

		public void LoadPlayerData()
		{
			var gameManager = GameManager.INSTANCE;

			if (!File.Exists(Application.persistentDataPath + "/Data/PlayerData.dat")) return;

			FileStream fileStream = File.Open(Application.persistentDataPath + "/Data/PlayerData.dat", FileMode.Open);
			PlayerData data = binaryFormatter.Deserialize(fileStream) as PlayerData;
			
			fileStream.Close();

			gameManager.playerManager.playerHealth = data.playerHealh;
			gameManager.playerManager.defaultPlayerHealth = data.maxPlayerHealth;

			gameManager.playerManager.playerArmor = data.playerArmor;
			gameManager.playerManager.defaultPlayerArmor = data.maxPlayerArmor;

			gameManager.playerManager.playerAttack.damage = data.playerAttackPower;
			gameManager.playerManager.playerMovement.defaultPlayerSpeed = data.playerSpeed;

			gameManager.playerManager.playerLeveling.playerLevel = data.playerLevel;
			gameManager.playerManager.playerLeveling.PlayerXP = data.playerXP;
			gameManager.playerManager.playerLeveling.nextLevelXP = data.nextLevelXp;

			gameManager.playerManager.coinsAmount = data.coinsAmount;
			gameManager.playerManager.playerHealing.healingPotions = data.healingPotionsAmount;

			PlayerManager.ROGUE_MODE = data.rogueMode;

			gameManager.bossesCompleted = data.bossesCompleted;
			gameManager.secretLevelsUnlocked = data.secretLevelUnlocked;

			gameManager.playerManager.IncreaseLifesteal(data.lifestealAmount);
			gameManager.UnlockLevel(data.levelReached);
		}
		
		public void LoadGameData()
		{
			var gameManager = GameManager.INSTANCE;

			if (!File.Exists(Application.persistentDataPath + "/Data/GameData.dat")) return;

			FileStream fileStream = File.Open(Application.persistentDataPath + "/Data/GameData.dat", FileMode.Open);
			GameData data = binaryFormatter.Deserialize(fileStream) as GameData;

			fileStream.Close();

			gameManager.hasDialogue = data.hasDialogue;

			for (int i = 0; i < data.shopItemRequiredLevels.Count; i++)
			{
				gameManager.shopItems[i].minRequiredLevel = data.shopItemRequiredLevels[i];
			}
		}

		public void LoadMenuData()
		{
			if (!File.Exists(Application.persistentDataPath + "/Data/MenuData.dat")) return;

			FileStream fileStream = File.Open(Application.persistentDataPath + "/Data/MenuData.dat", FileMode.Open);
			MenuData data = binaryFormatter.Deserialize(fileStream) as MenuData;

			fileStream.Close();

			MenuManager.GAME_COMPLETED = data.gameCompleted;
			MenuManager.achivementCheckboxValues = data.achievementsCompleted;
		}

		private PlayerData PlayerData()
		{
			var gameManager = GameManager.INSTANCE;

			var playerData = new PlayerData
			{
				playerHealh = gameManager.playerManager.playerHealth,
				maxPlayerHealth = gameManager.playerManager.defaultPlayerHealth,

				playerArmor = gameManager.playerManager.playerArmor,
				maxPlayerArmor = gameManager.playerManager.defaultPlayerArmor,

				playerAttackPower = gameManager.playerManager.playerAttack.damage,
				playerSpeed = gameManager.playerManager.playerMovement.defaultPlayerSpeed,

				playerLevel = gameManager.playerManager.playerLeveling.playerLevel,
				playerXP = gameManager.playerManager.playerLeveling.PlayerXP,
				nextLevelXp = gameManager.playerManager.playerLeveling.nextLevelXP,

				coinsAmount = gameManager.playerManager.coinsAmount,
				healingPotionsAmount = gameManager.playerManager.playerHealing.healingPotions,
				lifestealAmount = gameManager.playerManager.LifestealAmount,

				rogueMode = PlayerManager.ROGUE_MODE,

				levelReached = gameManager.LevelReached,
				secretLevelUnlocked = gameManager.secretLevelsUnlocked,

				bossesCompleted = gameManager.bossesCompleted
			};

			return playerData;
		}

		private GameData GameData()
		{
			var gameManager = GameManager.INSTANCE;
			var gameData = new GameData();

			gameData.hasDialogue = gameManager.hasDialogue;

			for (int i = 0; i < gameManager.shopItems.Count; i++)
			{
				if (gameManager.shopItems[i] != null)
				{
					gameData.shopItemRequiredLevels.Add(gameManager.shopItems[i].minRequiredLevel);
				}
			}

			return gameData;
		}

		private MenuData MenuData()
		{
			var menuData = new MenuData();

			menuData.gameCompleted = MenuManager.GAME_COMPLETED;

			for (int i = 0; i < AchievementManager.ACHIEVEMENT_LIST.Count; i++)
			{
				menuData.achievementsCompleted.Add(AchievementManager.ACHIEVEMENT_LIST[i].achieved);		
			}

			return menuData;
		}
	}
}
