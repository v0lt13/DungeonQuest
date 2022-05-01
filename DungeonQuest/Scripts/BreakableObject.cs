using UnityEngine;

namespace DungeonQuest
{
	public class BreakableObject : MonoBehaviour
	{
		[SerializeField] private Sprite brokenSprite;

		[Header("Drops Config:")]
		[SerializeField] private int coinDropChance;
		[SerializeField] private int pileOfCoinsDropChance;

		[Header("Prefab Config:")]
		[SerializeField] private GameObject pileOfCoinsPrefab;
		[SerializeField] private GameObject coinsPrefab;		

		public void BreakObject()
		{
			DropLoot();

			audio.Play();
			collider2D.enabled = false;

			GetComponent<SpriteRenderer>().sprite = brokenSprite;

			// We mark the node for the AI pathfinding as walkable
			FindObjectOfType<Grid.GridGenerator>().MarkObstacle(transform.position, true);
		}

		public void DropLoot()
		{
			var dropChance = Random.Range(1, 100);

			if (dropChance <= coinDropChance)
				Instantiate(coinsPrefab, new Vector2(transform.position.x + Random.Range(-3f, 3f), transform.position.y + Random.Range(-3f, 3f)), Quaternion.identity);

			if (dropChance <= pileOfCoinsDropChance)
				Instantiate(pileOfCoinsPrefab, new Vector2(transform.position.x + Random.Range(-3f, 3f), transform.position.y + Random.Range(-3f, 3f)), Quaternion.identity);
		}
	}
}
