﻿using System.Collections.Generic;

namespace DungeonQuest.Data
{
	[System.Serializable]
	public class GameData
	{
		public List<int> shopItemRequiredLevels = new List<int>();

		public List<bool> completedLevels = new List<bool>();
	}
}