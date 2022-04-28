﻿using UnityEngine;

namespace DungeonQuest.Shop
{
	public class ShopMenu : MonoBehaviour
	{
		[Header("Shop Config:")]
		[SerializeField] private GameObject shopMenu;
		[SerializeField] private GameObject prompt;

		private bool isShopOpen;
		private bool canOpenShop;

		private Collider2D playerCollider;

		void Awake()
		{
			playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
		}

		void Update()
		{
			if (Input.GetButtonDown("Interact") && canOpenShop)
			{
				if (!isShopOpen && GameManager.INSTANCE.CurrentGameState != GameManager.GameState.Paused)
				{
					isShopOpen = true;

					audio.Play();
					GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
				}
			}

			prompt.SetActive(canOpenShop);
			shopMenu.SetActive(isShopOpen);
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider == playerCollider) canOpenShop = true;
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			if (collider == playerCollider) canOpenShop = false;
		}

		public void CloseShop() // Called by Button
		{
			isShopOpen = false;

			GameManager.INSTANCE.SetGameState(GameManager.GameState.Running);
		}
	}
}
