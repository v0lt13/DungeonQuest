using UnityEngine;
using UnityEngine.UI;
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
			
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ToogleGamePause();
			}

			Time.timeScale = IS_GAME_PAUSED ? 0f : 1f;
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
			Application.LoadLevel("mainScene");
		}

		public void QuitGame()
		{
			IS_GAME_PAUSED = false;

			GameManager.INSTANCE.EnableCursor(true);
			Application.LoadLevel("MainMenu");
		}
	}
}
