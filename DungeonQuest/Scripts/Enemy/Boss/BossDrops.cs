﻿using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy.Boss
{
	public class BossDrops : MonoBehaviour
	{
		[Header("Drops Config:")]
		[SerializeField] private int healthDropAmount;
		[SerializeField] private int coinDropAmount;
		[SerializeField] private int pileOfCoinsDropAmount;
		[SerializeField] private int xpDrop;

		[Header("Replay Drops Config:")]
		[SerializeField] private int replayHealthDropAmount;
		[SerializeField] private int replayCoinDropAmount;
		[SerializeField] private int replayPileOfCoinsDropAmount;
		[SerializeField] private int replayXpDrop;

		[Header("Prefab Config:")]
		[SerializeField] private GameObject healthPotionPrefab;
		[SerializeField] private GameObject pileOfCoinsPrefab;
		[SerializeField] private GameObject coinsPrefab;

		private BossManager bossManager;

		void Awake()
		{
			bossManager = GetComponent<BossManager>();
		}

		public void DropLoot()
		{
			// Check if player has not beaten the boss before else give him less loot
			if (GameManager.INSTANCE.bossesCompleted < bossManager.GetBossID)
			{
				bossManager.playerManager.playerLeveling.PlayerXP += xpDrop;

				for (int i = healthDropAmount; i > 0; i--)
					Instantiate(healthPotionPrefab, new Vector2(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f)), Quaternion.identity);

				for (int i = coinDropAmount; i > 0; i--)
					Instantiate(coinsPrefab, new Vector2(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f)), Quaternion.identity);

				for (int i = pileOfCoinsDropAmount; i > 0; i--)
					Instantiate(pileOfCoinsPrefab, new Vector2(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f)), Quaternion.identity);
			}
			else
			{
				bossManager.playerManager.playerLeveling.PlayerXP += replayXpDrop;

				for (int i = replayHealthDropAmount; i > 0; i--)
					Instantiate(healthPotionPrefab, new Vector2(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f)), Quaternion.identity);

				for (int i = replayCoinDropAmount; i > 0; i--)
					Instantiate(coinsPrefab, new Vector2(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f)), Quaternion.identity);

				for (int i = replayPileOfCoinsDropAmount; i > 0; i--)
					Instantiate(pileOfCoinsPrefab, new Vector2(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f)), Quaternion.identity);
			}
		}
	}
}
