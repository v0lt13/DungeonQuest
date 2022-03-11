﻿using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest
{
	public class FramerateCounter : MonoBehaviour
	{
		private const float REFRESH_TIME = 0.1f;

		private int frameCounter;
		private float timeCounter;

		[SerializeField] private Text framerateText;

		void Update()
		{
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
