using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest
{
	public class FramerateCounter : MonoBehaviour
	{
		public static bool SHOW_FPS;

		private const float REFRESH_TIME = 0.1f;

		private int frameCounter;
		private float timeCounter;

		[SerializeField] private Text framerateText;

		void Update()
		{
			framerateText.enabled = SHOW_FPS;

			if (timeCounter < REFRESH_TIME)
			{
				timeCounter += Time.deltaTime;
				frameCounter++;
			}
			else
			{
				var lastFramerate = frameCounter / timeCounter;
				frameCounter = 0;
				timeCounter = 0.0f;
				framerateText.text = "FPS: " + lastFramerate.ToString("n1");
			}
		}
	}
}
