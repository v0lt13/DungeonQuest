using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Menus
{
	public class TutorialMenu : MonoBehaviour
	{
		[SerializeField] private GameObject[] tutorialPages;
		[SerializeField] private Text pageNumberText;

		private int currentPage;

		void Update()
		{
			pageNumberText.text = (currentPage + 1).ToString() + "/" + tutorialPages.Length.ToString();
		}

		public void NextPage() // Called by Button
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

		public void PreviousPage() // Called by Button
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
