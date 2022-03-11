using UnityEngine;

namespace DungeonQuest.Menus
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private GameObject optionsMenu;

		public void Play()
		{
			Application.LoadLevel("mainScene");
		}

		public void Options()
		{
			optionsMenu.SetActive(true);
			gameObject.SetActive(false);
		}

		public void Exit()
		{
			Application.Quit();
		}
	}
}
