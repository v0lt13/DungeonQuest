using UnityEngine;
using DungeonQuest.Menus;
using DungeonQuest.Player;
using DungeonQuest.DebugConsole;

namespace DungeonQuest.Shop
{
	public class ShopMenu : MonoBehaviour
	{

		[Header("Shop config:")]
		[SerializeField] private GameObject shopMenu;
		[SerializeField] private GameObject prompt;
		[SerializeField] private AudioClip openShopSFX;

		public static bool IS_SHOP_OPEN;
		private bool canOpenShop;

		private Collider2D playerCollider;
		private AudioSource audioSource;

		void Awake()
		{
			playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
			audioSource = GetComponent<AudioSource>();
		}

		void Update()
		{
			if (DebugController.IS_CONSOLE_ON || PauseMenu.IS_GAME_PAUSED) return;

			if (Input.GetButtonDown("Interact") && canOpenShop)
			{
				if (IS_SHOP_OPEN)
				{
					CloseShop();
				}
				else
				{
					OpenShop();
				}

			}

			prompt.SetActive(canOpenShop);
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
			IS_SHOP_OPEN = !IS_SHOP_OPEN;
			Time.timeScale = IS_SHOP_OPEN ? 0f : 1f;

			shopMenu.SetActive(IS_SHOP_OPEN);
			GameManager.EnableCursor(IS_SHOP_OPEN);
		}

		private void OpenShop()
		{
			IS_SHOP_OPEN = !IS_SHOP_OPEN;
			Time.timeScale = IS_SHOP_OPEN ? 0f : 1f;

			shopMenu.SetActive(IS_SHOP_OPEN);
			audioSource.PlayOneShot(openShopSFX);
			GameManager.EnableCursor(IS_SHOP_OPEN);
		}
	}
}
