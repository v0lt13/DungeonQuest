using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Player;

namespace DungeonQuest.Shop
{
	public class ShopItem : MonoBehaviour
	{
		[Header("ItemConfig:")]
		public int minRequiredLevel;
		[Space(10f)]
		[SerializeField] private Item item;
		[SerializeField] private Image itemIcon;
		[SerializeField] private Button buyButton;
		[Space(10f)]
		[SerializeField] private Text itemNameText;
		[SerializeField] private Text itemDescriptionText;
		[SerializeField] private Text itemPriceText;
		[SerializeField] private Text problemText;

		private PlayerManager playerManager;

		void Awake()
		{
			itemDescriptionText.text = item.itemDescription;
			itemNameText.text = item.itemName;
			itemPriceText.text = item.itemPrice.ToString();
			itemIcon.sprite = item.itemIcon;

			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		}

		void Update()
		{
			var playerLeveling = playerManager.playerLeveling;

			if (playerLeveling.playerLevel < minRequiredLevel)
			{
				if (playerLeveling.playerLevel == playerLeveling.maxLevel)
				{
					HoustonWeHaveProblem("Maxed out");
					return;
				}

				HoustonWeHaveProblem("level " + minRequiredLevel.ToString() + " required");
			}
			else if (playerManager.coinsAmount < item.itemPrice)
			{
				HoustonWeHaveProblem("you're to poor");
			}
			else
			{
				buyButton.interactable = true;
				problemText.text = string.Empty;
			}
		}

		public void Buy()
		{
			playerManager.GiveCoins(-item.itemPrice); // We substract the coins

			GameManager.INSTANCE.gameData.SavePlayerData();
			GameManager.INSTANCE.gameData.SaveGameData();
		}

		public void IncreaseMinRequiredLevel(int amount)
		{
			minRequiredLevel += amount;
		}

		private void HoustonWeHaveProblem(string problem)
		{
			buyButton.interactable = false;
			problemText.text = problem;
		}
	}
}
