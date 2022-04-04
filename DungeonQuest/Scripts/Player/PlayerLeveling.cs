using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerLeveling : MonoBehaviour
	{
		[Header("UI Config:")]
		[SerializeField] private Slider xpBar;
		[SerializeField] private Text levelText;

		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip levelUpSFX;

		private int playerLevel = 1;
		private int nextLevelXP = 100;

		private PlayerManager playerManager;

		public int PlayerXP { get; set; }
		public int GetPlayerLevel { get { return playerLevel; } }

		void Awake() 
		{
			playerManager = GetComponent<PlayerManager>();

			xpBar.maxValue = nextLevelXP;
			levelText.text = "lvl " + playerLevel.ToString();
		}
		
		void Update()
		{
			xpBar.value = PlayerXP;

			if (PlayerXP >= nextLevelXP)
			{
				LevelUp();
			}
		}

		public void LevelUp()
		{
			playerLevel++;
			PlayerXP = PlayerXP - nextLevelXP;
			levelText.text = "lvl " + playerLevel.ToString();

			nextLevelXP += 100;
			playerManager.IncreaseMaxHealth(10);
			playerManager.IncreaseMaxArmor(10);
			playerManager.playerAttack.IncreaseDamage(10);

			xpBar.maxValue = nextLevelXP;

			audioSource.clip = levelUpSFX;
			audioSource.Play();
		}
	}
}
