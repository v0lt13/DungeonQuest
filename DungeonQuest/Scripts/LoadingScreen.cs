using UnityEngine;

namespace DungeonQuest
{
	public class LoadingScreen : MonoBehaviour
	{
		public static string SCENE_NAME;

		void Start()
		{
			if (SCENE_NAME == null) return;

			GameManager.EnableCursor(true);
			Application.LoadLevel(SCENE_NAME);
		}
	}
}
