﻿using UnityEngine;
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
		public Object FrostGolem { get; private set; }
		public Object IceWarrior { get; private set; }
		public Object Goblin { get; private set; }
		public Object SpearGoblin { get; private set; }
		public Object EliteGoblin { get; private set; }
		public Object GoblinAssasin { get; private set; }
		public Object Slime { get; private set; }
		public Object LavaGolem { get; private set; }
		public Object PyroArcher { get; private set; }
		public Object PyroKnight { get; private set; }
		public Object BlastGolem { get; private set; }

		public List<string> enemyList = new List<string>
		{
			"MeleeSkeleton",
			"RangedSkeleton",
			"ArmMeleeSkeleton",
			"ArmRangedSkeleton",
			"Spider",
			"SnowGolem",
			"IceGolem",
			"FrostGolem",
			"IceWarrior",
			"Goblin",
			"SpearGoblin",
			"EliteGoblin",
			"GoblinAssasin",
			"Slime",
			"LavaGolem",
			"PyroArcher",
			"PyroKnight",
			"BlastGolem"
		};

		public void LoadPrefabs()
		{
			// Chapter 1 enemies
			MeleeSkeleton = Resources.Load("Prefabs/Entities/MeleeSkeleton", typeof(GameObject));
			RangedSkeleton = Resources.Load("Prefabs/Entities/RangedSkeleton", typeof(GameObject));
			ArmoredMeleeSkeleton = Resources.Load("Prefabs/Entities/ArmoredMeleeSkeleton", typeof(GameObject));
			ArmoredRangedSkeleton = Resources.Load("Prefabs/Entities/ArmoredRangedSkeleton", typeof(GameObject));
			Spider = Resources.Load("Prefabs/Entities/Spider", typeof(GameObject));

			// Chapter 2 enemies
			SnowGolem = Resources.Load("Prefabs/Entities/SnowGolem", typeof(GameObject));
			IceGolem = Resources.Load("Prefabs/Entities/IceGolem", typeof(GameObject));
			FrostGolem = Resources.Load("Prefabs/Entities/FrostGolem", typeof(GameObject));
			IceWarrior = Resources.Load("Prefabs/Entities/IceWarrior", typeof(GameObject));

			// Chapter 3 enemies
			Goblin = Resources.Load("Prefabs/Entities/Goblin", typeof(GameObject));
			SpearGoblin = Resources.Load("Prefabs/Entities/SpearGoblin", typeof(GameObject));
			EliteGoblin = Resources.Load("Prefabs/Entities/EliteGoblin", typeof(GameObject));
			GoblinAssasin = Resources.Load("Prefabs/Entities/GoblinAssasin", typeof(GameObject));
			Slime = Resources.Load("Prefabs/Entities/Slime", typeof(GameObject));

			// Chapter 4 enemies
			LavaGolem = Resources.Load("Prefabs/Entities/LavaGolem", typeof(GameObject));
			PyroArcher = Resources.Load("Prefabs/Entities/PyroArcher", typeof(GameObject));
			PyroKnight = Resources.Load("Prefabs/Entities/PyroKnight", typeof(GameObject));
			BlastGolem = Resources.Load("Prefabs/Entities/BlastGolem", typeof(GameObject));
		}


		public void InstatiateEnemy(GameObject enemy, uint level)
		{
			var playerPosition = GameManager.INSTANCE.playerManager.transform.position;
			var spawnPosition = new Vector2(playerPosition.x + Random.Range(-5, 5), playerPosition.y + Random.Range(-5, 5));

			var enemyObject = Object.Instantiate(enemy, spawnPosition, Quaternion.identity);

			enemyObject.GetComponent<EnemyManager>().enemyLevel = (int)level;
		}
	}
}
