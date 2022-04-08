using UnityEngine;

namespace DungeonQuest.Menus
{
	public class MenuManager : MonoBehaviour
	{
		[SerializeField] private GameObject[] menus;

		private int currentMenu;

		void Awake()
		{
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
				GoBackToMainMenu();
			}
		}

		public void Play() // Called by Button
		{
			GameManager.LoadScene("Level0");
		}

		public void Options() // Called by Button
		{
			ToggleMenu(1);
		}

		public void TutorialMenu() // Called by Button
		{
			ToggleMenu(2);
		}

		public void Credits() // Called by Button
		{
			ToggleMenu(3);
		}

		public void Exit() // Called by Button
		{
			Application.Quit();
		}

		public void GoBackToMainMenu() // Called by Button
		{
			ToggleMenu(0);
		}

		public void OpenURL(string url) // Called by Button
		{
			Application.OpenURL(url);
		}

		private void ToggleMenu(int menuIndex)
		{
			currentMenu = menuIndex;

			foreach (var menu in menus)
			{
				menu.SetActive(false);
			}

			menus[menuIndex].SetActive(true);
		}
	}
}
