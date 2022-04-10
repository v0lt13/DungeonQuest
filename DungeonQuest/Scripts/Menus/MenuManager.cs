using UnityEngine;

namespace DungeonQuest.Menus
{
	public class MenuManager : MonoBehaviour
	{
		[SerializeField] private GameObject[] menus;

		private int currentMenu;

		void Awake()
		{
			var audioSources = GetComponents<AudioSource>();

			foreach (var audioSource in audioSources)
			{
				audioSource.ignoreListenerPause = true;
			}
		}

		void Update()
		{
			if (Input.GetButtonDown("Back") && currentMenu != 0)
			{
				ToggleMenu(0);
			}
		}

		public void Play() // Called by Button
		{
			GameManager.LoadScene("Level0");
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

		public void Exit() // Called by Button
		{
			Application.Quit();
		}

		public void OpenURL(string url) // Called by Button
		{
			Application.OpenURL(url);
		}
	}
}
