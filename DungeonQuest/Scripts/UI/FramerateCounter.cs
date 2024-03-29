﻿using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.UI
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
			if (!GameManager.INSTANCE.playerManager.playerMap.isMapOn)
			{
				framerateText.enabled = SHOW_FPS;
			}
			else
			{
				framerateText.enabled = false;
			}

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
				framerateText.text = "FPS: " + lastFramerate.ToString("n0");
			}
		}
	}
}
