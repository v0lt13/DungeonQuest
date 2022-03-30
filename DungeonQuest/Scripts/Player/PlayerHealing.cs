﻿using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Shop;
using DungeonQuest.Menus;
using DungeonQuest.DebugConsole;

namespace DungeonQuest.Player
{
	public class PlayerHealing : MonoBehaviour
	{
		[Header ("Healing Config;")]
		[SerializeField] private float defaultCooldown;
		[Space(10f)]
		[SerializeField] private Slider cooldownSlider;
		[SerializeField] private Text healingPotionsAmount;
		[SerializeField] private AudioSource healingSFX;

		private PlayerManager playerManager;

		public int HealingPotions { get; private set; }
		public float Cooldown { get; private set; }

		void Awake()
		{
			playerManager = GetComponent<PlayerManager>();

			Cooldown = defaultCooldown;
			cooldownSlider.maxValue = defaultCooldown;
		}

		void Update()
		{

			cooldownSlider.value = Cooldown;
			healingPotionsAmount.text = HealingPotions.ToString();

			if (Cooldown > 0f)
			{
				Cooldown -= Time.deltaTime;
			}

			if (Input.GetButtonDown("Heal") && Cooldown <= 0f && HealingPotions != 0 && !playerManager.IsDead && !GameManager.INSTANCE.LevelEnded && !PauseMenu.IS_GAME_PAUSED && !DebugController.IS_CONSOLE_ON && !ShopMenu.IS_SHOP_OPEN)
			{
				Cooldown = defaultCooldown;
				HealingPotions--;

				playerManager.HealPlayer(playerManager.GetDefaultPlayerHealth);
				healingSFX.Play();
			}
		}

		public void AddPotion()
		{
			HealingPotions++;
		}
	}
}
