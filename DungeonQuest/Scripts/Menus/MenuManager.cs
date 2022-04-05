using UnityEngine;

namespace DungeonQuest.Menus
{
	public class MenuManager : MonoBehaviour
	{
		[SerializeField] private GameObject[] menus;

		private int currentMenu;

		void Update()
		{
			if (Input.GetButtonDown("Back") && currentMenu != 0)
			{
				GoBackToMainMenu();
			}
		}

		public void Play() // Button
		{
			GameManager.LoadScene("Level0");
		}

		public void Options() // Button
		{
			ToggleMenu(1);
		}

		public void TutorialMenu() // Button
		{
			ToggleMenu(2);
		}

		public void Credits() // Button
		{
			ToggleMenu(3);
		}

		public void Exit() // Button
		{
			Application.Quit();
		}

		public void GoBackToMainMenu() // Button
		{
			ToggleMenu(0);
		}

		public void OpenURL(string url) // Button
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
