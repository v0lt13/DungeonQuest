using UnityEngine;
using DungeonQuest.Enemy.Boss.Projectiles;

namespace DungeonQuest.Enemy.Boss.Special
{
	public class FireLordSpecial : SpecialAbility 
	{
		[SerializeField] private GameObject fireballPrefab;
		[SerializeField] private GameObject[] enemyPrefabs;

		public override void Special()
		{
			var rng = Random.Range(1, 100);

			if (rng <= 50)
			{
				SummonEnemies();
			}
			else
			{
				ShootFireballs();
			}
		}

		private void SummonEnemies()
		{
			for (int i = 0; i < 4; i++)
			{
				var spawnPosition = new Vector2(transform.position.x + Random.Range(-20, 20), transform.position.y + Random.Range(-20, 20));
				var enemyObject = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity) as GameObject;

				enemyObject.GetComponent<EnemyManager>().enemyLevel = 24;
			}
		}

		private void ShootFireballs()
		{
			for (int i = 1; i <= 8; i++)
			{
				var projectile = Instantiate(fireballPrefab, transform.position, new Quaternion(0f, 0f, 0f, 0f)) as GameObject;
				var fireball = projectile.GetComponent<BossFireball>();

				switch (i)
				{
					case 1:
						// Up
						fireball.direction = new Vector2(0f, 1f);
						break;

					case 2:
						// Down
						fireball.direction = new Vector2(0f, -1f);
						fireball.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
						break;

					case 3:
						// Right
						fireball.direction = new Vector2(1f, 0f);
						fireball.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
						break;

					case 4:
						// Left
						fireball.direction = new Vector2(-1f, 0f);
						fireball.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
						break;

					case 5:
						// Up Right
						fireball.direction = new Vector2(1f, 1f);
						fireball.transform.rotation = Quaternion.Euler(0f, 0f, -45f);
						break;

					case 6:
						// Up Left
						fireball.direction = new Vector2(-1f, 1f);
						fireball.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
						break;

					case 7:
						// Down Right
						fireball.direction = new Vector2(1f, -1f);
						fireball.transform.rotation = Quaternion.Euler(0f, 0f, -135f);
						break;

					case 8:
						// Down Left
						fireball.direction = new Vector2(-1f, -1f);
						fireball.transform.rotation = Quaternion.Euler(0f, 0f, 135f);
						break;
				}
			}
		}
	}
}
