using UnityEngine;
using DungeonQuest.UI.Menus;

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
		private PauseMenu pauseMenu;

		void Awake()
		{
			playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
			pauseMenu = GameObject.Find("GameCanvas").GetComponent<PauseMenu>();
		}

		void Update()
		{
			if (Input.GetButtonDown("Interact") && canOpenShop)
			{
				if (!isShopOpen && GameManager.INSTANCE.CurrentGameState != GameManager.GameState.Paused)
				{
					isShopOpen = true;
					pauseMenu.enabled = false;

					audio.Play();
					GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
				}

			}

			if (Input.GetButtonDown("Back") && isShopOpen) CloseShop();

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

		public void CloseShop()
		{
			isShopOpen = false;
			pauseMenu.enabled = true;

			GameManager.INSTANCE.SetGameState(GameManager.GameState.Running);
		}
	}
}
