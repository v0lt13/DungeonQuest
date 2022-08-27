using UnityEngine;
using DungeonQuest.Player;
using DungeonQuest.Enemy.Boss.Projectiles;

namespace DungeonQuest.Enemy.Boss.Special
{
	public class GoblinChiefSpecial : SpecialAbility
	{
		[SerializeField] private float spread;
		[SerializeField] private float minRange;
		[SerializeField] private float maxRange;
		[Space(10f)]
		[SerializeField] private GameObject dynamitePrefab;
		[SerializeField] private GameObject player;

		public override void Special()
		{
			for (int i = 0; i < 5; i++)
			{
				var dynamiteObject = Instantiate(dynamitePrefab, transform.position, Quaternion.identity);

				var dynamite = dynamiteObject.GetComponent<Dynamite>();
				var playerTransform = player.GetComponent<PlayerManager>().transform.position;

				float spreadX = Random.Range(-spread, spread);
				float spreadY = Random.Range(-spread, spread);

				dynamite.direction = (Vector2)playerTransform + new Vector2(spreadX, spreadY);
			}
		}
	}
}