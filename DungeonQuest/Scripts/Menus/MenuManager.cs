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

		public void Play()
		{
			GameManager.LoadScene("Level0");
		}

		public void Options()
		{
			ToggleMenu(1);
		}

		public void TutorialMenu()
		{
			ToggleMenu(2);
		}

		public void Credits()
		{
			ToggleMenu(3);
		}

		public void Exit()
		{
			Application.Quit();
		}

		public void GoBackToMainMenu()
		{
			ToggleMenu(0);
		}

		public void OpenURL(string url)
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
