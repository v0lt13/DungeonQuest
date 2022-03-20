using UnityEngine;

namespace DungeonQuest
{
	public class LoadingScreen : MonoBehaviour
	{
		public static string SCENE_NAME;

		void Start()
		{
			Application.LoadLevel(SCENE_NAME);
		}
	}
}
