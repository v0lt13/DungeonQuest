using UnityEngine;
using System.Collections.Generic;

namespace DungeonQuest
{
	public class EnemyPrefabLoader
	{
		public Object MeleeSkeleton { get; private set; }
		public Object RangedSkeleton { get; private set; }

		public List<string> enemyList = new List<string>
		{
			"MeleeSkeleton",
			"RangedSkeleton"
		};

		public void LoadPrefabs()
		{
			MeleeSkeleton = Resources.Load("Prefabs/Entities/MeleeEnemy", typeof(GameObject));
			RangedSkeleton = Resources.Load("Prefabs/Entities/RangedEnemy", typeof(GameObject));
		}
	}
}
