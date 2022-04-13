using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerLeveling : MonoBehaviour
	{
		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip levelUpSFX;

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
			xpBar.value = PlayerXP;
			xpBar.maxValue = nextLevelXP;
			levelText.text = "lvl " + playerLevel.ToString();

			if (PlayerXP >= nextLevelXP && !IsPlayerMaxLevel)
			{
				LevelUp();
			}

			if (playerLevel == 100)
			{
				IsPlayerMaxLevel = true;
				nextLevelXP = 0;
				xpBar.value = xpBar.maxValue;
			}
		}

		public void LevelUp()
		{
			if (IsPlayerMaxLevel) return;

			playerLevel++;
			PlayerXP = 0;

			nextLevelXP += 100;
			playerManager.IncreaseMaxHealth(10);
			playerManager.IncreaseMaxArmor(10);
			playerManager.playerAttack.IncreaseDamage(5);

			audioSource.clip = levelUpSFX;
			audioSource.Play();
		}
	}
}
