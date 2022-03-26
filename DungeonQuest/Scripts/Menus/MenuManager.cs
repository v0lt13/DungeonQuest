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
			LoadingScreen.SCENE_NAME = "mainScene";
			Application.LoadLevel("LoadingScreen");
		}

		public void Options()
		{
			ToogleMenu(1);
		}

		public void Exit()
		{
			Application.Quit();
		}

		public void GoBackToMainMenu()
		{
			ToogleMenu(0);
		}

		private void ToogleMenu(int menuIndex)
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
