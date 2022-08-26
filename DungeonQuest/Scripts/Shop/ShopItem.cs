using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Player;
using DungeonQuest.Data;

namespace DungeonQuest.Shop
{
	public class ShopItem : MonoBehaviour
	{
		public enum AditionalProblem
		{
			None,
			HealthCap,
			ArmorCap,
			PotionCap
		}

		[Header("ItemConfig:")]
		public int minRequiredLevel;
		[SerializeField] private AditionalProblem aditionalProblem;
		[Space(10f)]
		[SerializeField] private Item item;
		[SerializeField] private Image itemIcon;
		[SerializeField] private Button buyButton;
		[Space(10f)]
		[SerializeField] private Text itemNameText;
		[SerializeField] private Text itemDescriptionText;
		[SerializeField] private Text itemPriceText;
		[SerializeField] private Text problemText;

		[HideInInspector] public bool isUpgradeMaxed;

		private const int MAX_UPGARDE_LEVEL = 20;

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

			switch (itemNameText.text)
			{
				case "Health Upgrade":
					if (isUpgradeMaxed) GameManager.INSTANCE.achievementManager.UnlockAchivement(17);
					break;

				case "Lifesteal":
					if (isUpgradeMaxed) GameManager.INSTANCE.achievementManager.UnlockAchivement(18);
					break;

				case "Armor Upgrade":
					if (isUpgradeMaxed) GameManager.INSTANCE.achievementManager.UnlockAchivement(15);
					break;

				case "Speed Upgrade":
					if (isUpgradeMaxed) GameManager.INSTANCE.achievementManager.UnlockAchivement(16);
					break;

				case "Sword Upgrade":
					if (isUpgradeMaxed) GameManager.INSTANCE.achievementManager.UnlockAchivement(14);
					break;
			}

			if (minRequiredLevel > MAX_UPGARDE_LEVEL)
			{
				isUpgradeMaxed = true;
				HoustonWeHaveProblem("Maxed out");
				return;
			}

			if (playerLeveling.playerLevel < minRequiredLevel)
			{
				HoustonWeHaveProblem("Level " + minRequiredLevel.ToString() + " required");
			}
			else if (playerManager.coinsAmount < item.itemPrice)
			{
				HoustonWeHaveProblem("You're too poor");
			}
			else
			{
				buyButton.interactable = true;
				problemText.text = string.Empty;
			}

			switch (aditionalProblem)
			{
				case AditionalProblem.HealthCap:
					if (playerManager.playerHealth == playerManager.defaultPlayerHealth) HoustonWeHaveProblem("Health is full");
					break;

				case AditionalProblem.ArmorCap:
					if (playerManager.playerArmor == playerManager.defaultPlayerArmor) HoustonWeHaveProblem("Armor is full");
					break;

				case AditionalProblem.PotionCap:
					if (playerManager.playerHealing.healingPotions == PlayerHealing.POTION_CAP) HoustonWeHaveProblem("Max potions");
					break;
			}
		}

		public void Buy()
		{
			playerManager.GiveCoins(-item.itemPrice); // We substract the coins

			GameManager.INSTANCE.gameData.SaveData(GameDataHandler.DataType.Player);
			GameManager.INSTANCE.gameData.SaveData(GameDataHandler.DataType.Game);
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
