using UnityEngine;
using System.Collections.Generic;

namespace DungeonQuest.Enemy.Boss.Special
{
	public class SkeletonKingSpecial : SpecialAbility
	{
		[SerializeField] private GameObject[] skeletonPrefabs;

		public override void Special()
		{
			// Spawn 4 random skeletons
			for (int i = 0; i < 4; i++)
			{
				var spawnPosition = new Vector2(transform.position.x + Random.Range(-20, 20), transform.position.y + Random.Range(-20, 20));
				var enemyObject = Instantiate(skeletonPrefabs[Random.Range(0, skeletonPrefabs.Length)], spawnPosition, Quaternion.identity);

				enemyObject.GetComponent<EnemyManager>().enemyLevel = 9;				
			}
		}
	}
}
