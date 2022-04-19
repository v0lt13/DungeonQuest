﻿using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerLeveling : MonoBehaviour
	{
		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip levelUpSFX;

		public readonly int maxLevel = 20;
		[HideInInspector] public int playerLevel = 1;
		[HideInInspector] public int nextLevelXP = 100;
		
		private PlayerManager playerManager;
		private Text levelText;
		private Slider xpBar;

		public int PlayerXP { get; set; }
		public bool IsPlayerMaxLevel { get; private set; }

		void Awake() 
		{
			xpBar = GameObject.Find("PlayerXPBar").GetComponent<Slider>();
			levelText = GameObject.Find("PlayerLevelText").GetComponent<Text>();

			playerManager = GetComponent<PlayerManager>();
		}

		void Update()
		{
			levelText.text = "lvl " + playerLevel.ToString();
			xpBar.maxValue = nextLevelXP;
			xpBar.value = PlayerXP;

			if (playerLevel == maxLevel)
			{
				IsPlayerMaxLevel = true;
				nextLevelXP = 1;
				PlayerXP = 1;
			}

			if (PlayerXP >= nextLevelXP && !IsPlayerMaxLevel)
			{
				LevelUp();
			}
		}

		public void LevelUp()
		{
			if (IsPlayerMaxLevel) return;

			playerLevel++;
			PlayerXP = 0;
			nextLevelXP *= 2;

			playerManager.IncreaseMaxHealth(10);
			playerManager.IncreaseMaxArmor(10);
			playerManager.playerAttack.IncreaseDamage(5);

			audioSource.clip = levelUpSFX;
			audioSource.Play();
		}
	}
}
