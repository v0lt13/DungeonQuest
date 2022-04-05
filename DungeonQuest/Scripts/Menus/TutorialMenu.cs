using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Menus
{
	public class TutorialMenu : MonoBehaviour
	{
		[SerializeField] private GameObject[] tutorialPages;
		[SerializeField] private Text pageText;

		private int currentPage;

		void Update()
		{
			pageText.text = (currentPage + 1).ToString() + "/" + tutorialPages.Length.ToString();
		}

		public void NextPage() // Button
		{
			if (currentPage >= tutorialPages.Length - 1)
			{
				currentPage = 0;
			}
			else
			{
				currentPage++;
			}

			TogglePage(currentPage);
		}

		public void PreviousPage() // Button
		{
			if (currentPage <= 0)
			{
				currentPage = tutorialPages.Length - 1;
			}
			else
			{
				currentPage--;
			}

			TogglePage(currentPage);
		}

		private void TogglePage(int menuIndex)
		{
			foreach (var menu in tutorialPages)
			{
				menu.SetActive(false);
			}

			tutorialPages[menuIndex].SetActive(true);
		}

	}
}
