using UnityEngine;

namespace DungeonQuest.Enemy
{
	public class EnemyDrops : MonoBehaviour
	{
		[Header("Drops Config:")]
		[SerializeField] private int healthDropChance;
		[SerializeField] private int coinDropChance;
		[SerializeField] private int pileOfCoinsDropChance;
		[SerializeField] private int minXpDrop;
		[SerializeField] private int maxXpDrop;

		[Header ("Prefab Config:")]
		[SerializeField] private GameObject healthPotionPrefab;
		[SerializeField] private GameObject pileOfCoinsPrefab;
		[SerializeField] private GameObject coinsPrefab;

		private EnemyManager enemyManager;

		public int GetMinXpDrop { get { return minXpDrop; } }
		public int GetMaxXpDrop { get { return maxXpDrop; } }

		void Awake()
		{
			enemyManager = GetComponent<EnemyManager>();
		}

		void Start()
		{
			minXpDrop *= enemyManager.enemyLevel;
			maxXpDrop *= enemyManager.enemyLevel;
		}

		public void DropLoot()
		{
			var dropChance = Random.Range(1, 100);

			if (dropChance <= healthDropChance)
				Instantiate(healthPotionPrefab, new Vector2(transform.position.x + Random.Range(-5f, 5f), transform.position.y), Quaternion.identity);

			if (dropChance <= coinDropChance)
				Instantiate(coinsPrefab, new Vector2(transform.position.x + Random.Range(-5f, 5f), transform.position.y), Quaternion.identity);

			if (dropChance <= pileOfCoinsDropChance)
				Instantiate(pileOfCoinsPrefab, new Vector2(transform.position.x + Random.Range(-5f, 5f), transform.position.y), Quaternion.identity);
		}
	}
}
