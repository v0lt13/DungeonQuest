using System.Collections.Generic;

namespace DungeonQuest.Data
{
	[System.Serializable]
	public class PlayerData
	{
		public int playerHealh;
		public int maxPlayerHealth;
		
		public int playerArmor;
		public int maxPlayerArmor;
		
		public int playerAttackPower;
		
		public int playerLevel;
		public int playerXP;
		public int nextLevelXp;

		public int coinsAmount;
		public int healingPotionsAmount;

		public int levelReached;
		public int bossesCompleted;

		public float playerSpeed;
		public float lifestealAmount;

		public bool rogueMode;

		public Dictionary<int, bool> secretLevelUnlocked;
	}
}
