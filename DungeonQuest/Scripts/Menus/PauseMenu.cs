using UnityEngine;

namespace DungeonQuest.Menus
{
	public class PauseMenu : MonoBehaviour
	{
		private bool isGamePaused = false;

		[SerializeField] private GameObject pauseMenu;

		void Awake()
		{
			audio.ignoreListenerPause = true;
		}

		void Update()
		{			
			if (Input.GetButtonDown("Back"))
			{
				if (!isGamePaused && GameManager.INSTANCE.CurrentGameState != GameManager.GameState.Paused)
				{
					isGamePaused = true;
					AudioListener.pause = true;

					GameManager.EnableCursor(true);
					GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
				}
				else if (isGamePaused)
				{
					Resume();
				}
			}

			pauseMenu.SetActive(isGamePaused);
		}

		public void Resume() // Button
		{
			isGamePaused = false;
			AudioListener.pause = false;

			GameManager.EnableCursor(false);
			GameManager.INSTANCE.SetGameState(GameManager.GameState.Running);
		}

		public void Restart() // Button
		{
			GameManager.EnableCursor(isGamePaused);
			GameManager.LoadScene(Application.loadedLevelName);
		}

		public void QuitGame() // Button
		{
			GameManager.LoadScene("MainMenu");
		}
	}
}
