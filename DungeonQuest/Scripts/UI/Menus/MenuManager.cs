using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Data;
using DungeonQuest.Player;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace DungeonQuest.UI.Menus
{
	public class MenuManager : MonoBehaviour
	{
		[SerializeField] private Button continueButton;
		[SerializeField] private Button rogueButton;
		[SerializeField] private Text rogueText;
		[SerializeField] private Text achievementPercentange;
		[Space]
		[SerializeField] private GameObject[] menus;
		[SerializeField] private Toggle[] achievementCheckboxes;

		public static bool GAME_COMPLETED;
		public static List<bool> achivementCheckboxValues = new List<bool>();

		private int currentMenu;
		private AudioSource[] audioSources;
		private GameDataHandler data = new GameDataHandler();

		void Awake()
		{
			audioSources = GetComponents<AudioSource>();

			if (!Directory.Exists(Application.persistentDataPath + "/Data"))
			{
				Directory.CreateDirectory(Application.persistentDataPath + "/Data");
			}

			data.LoadMenuData();

			for (int i = 0; i < achivementCheckboxValues.Count; i++)
			{
				achievementCheckboxes[i].isOn = achivementCheckboxValues[i];
			}

			rogueButton.interactable = GAME_COMPLETED;
			rogueText.text = GAME_COMPLETED ? "Survive the whole campaign with only 1 life" : "Complete the game on Normal mode to unlock Rogue mode";

			continueButton.interactable = File.Exists(Application.persistentDataPath + "/Data/PlayerData.dat");

			foreach (var audioSource in audioSources)
			{
				audioSource.ignoreListenerPause = true;
			}

			var numberOfUnlockedAchievements = 0f;

			for (int i = 0; i < achivementCheckboxValues.Count; i++)
			{
				if (achivementCheckboxValues[i])
				{
					numberOfUnlockedAchievements++;
				}
			}

			var achivevementPercentage = (numberOfUnlockedAchievements / achivementCheckboxValues.Count * 100); // Calculate the percentage of achievements unlocked

			achievementPercentange.text = numberOfUnlockedAchievements != 0 ? "Completed: " + achivevementPercentage.ToString("n0") + "%" : "Completed: 0%";
		}

		void Start()
		{
			GetComponent<OptionsMenu>().LoadSettings();

			// Fix for settings checkboxes SFX playing when the values load
			foreach (var audioSource in audioSources)
			{
				audioSource.enabled = true;
			}
		}

		void Update()
		{
			if (Input.GetButtonDown("Back") && currentMenu != 0)
			{
				if (currentMenu != 5)
				{
					ToggleMenu(0);
				}
				else
				{
					ToggleMenu(1);
				}
			}
		}

		public void NewGame() // Called by Button
		{
			PlayerManager.ROGUE_MODE = false;

			File.Delete(Application.persistentDataPath + "/Data/PlayerData.dat");
			File.Delete(Application.persistentDataPath + "/Data/GameData.dat");

			SceneManager.LoadScene("Intermission01");
		}

		public void NewRogueGame() // Called by Button
		{
			PlayerManager.ROGUE_MODE = true;

			File.Delete(Application.persistentDataPath + "/Data/PlayerData.dat");
			File.Delete(Application.persistentDataPath + "/Data/GameData.dat");

			SceneManager.LoadScene("Intermission01");
		}

		public void Continue()
		{
			GameManager.LoadScene("Lobby");
		}

		public void ToggleMenu(int menuIndex) // Called by Button
		{
			currentMenu = menuIndex;

			foreach (var menu in menus)
			{
				menu.SetActive(false);
			}

			menus[menuIndex].SetActive(true);
		}

		public void Exit() // Called by Button
		{
			Application.Quit();
		}

		public void OpenURL(string url) // Called by Button
		{
			Application.OpenURL(url);
		}
	}
}
