using UnityEngine;

namespace DungeonQuest.Menus
{
	public class OptionsMenu : MonoBehaviour
	{
		[SerializeField] private GameObject mainMenu;

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GoBack();
			}
		}

		public void GoBack()
		{
			mainMenu.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
