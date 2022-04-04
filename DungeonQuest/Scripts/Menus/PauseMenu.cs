using UnityEngine;
using DungeonQuest.Shop;
using DungeonQuest.Debuging;

namespace DungeonQuest.Menus
{
	public class PauseMenu : MonoBehaviour
	{
		public static bool IS_GAME_PAUSED = false;

		[SerializeField] private GameObject pauseMenu;

		void Update()
		{
			if (DebugConsole.IS_CONSOLE_ON || ShopMenu.IS_SHOP_OPEN || GameManager.INSTANCE.LevelEnded) return;
			
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

			GameManager.EnableCursor(IS_GAME_PAUSED);
		}

		public void Restart()
		{
			IS_GAME_PAUSED = false;

			GameManager.EnableCursor(IS_GAME_PAUSED);
			GameManager.LoadScene(Application.loadedLevelName);
		}

		public void QuitGame()
		{
			IS_GAME_PAUSED = false;

			GameManager.LoadScene("MainMenu");
		}
	}
}
