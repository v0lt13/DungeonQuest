using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace DungeonQuest.Data
{
	public class GameDataHandler
	{
		private BinaryFormatter binaryFormatter = new BinaryFormatter();

		public void SaveData() 
		{
			// I dont want the data to be saved while in Unity
			#if !UNITY_EDITOR
			var data = AllData();
			var binaryFormatter = new BinaryFormatter();

			FileStream fileStream = File.Create(Application.dataPath + "/GameData.dat");

			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
			#endif
		}

		public void LoadPlayerData()
		{
			var gameManager = GameManager.INSTANCE;

			if (!File.Exists(Application.dataPath + "/GameData.dat")) return;

			FileStream fileStream = File.Open(Application.dataPath + "/GameData.dat", FileMode.Open);
			AllData data = binaryFormatter.Deserialize(fileStream) as AllData;
			
			fileStream.Close();

			gameManager.playerManager.playerHealth = data.playerData.playerHealh;
			gameManager.playerManager.defaultPlayerHealth = data.playerData.maxPlayerHealth;

			gameManager.playerManager.playerArmor = data.playerData.playerArmor;
			gameManager.playerManager.defaultPlayerArmor = data.playerData.maxPlayerArmor;

			gameManager.playerManager.playerAttack.GetDamage = data.playerData.playerAttackPower;

			gameManager.playerManager.playerLeveling.playerLevel = data.playerData.playerLevel;
			gameManager.playerManager.playerLeveling.PlayerXP = data.playerData.playerXP;
			gameManager.playerManager.playerLeveling.nextLevelXP = data.playerData.nextLevelXp;

			gameManager.playerManager.coinsAmount = data.playerData.coinsAmount;
			gameManager.playerManager.playerHealing.healingPotions = data.playerData.healingPotionsAmount;
		}
		
		public void LoadGameData()
		{
			var gameManager = GameManager.INSTANCE;

			if (!File.Exists(Application.dataPath + "/GameData.dat")) return;

			FileStream fileStream = File.Open(Application.dataPath + "/GameData.dat", FileMode.Open);
			AllData data = binaryFormatter.Deserialize(fileStream) as AllData;

			fileStream.Close();

			for (int i = 0; i < data.gameData.shopItemRequiredLevels.Count; i++)
			{
				gameManager.shopItems[i].minRequiredLevel = data.gameData.shopItemRequiredLevels[i];
			}
		}

		private AllData AllData()
		{
			var data = new AllData 
			{
				playerData = PlayerData(),
				gameData = GameData()
			};

			return data;
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

				playerAttackPower = gameManager.playerManager.playerAttack.GetDamage,

				playerLevel = gameManager.playerManager.playerLeveling.playerLevel,
				playerXP = gameManager.playerManager.playerLeveling.PlayerXP,
				nextLevelXp = gameManager.playerManager.playerLeveling.nextLevelXP,

				coinsAmount = gameManager.playerManager.coinsAmount,
				healingPotionsAmount = gameManager.playerManager.playerHealing.healingPotions,
			};

			return playerData;
		}

		private GameData GameData()
		{
			var gameManager = GameManager.INSTANCE;
			var gameData = new GameData();

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
