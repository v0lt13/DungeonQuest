using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.UI.Menus
{
	public class MenuManager : MonoBehaviour
	{
		[SerializeField] private Button continueButton;
		[SerializeField] private GameObject[] menus;

		private int currentMenu;

		void Awake()
		{
			continueButton.interactable = File.Exists(Application.dataPath + "/Data/PlayerData.dat");

			if (!Directory.Exists(Application.dataPath + "/Data"))
			{
				Directory.CreateDirectory(Application.dataPath + "/Data");
			}

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
				ToggleMenu(0);
			}
		}

		public void NewGame() // Called by Button
		{
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
