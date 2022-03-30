using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Menus
{
	public class EndMenu : MonoBehaviour
	{
		[SerializeField] private Text killsCountText;
		[SerializeField] private Text secretCountText;
		[SerializeField] private Text completionTimeText;

		void Start()
		{
			var gameManager = GameManager.INSTANCE;

			killsCountText.text = "Kills: " + gameManager.KillCount.ToString() + "/" + gameManager.TotalKillCount.ToString();
			secretCountText.text = "Secrets: " + gameManager.SecretCount.ToString() + "/" + gameManager.TotalSecretCount.ToString();
			completionTimeText.text = "Time: " + gameManager.CompletionTime.ToString("n2");
		}

		public void Continue()
		{
			GameManager.LoadScene("MainMenu");
		}
	}
}
