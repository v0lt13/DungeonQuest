using UnityEngine;
using UnityEngine.UI;

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
		private int completionTimeHours;

		void Start()
		{
			var gameManager = GameManager.INSTANCE;

			completionTimeSeconds = (int)gameManager.CompletionTime;

			// Convert seconds to minutes and hours
			while (completionTimeSeconds >= 60)
			{
				completionTimeMinutes++;
				completionTimeSeconds -= 60;

				while (completionTimeMinutes >= 60)
				{
					completionTimeHours++;
					completionTimeMinutes -= 60;
				}
			}

			killCountText.text = "Kills: " + gameManager.killCount.ToString() + "/" + gameManager.totalKillCount.ToString();
			secretCountText.text = "Secrets: " + gameManager.secretCount.ToString() + "/" + gameManager.totalSecretCount.ToString();
			completionTimeText.text = "Time: " + completionTimeHours + "h " + completionTimeMinutes + "m " + completionTimeSeconds + "s";
		}

		public void Continue() // Called by Button
		{
			GameManager.LoadScene("Lobby");
		}

		public void LoadIntermission(int sceneIndex) // Called by Button
		{
			Application.LoadLevel(sceneIndex);
		}

	}
}
