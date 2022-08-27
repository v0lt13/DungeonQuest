using UnityEngine;
using DungeonQuest.Enemy.Boss.Projectiles;

namespace DungeonQuest.Enemy.Boss.Special
{
	public class BigChillSpecial : SpecialAbility
	{
		[SerializeField] private GameObject iceSpikePrefab;

		public override void Special()
		{
			for (int i = 1; i <= 8; i++)
			{
				var projectile = Instantiate(iceSpikePrefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
				var iceSpike = projectile.GetComponent<BossIceSpike>();

				switch (i)
				{
					case 1:
						// Up
						iceSpike.direction = new Vector2(0f, 1f);
						break;

					case 2:
						// Down
						iceSpike.direction = new Vector2(0f, -1f);
						iceSpike.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
						break;

					case 3:
						// Right
						iceSpike.direction = new Vector2(1f, 0f);
						iceSpike.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
						break;

					case 4:
						// Left
						iceSpike.direction = new Vector2(-1f, 0f);
						iceSpike.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
						break;

					case 5:
						// Up Right
						iceSpike.direction = new Vector2(1f, 1f);
						iceSpike.transform.rotation = Quaternion.Euler(0f, 0f, -45f);
						break;

					case 6:
						// Up Left
						iceSpike.direction = new Vector2(-1f, 1f);
						iceSpike.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
						break;

					case 7:
						// Down Right
						iceSpike.direction = new Vector2(1f, -1f);
						iceSpike.transform.rotation = Quaternion.Euler(0f, 0f, -135f);
						break;

					case 8:
						// Down Left
						iceSpike.direction = new Vector2(-1f, -1f);
						iceSpike.transform.rotation = Quaternion.Euler(0f, 0f, 135f);
						break;
				}
			}
		}
	}
}
