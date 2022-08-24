using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Data;
using DungeonQuest.Player;

namespace DungeonQuest.UI.Menus
{
	public class MenuManager : MonoBehaviour
	{
		[SerializeField] private Button continueButton;
		[SerializeField] private Button rogueButton;
		[SerializeField] private Text rogueText;
		[Space(10f)]
		[SerializeField] private GameObject[] menus;

		public static bool GAME_COMPLETED;

		private int currentMenu;
		private GameDataHandler data = new GameDataHandler(); 

		void Awake()
		{
			if (!Directory.Exists(Application.dataPath + "/Data"))
			{
				Directory.CreateDirectory(Application.dataPath + "/Data");
			}

			data.LoadMenuData();

			continueButton.interactable = File.Exists(Application.dataPath + "/Data/PlayerData.dat");
			rogueButton.interactable = GAME_COMPLETED;
			rogueText.text = GAME_COMPLETED ? "Survive the whole campaign with only 1 life" : "Complete the game on Normal mode to unlock Rogue mode";

			var audioSources = GetComponents<AudioSource>();

			foreach (var audioSource in audioSources)
			{
				audioSource.ignoreListenerPause = true;
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

			File.Delete(Application.dataPath + "/Data/PlayerData.dat");
			File.Delete(Application.dataPath + "/Data/GameData.dat");

			Application.LoadLevel("Intermission01");
		}

		public void NewRogueGame() // Called by Button
		{
			PlayerManager.ROGUE_MODE = true;

			File.Delete(Application.dataPath + "/Data/PlayerData.dat");
			File.Delete(Application.dataPath + "/Data/GameData.dat");

			Application.LoadLevel("Intermission01");
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
