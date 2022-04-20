using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace DungeonQuest.Data
{
	public class GameDataHandler
	{
		private BinaryFormatter binaryFormatter = new BinaryFormatter();

		public void SavePlayerData() 
		{
			var data = PlayerData();
			var binaryFormatter = new BinaryFormatter();

			if (!Directory.Exists(Application.dataPath + "/Data"))
			{
				Directory.CreateDirectory(Application.dataPath + "/Data");
			}

			FileStream fileStream = File.Create(Application.dataPath + "/Data/PlayerData.dat");

			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void SaveGameData()
		{
			var data = GameData();
			var binaryFormatter = new BinaryFormatter();

			if (!Directory.Exists(Application.dataPath + "/Data"))
			{
				Directory.CreateDirectory(Application.dataPath + "/Data");
			}

			// We save the game data separate from the player data because we want to save the game data only when the player is in the lobby or the testing scene
			// Saveing the game data on other scenes will overwrite it with the wrong values
			FileStream fileStream = File.Create(Application.dataPath + "/Data/GameData.dat");

			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void LoadPlayerData()
		{
			var gameManager = GameManager.INSTANCE;

			if (!File.Exists(Application.dataPath + "/Data/PlayerData.dat")) return;

			FileStream fileStream = File.Open(Application.dataPath + "/Data/PlayerData.dat", FileMode.Open);
			PlayerData data = binaryFormatter.Deserialize(fileStream) as PlayerData;
			
			fileStream.Close();

			gameManager.playerManager.playerHealth = data.playerHealh;
			gameManager.playerManager.defaultPlayerHealth = data.maxPlayerHealth;

			gameManager.playerManager.playerArmor = data.playerArmor;
			gameManager.playerManager.defaultPlayerArmor = data.maxPlayerArmor;

			gameManager.playerManager.playerAttack.damage = data.playerAttackPower;

			gameManager.playerManager.playerLeveling.playerLevel = data.playerLevel;
			gameManager.playerManager.playerLeveling.PlayerXP = data.playerXP;
			gameManager.playerManager.playerLeveling.nextLevelXP = data.nextLevelXp;

			gameManager.playerManager.coinsAmount = data.coinsAmount;
			gameManager.playerManager.playerHealing.healingPotions = data.healingPotionsAmount;

			gameManager.LevelReached = data.levelReached;
		}
		
		public void LoadGameData()
		{
			var gameManager = GameManager.INSTANCE;

			if (!File.Exists(Application.dataPath + "/Data/GameData.dat")) return;

			FileStream fileStream = File.Open(Application.dataPath + "/Data/GameData.dat", FileMode.Open);
			GameData data = binaryFormatter.Deserialize(fileStream) as GameData;

			fileStream.Close();

			for (int i = 0; i < data.shopItemRequiredLevels.Count; i++)
			{
				gameManager.shopItems[i].minRequiredLevel = data.shopItemRequiredLevels[i];
			}
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

				playerLevel = gameManager.playerManager.playerLeveling.playerLevel,
				playerXP = gameManager.playerManager.playerLeveling.PlayerXP,
				nextLevelXp = gameManager.playerManager.playerLeveling.nextLevelXP,

				coinsAmount = gameManager.playerManager.coinsAmount,
				healingPotionsAmount = gameManager.playerManager.playerHealing.healingPotions,

				levelReached = gameManager.LevelReached
			};

			return playerData;
		}

		private GameData GameData()
		{
			var gameManager = GameManager.INSTANCE;
			var gameData = new GameData();

			// Currently the game data only holds the shop upgrades required levels
			foreach (var item in gameManager.shopItems)
			{
				if (item != null)
				{
					gameData.shopItemRequiredLevels.Add(item.minRequiredLevel);
				}
			}

			return gameData;
		}
	}
}
