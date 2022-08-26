using System.Collections.Generic;

namespace DungeonQuest.Data
{
	[System.Serializable]
	public class MenuData
	{
		public bool gameCompleted;
		public List<bool> achievementsCompleted = new List<bool>();
	}
}
