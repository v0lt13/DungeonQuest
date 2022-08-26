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

		public static List<Achievement> ACHIEVEMENTS;

		void Awake() 
		{
			achievementPopup01 = GameObject.Find("AchievementPopup01");
			achievementPopup02 = GameObject.Find("AchievementPopup02");

			popupAnimators[0] = achievementPopup01.GetComponent<Animator>();
			popupAnimators[1] = achievementPopup02.GetComponent<Animator>();
			achievementNameTexts[0] = achievementPopup01.GetComponentInChildren<Text>();
			achievementNameTexts[1] = achievementPopup02.GetComponentInChildren<Text>();

			Achievement.InitializePopoutComponents(popupAnimators, achievementNameTexts);

			if (ACHIEVEMENTS != null) return;

			ACHIEVEMENTS = new List<Achievement>
			{
				// Leveling acievements
				new Achievement("Newbie", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 5), // 0
				new Achievement("Advanced", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 10), // 1
				new Achievement("Experienced", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 15), // 2
				new Achievement("Master", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 20), // 3
				new Achievement("Overleveled!", (object o) => GameManager.INSTANCE.playerManager.playerLeveling.playerLevel == 25), // 4

				// Boss acievements
				new Achievement("And stay dead!", (object o) => GameManager.INSTANCE.bossesCompleted == 1), // 5
				new Achievement("Chill out!", (object o) => GameManager.INSTANCE.bossesCompleted == 2), // 6
				new Achievement("Sneak attack!", (object o) => GameManager.INSTANCE.bossesCompleted == 3), // 7
				new Achievement("Too hot to handle!", (object o) => GameManager.INSTANCE.bossesCompleted == 4), // 8

				// SecretLevels acievements
				new Achievement("Getting Creepy", (object o) => false), // 9
				new Achievement("Getting Frosty", (object o) => false), // 10
				new Achievement("Getting Sticky", (object o) => false), // 11
				new Achievement("Getting Sweaty", (object o) => false), // 12

				// Upgrade acievements
				new Achievement("Time for an Upgrade!", (object o) => false), // 13
				new Achievement("Swordsman!", (object o) => false), // 14
				new Achievement("Tank!", (object o) => false), // 15
				new Achievement("Fasttrack!", (object o) => false), // 16
				new Achievement("Full of life!", (object o) => false), // 17
				new Achievement("Vampire!", (object o) => false), // 18
				new Achievement("Overpowered!", (object o) => false), // 19

				// Misc acievements
				new Achievement("IM RICH!!!", (object o) => GameManager.INSTANCE.playerManager.coinsAmount >= 10000), // 20
				new Achievement("Secrets!", (object o) => GameManager.INSTANCE.secretCount == 1), // 21
				new Achievement("The Ultimate Knight", (object o) => MenuManager.GAME_COMPLETED), // 22
				new Achievement("Ultimate Ultimate Knight", (object o) => false) // 23
			};

			for (int i = 0; i < MenuManager.achivementCheckboxValues.Count; i++)
			{
				ACHIEVEMENTS[i].achieved = MenuManager.achivementCheckboxValues[i];
			}
		}

		void Update()
		{
			if (ACHIEVEMENTS == null) return;

			foreach (var achievement in ACHIEVEMENTS)
			{
				achievement.UpdateCompletion();
			}
		}

		public void UnlockAchivement(int achievementNumber)
		{
			ACHIEVEMENTS[achievementNumber].ActivateAchievement();
		}
	}
}
