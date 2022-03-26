using UnityEngine;

namespace DungeonQuest
{
	public class LoadingScreen : MonoBehaviour
	{
		public static string SCENE_NAME;

		void Start()
		{
			GameManager.EnableCursor(true);
			Application.LoadLevel(SCENE_NAME);
		}
	}
}
