using UnityEngine;
using System.Collections.Generic;

namespace DungeonQuest.Enemy
{
	public class EnemyPrefabManager
	{
		public Object MeleeSkeleton { get; private set; }
		public Object RangedSkeleton { get; private set; }
		public Object ArmoredMeleeSkeleton { get; private set; }
		public Object ArmoredRangedSkeleton { get; private set; }
		public Object Spider { get; private set; }
		public Object SnowGolem { get; private set; }
		public Object IceGolem { get; private set; }

		public List<string> enemyList = new List<string>
		{
			"MeleeSkeleton",
			"RangedSkeleton",
			"ArmMeleeSkeleton",
			"ArmRangedSkeleton",
			"Spider",
			"SnowGolem",
			"IceGolem"
		};

		public void LoadPrefabs()
		{
			MeleeSkeleton = Resources.Load("Prefabs/Entities/MeleeSkeleton", typeof(GameObject));
			RangedSkeleton = Resources.Load("Prefabs/Entities/RangedSkeleton", typeof(GameObject));
			ArmoredMeleeSkeleton = Resources.Load("Prefabs/Entities/ArmoredMeleeSkeleton", typeof(GameObject));
			ArmoredRangedSkeleton = Resources.Load("Prefabs/Entities/ArmoredRangedSkeleton", typeof(GameObject));
			Spider = Resources.Load("Prefabs/Entities/Spider", typeof(GameObject));
			SnowGolem = Resources.Load("Prefabs/Entities/SnowGolem", typeof(GameObject));
			IceGolem = Resources.Load("Prefabs/Entities/IceGolem", typeof(GameObject));
		}

		public void InstatiateEnemy(GameObject enemy, uint level)
		{
			var playerPosition = GameManager.INSTANCE.playerManager.transform.position;
			var spawnPosition = new Vector2(playerPosition.x + Random.Range(-5, 5), playerPosition.y + Random.Range(-5, 5));

			var enemyObject = Object.Instantiate(enemy, spawnPosition, Quaternion.identity) as GameObject;

			enemyObject.GetComponent<EnemyManager>().enemyLevel = (int)level;
		}
	}
}
