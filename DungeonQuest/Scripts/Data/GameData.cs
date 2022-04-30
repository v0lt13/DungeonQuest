using System.Collections.Generic;

namespace DungeonQuest.Data
{
	[System.Serializable]
	public class GameData
	{
		public bool hasDialogue;

		public List<int> shopItemRequiredLevels = new List<int>();
	}
}
