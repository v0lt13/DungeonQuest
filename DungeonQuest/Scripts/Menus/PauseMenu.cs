using UnityEngine;
using DungeonQuest.DebugConsole;

namespace DungeonQuest.Menus
{
	public class PauseMenu : MonoBehaviour
	{
		public static bool IS_GAME_PAUSED = false;

		[SerializeField] private GameObject pauseMenu;

		void Update()
		{
			if (DebugController.IS_CONSOLE_ON) return;
			
			if (Input.GetButtonDown("Back"))
			{
				ToogleGamePause();
			}

			Time.timeScale = IS_GAME_PAUSED ? 0f : 1f;
			AudioListener.pause = IS_GAME_PAUSED;
			pauseMenu.SetActive(IS_GAME_PAUSED);
		}

		public void ToogleGamePause()
		{
			IS_GAME_PAUSED = !IS_GAME_PAUSED;

			GameManager.INSTANCE.EnableCursor(IS_GAME_PAUSED);
		}

		public void Restart()
		{
			IS_GAME_PAUSED = false;

			GameManager.INSTANCE.EnableCursor(IS_GAME_PAUSED);
			LoadingScreen.SCENE_NAME = "mainScene";
			Application.LoadLevel("LoadingScreen");
		}

		public void QuitGame()
		{
			IS_GAME_PAUSED = false;

			GameManager.INSTANCE.EnableCursor(true);
			LoadingScreen.SCENE_NAME = "MainMenu";
			Application.LoadLevel("LoadingScreen");
		}
	}
}
