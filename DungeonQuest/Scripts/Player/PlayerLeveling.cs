using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerLeveling : MonoBehaviour
	{
		[SerializeField] private AudioClip levelUpSFX;

		public readonly int maxLevel = 25;
		[HideInInspector] public int playerLevel = 1;
		[HideInInspector] public int nextLevelXP = 100;
		
		private PlayerManager playerManager;
		private Animator levelupAnim;
		private Text levelText;
		private Slider xpBar;

		public int PlayerXP { get; set; }
		public bool IsPlayerMaxLevel { get; private set; }

		void Awake() 
		{
			xpBar = GameObject.Find("PlayerXPBar").GetComponent<Slider>();
			levelText = GameObject.Find("PlayerLevelText").GetComponent<Text>();
			levelupAnim = GameObject.Find("Vignette").GetComponent<Animator>();

			playerManager = GetComponent<PlayerManager>();
		}

		void Update()
		{
			levelText.text = "lvl " + playerLevel.ToString();

			if (playerLevel == maxLevel)
			{
				IsPlayerMaxLevel = true;
				return;
			}

			xpBar.maxValue = nextLevelXP;
			xpBar.value = PlayerXP;

			if (PlayerXP >= nextLevelXP)
			{
				LevelUp();
			}
		}

		public void LevelUp()
		{
			playerLevel++;
			PlayerXP = 0;
			nextLevelXP *= 2;

			levelupAnim.Play("Levelup", -1, 0f);
			playerManager.IncreaseMaxHealth(20);
			playerManager.IncreaseMaxArmor(20);
			playerManager.playerAttack.IncreaseDamage(10);

			audio.clip = levelUpSFX;
			audio.pitch = 1f;
			audio.Play();
		}
	}
}
