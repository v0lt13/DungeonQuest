﻿using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Pickups
{
	public class ArmorPickup : MonoBehaviour
	{
		[SerializeField] private int amountGiven;
		[SerializeField] private bool destroyOnPickup;

		private Collider2D playerCollider;

		void Awake()
		{
			playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (playerCollider == collider)
			{
				var player = collider.GetComponent<PlayerManager>();

				if (player.PlayerArmor == player.defaultPlayerArmor) return;
					
				player.ArmorPlayer(amountGiven);

				if (destroyOnPickup) Destroy(gameObject);
			}
		}
	}
}
