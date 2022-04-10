using UnityEngine;

namespace DungeonQuest
{
	public class LoadingScreen : MonoBehaviour
	{
		public static int SCENE_INDEX = -1;
		public static string SCENE_NAME = "";

		void Start()
		{
			GameManager.EnableCursor(true);

			if (SCENE_NAME != string.Empty)
			{
				Application.LoadLevel(SCENE_NAME);
				SCENE_NAME = string.Empty;
			}
			else if (SCENE_INDEX != -1)
			{
				Application.LoadLevel(SCENE_INDEX);
				SCENE_INDEX = -1;
			}
			else
			{
				GetComponentInChildren<UnityEngine.UI.Text>().text = "Couldn't load scene";
				Invoke("LoadMenu", 2f);
			}
		}

		private void LoadMenu()
		{
			Application.LoadLevel("MainMenu");
		}
	}
}
