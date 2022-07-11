using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.UI.Menus
{
	public class LevelSelectMenu : MonoBehaviour
	{
		[SerializeField] private GameObject[] pages;
		[SerializeField] private GameObject[] secretLevelButtons;
		[SerializeField] private Button[] levelSelectButtons;

		private int currentPage;

		void Awake()
		{
			// Enable all buttons for the unlocked levels
			for (int i = 1; i < levelSelectButtons.Length; i++)
			{
				if (i < GameManager.INSTANCE.LevelReached) levelSelectButtons[i].interactable = true;
			}

			for (int i = 0; i < secretLevelButtons.Length; i++)
			{
				secretLevelButtons[i].SetActive(GameManager.INSTANCE.secretLevelsUnlocked[i + 1]);
			}
		}

		void Update()
		{
			if (Input.GetButtonDown("Back")) CloseMenu();
		}

		void OnEnable()
		{
			GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
		}

		public void CloseMenu() // Called by Button
		{
			GameManager.INSTANCE.SetGameState(GameManager.GameState.Running);
			gameObject.SetActive(false);
		}

		public void NextPage() // Called by Button
		{
			if (currentPage >= pages.Length - 1)
			{
				currentPage = 0;
			}
			else
			{
				currentPage++;
			}

			TogglePage(currentPage);
		}

		public void PreviousPage() // Called by Button
		{
			if (currentPage <= 0)
			{
				currentPage = pages.Length - 1;
			}
			else
			{
				currentPage--;
			}

			TogglePage(currentPage);
		}

		public void TogglePage(int tabIndex) // Called by Button
		{
			foreach (var menu in pages)
			{
				menu.SetActive(false);
			}

			pages[tabIndex].SetActive(true);
		}
	}
}
