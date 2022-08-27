using System;
using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Data;

namespace DungeonQuest.Achievements
{
	public class Achievement
	{
		public bool achieved;
		public string name;

		private Predicate<object> requirement;
		private GameDataHandler gameData = new GameDataHandler();

		private static Animator[] popupAnimators;
		private static Text[] achievementNameTexts;

		public Achievement(string name, Predicate<object> requirement)
		{
			this.name = name;
			this.requirement = requirement;
		}

		public void UpdateCompletion()
		{
			if (RequirementsMet())
			{
				ActivateAchievement();
			}
		}

		public static void InitializePopoutComponents(Animator[] popoutAnimators, Text[] texts)
		{
			popupAnimators = popoutAnimators;
			achievementNameTexts = texts;
		}

		public void ActivateAchievement()
		{
			if (achieved) return;

			achieved = true;

			// Check if the first animation popup plays and play the animation on the second popup if it does
			if (popupAnimators[0].GetCurrentAnimatorClipInfo(0).Length < popupAnimators[0].GetCurrentAnimatorStateInfo(0).normalizedTime)
			{
				popupAnimators[0].Play("AchievementPopup");
				achievementNameTexts[0].text = name;

			}
			else
			{
				popupAnimators[1].Play("AchievementPopup");
				achievementNameTexts[1].text = name;
			}

			gameData.SaveData(GameDataHandler.DataType.Menu);
		}

		private bool RequirementsMet()
		{
			return requirement.Invoke(null);
		}
	}
}
