using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Menus
{
	public class PauseMenu : MonoBehaviour
	{
		[SerializeField] private GameObject pauseMenu;

		[Header("Stats config:")]
		[SerializeField] private Text playerHealthText;
		[SerializeField] private Text playerArmorText;
		[SerializeField] private Text playerXPText;
		[SerializeField] private Text killCountText;
		[SerializeField] private Text secretCountText;
		
		private bool isGamePaused = false;

		private GameManager gameManager;

		void Awake()
		{
			audio.ignoreListenerPause = true;
		}

		void Start()
		{
			gameManager = GameManager.INSTANCE;
		}

		void Update()
		{
			if (Input.GetButtonDown("Back"))
			{
				if (!isGamePaused && gameManager.CurrentGameState != GameManager.GameState.Paused)
				{
					isGamePaused = true;
					AudioListener.pause = true;

					GameManager.EnableCursor(true);
					gameManager.SetGameState(GameManager.GameState.Paused);
				}
				else if (isGamePaused)
				{
					Resume();
				}
			}

			pauseMenu.SetActive(isGamePaused);
			SetPlayerStats();
		}

		public void Resume() // Called by Button
		{
			isGamePaused = false;
			AudioListener.pause = false;

			GameManager.EnableCursor(false);
			gameManager.SetGameState(GameManager.GameState.Running);
		}

		public void Restart() // Called by Button
		{
			GameManager.EnableCursor(isGamePaused);
			GameManager.LoadScene(Application.loadedLevelName);
		}

		public void QuitGame() // Called by Button
		{
			GameManager.LoadScene("MainMenu");
		}

		private void SetPlayerStats()
		{
			var playerManager = gameManager.playerManager;
			var playerLeveling = gameManager.playerManager.playerLeveling;

			playerHealthText.text = "HP: " + playerManager.PlayerHealth.ToString() + "/" + playerManager.GetDefaultPlayerHealth.ToString();
			playerArmorText.text = "AP: " + playerManager.PlayerArmor.ToString() + "/" + playerManager.GetDefaultPlayerArmor.ToString();
			playerXPText.text = playerLeveling.IsPlayerMaxLevel ? "" : playerLeveling.PlayerXP.ToString() + "\n━━━━━\n" + playerLeveling.GetPlayerNextLevelXP.ToString();

			killCountText.text = "Kills: " + gameManager.KillCount.ToString() + "/" + gameManager.TotalKillCount.ToString();
			secretCountText.text = "Secrets: " + gameManager.SecretCount.ToString() + "/" + gameManager.TotalSecretCount.ToString();
		}
	}
}
