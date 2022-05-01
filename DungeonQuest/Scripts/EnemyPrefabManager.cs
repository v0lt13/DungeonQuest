using UnityEngine;
using System.Collections.Generic;

namespace DungeonQuest
{
	public class EnemyPrefabManager
	{
		public Object MeleeSkeleton { get; private set; }
		public Object RangedSkeleton { get; private set; }
		public Object ArmoredMeleeSkeleton { get; private set; }
		public Object ArmoredRangedSkeleton { get; private set; }

		public List<string> enemyList = new List<string>
		{
			"MeleeSkeleton",
			"RangedSkeleton",
			"ArmMeleeSkeleton",
			"ArmRangedSkeleton"
		};

		public void LoadPrefabs()
		{
			MeleeSkeleton = Resources.Load("Prefabs/Entities/MeleeSkeleton", typeof(GameObject));
			RangedSkeleton = Resources.Load("Prefabs/Entities/RangedSkeleton", typeof(GameObject));
			ArmoredMeleeSkeleton = Resources.Load("Prefabs/Entities/ArmoredMeleeSkeleton", typeof(GameObject));
			ArmoredRangedSkeleton = Resources.Load("Prefabs/Entities/ArmoredRangedSkeleton", typeof(GameObject));
		}

		public void InstatiateEnemy(GameObject enemy, uint level)
		{
			var playerPosition = GameManager.INSTANCE.playerManager.transform.position;
			var spawnPositionOffset = new Vector2(playerPosition.x + Random.Range(-5, 5), playerPosition.y + Random.Range(-5, 5));

			var enemyObject = MonoBehaviour.Instantiate(enemy, spawnPositionOffset, Quaternion.identity) as GameObject;

			enemyObject.GetComponent<Enemy.EnemyManager>().enemyLevel = (int)level;
		}
	}
}
