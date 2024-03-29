﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DungeonQuest.UI.Menus
{
	public class EndMenu : MonoBehaviour
	{
		[Header("Stats Config:")]
		[SerializeField] private Text killCountText;
		[SerializeField] private Text secretCountText;
		[SerializeField] private Text completionTimeText;

		private int completionTimeSeconds;
		private int completionTimeMinutes;

		void Start()
		{
			var gameManager = GameManager.INSTANCE;

			completionTimeSeconds = (int)gameManager.CompletionTime;

			// Convert seconds to minutes
			while (completionTimeSeconds >= 60)
			{
				completionTimeMinutes++;
				completionTimeSeconds -= 60;
			}

			killCountText.text = "Kills: " + gameManager.killCount.ToString() + "/" + gameManager.totalKillCount.ToString();
			secretCountText.text = "Secrets: " + gameManager.secretCount.ToString() + "/" + gameManager.totalSecretCount.ToString();
			completionTimeText.text = "Time: " + completionTimeMinutes + "m " + completionTimeSeconds + "s";
		}

		public void Continue() // Called by Button
		{
			GameManager.LoadScene("Lobby");
		}

		public void LoadIntermission(string intermissionNumber) // Called by Button
		{
			SceneManager.LoadScene("Intermission0" + intermissionNumber);
		}

	}
}
