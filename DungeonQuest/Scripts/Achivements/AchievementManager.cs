using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Player;
using DungeonQuest.UI.Menus;
using System.Collections.Generic;

namespace DungeonQuest.Achievements
{
	public class AchievementManager : MonoBehaviour 
	{
		private GameObject achievementPopup01, achievementPopup02;

		private Text[] achievementNameTexts = new Text[2];
		private Animator[] popupAnimators = new Animator[2];

		public static List<Achievement> ACHIEVEMENT_LIST;

		void Awake() 
		{
			achievementPopup01 = GameObject.Find("AchievementPopup01");
			achievementPopup02 = GameObject.Find("AchievementPopup02");

			popupAnimators[0] = achievementPopup01.GetComponent<Animator>();
			popupAnimators[1] = achievementPopup02.GetComponent<Animator>();
			achievementNameTexts[0] = achievementPopup01.GetComponentInChildren<Text>();
			achievementNameTexts[1] = achievementPopup02.GetComponentInChildren<Text>();

			Achievement.InitializePopoutComponents(popupAnimators, achievementNameTexts); // Reinitialize the animators and texts for the achievement popout after loading a new scene

			if (ACHIEVEMENT_LIST != null) return;

			ACHIEVEMENT_LIST = new List<Achievement>
			{
				// Leveling acievements
				new Achievement("Newbie", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 5),
				new Achievement("Advanced", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 10),
				new Achievement("Experienced", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 15),
				new Achievement("Master", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 20),
				new Achievement("Overleveled!", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 25),

				// Boss acievements
				new Achievement("And stay dead!", (object o) => GameManager.INSTANCE.bossesCompleted == 1),
				new Achievement("Chill out!", (object o) => GameManager.INSTANCE.bossesCompleted == 2),
				new Achievement("Sneak attack!", (object o) => GameManager.INSTANCE.bossesCompleted == 3),
				new Achievement("Too hot to handle!", (object o) => GameManager.INSTANCE.bossesCompleted == 4),

				// SecretLevels acievements
				new Achievement("Getting Creepy", (object o) => false),
				new Achievement("Getting Frosty", (object o) => false),
				new Achievement("Getting Sticky", (object o) => false),
				new Achievement("Getting Sweaty", (object o) => false),

				// Upgrade acievements
				new Achievement("Time for an Upgrade!", (object o) => false),
				new Achievement("Swordsman!", (object o) => false),
				new Achievement("Tank!", (object o) => false),
				new Achievement("Fasttrack!", (object o) => false),
				new Achievement("Full of life!", (object o) => false),
				new Achievement("Vampire!", (object o) => false),
				new Achievement("Overpowered!", (object o) => false),

				// Misc acievements
				new Achievement("IM RICH!!!", (object o) => GameManager.INSTANCE.playerManager.coinsAmount >= 10000),
				new Achievement("Secrets!", (object o) => GameManager.INSTANCE.secretCount == 1),
				new Achievement("The Ultimate Knight", (object o) => MenuManager.GAME_COMPLETED),
				new Achievement("Ultimate Ultimate Knight", (object o) => false)
			};

			for (int i = 0; i < MenuManager.achivementCheckboxValues.Count; i++)
			{
				ACHIEVEMENT_LIST[i].achieved = MenuManager.achivementCheckboxValues[i];
			}
		}

		void Update()
		{
			if (ACHIEVEMENT_LIST == null) return;

			foreach (var achievement in ACHIEVEMENT_LIST)
			{
				achievement.UpdateCompletion();
			}
		}

		public void UnlockAchivement(int achievementNumber)
		{
			ACHIEVEMENT_LIST[achievementNumber].ActivateAchievement();
		}
	}
}
