using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerLeveling : MonoBehaviour
	{
		[SerializeField] private int nextLevelXP;
		[SerializeField] private Slider xpBar;
		[SerializeField] private Text levelText;
		[SerializeField] private AudioSource levelUpSFX;

		private PlayerManager playerManager;

		public int PlayerLevel { get; private set; }
		public int PlayerXP { get; set; }

		void Awake() 
		{
			playerManager = GetComponent<PlayerManager>();

			if (PlayerLevel == 0) PlayerLevel = 1;

			xpBar.maxValue = nextLevelXP;
			levelText.text = "lvl " + PlayerLevel.ToString();
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
			PlayerLevel++;
			PlayerXP = PlayerXP - nextLevelXP;
			levelText.text = "lvl " + PlayerLevel.ToString();

			nextLevelXP += 100;
			playerManager.IncreaseMaxHealth(10);
			playerManager.IncreaseMaxArmor(10);
			playerManager.playerAttack.IncreaseDamage(10);

			xpBar.maxValue = nextLevelXP;

			levelUpSFX.Play();
		}
	}
}
