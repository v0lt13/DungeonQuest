﻿using UnityEngine;

namespace DungeonQuest.Enemy
{
	public class EnemySounds : MonoBehaviour
	{
		[Header ("Sound Config:")]
		[SerializeField] private float minTimeBetwenSounds;
		[SerializeField] private float maxTimeBetwenSounds;
		[Space(10f)]
		[SerializeField] private AudioClip[] enemySounds;

		private float timeBetwenSounds;

		private EnemyManager enemyManager;

		void Awake() 
		{
			enemyManager = GetComponent<EnemyManager>();
			
			timeBetwenSounds = Random.Range(minTimeBetwenSounds, maxTimeBetwenSounds);
		}
		
		void Update()
		{
			if (enemyManager.IsDead) return;

			if (timeBetwenSounds > 0)
			{
				timeBetwenSounds -= Time.deltaTime;
			}

			if (timeBetwenSounds <= 0)
			{
				audio.pitch = Random.Range(0.7f, 1.5f);

				// Play a random SFX from the array
				audio.PlayOneShot(enemySounds[Random.Range(0, enemySounds.Length)]);
				
				timeBetwenSounds = Random.Range(minTimeBetwenSounds, maxTimeBetwenSounds);
			}
		}
	}
}
