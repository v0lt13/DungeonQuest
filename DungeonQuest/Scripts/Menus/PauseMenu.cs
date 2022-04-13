using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Menus
{
	public class PauseMenu : MonoBehaviour
	{
		[Header("Menu Config:")]
		[SerializeField] private GameObject pauseMenuHolder;
		[SerializeField] private GameObject[] menus;

		[Header("Stats config:")]
		[SerializeField] private Text playerHealthText;
		[SerializeField] private Text playerArmorText;
		[SerializeField] private Text playerXPText;
		[SerializeField] private Text killCountText;
		[SerializeField] private Text secretCountText;

		private int currentMenu;
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
				if (currentMenu == 0)
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
				else
				{
					ToggleMenu(0);
				}
			}

			pauseMenuHolder.SetActive(isGamePaused);
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

		public void ToggleMenu(int menuIndex) // Called by Button
		{
			currentMenu = menuIndex;

			foreach (var menu in menus)
			{
				menu.SetActive(false);
			}

			menus[menuIndex].SetActive(true);
		}

		private void SetPlayerStats()
		{
			var playerManager = gameManager.playerManager;
			var playerLeveling = gameManager.playerManager.playerLeveling;

			playerHealthText.text = "HP: " + playerManager.playerHealth.ToString() + "/" + playerManager.defaultPlayerHealth.ToString();
			playerArmorText.text = "AP: " + playerManager.playerArmor.ToString() + "/" + playerManager.defaultPlayerArmor.ToString();
			playerXPText.text = playerLeveling.IsPlayerMaxLevel ? "" : playerLeveling.PlayerXP.ToString() + "\n━━━━━\n" + playerLeveling.nextLevelXP.ToString();

			killCountText.text = "Kills: " + gameManager.KillCount.ToString() + "/" + gameManager.TotalKillCount.ToString();
			secretCountText.text = "Secrets: " + gameManager.SecretCount.ToString() + "/" + gameManager.TotalSecretCount.ToString();
		}
	}
}
