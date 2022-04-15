using UnityEngine;

namespace DungeonQuest.Menus
{
	public class LevelSelectMenu : MonoBehaviour
	{
		[SerializeField] private GameObject[] pages;

		private int currentPage;

		void Update()
		{
			if (Input.GetButtonDown("Back"))
			{
				CloseMenu();
			}
		}

		void OnEnable()
		{
			GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
			GameManager.EnableCursor(true);
		}

		public void CloseMenu() // Called by Button
		{
			GameManager.INSTANCE.SetGameState(GameManager.GameState.Running);
			GameManager.EnableCursor(false);
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
