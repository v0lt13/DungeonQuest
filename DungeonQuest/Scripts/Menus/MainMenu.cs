using UnityEngine;

namespace DungeonQuest.Menus
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private GameObject optionsMenu;

		public void Play()
		{
			LoadingScreen.SCENE_NAME = "mainScene";
			Application.LoadLevel("LoadingScreen");
		}

		public void Options()
		{
			optionsMenu.SetActive(true);
			gameObject.SetActive(false);
		}

		public void Exit()
		{
			if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();
		}
	}
}
