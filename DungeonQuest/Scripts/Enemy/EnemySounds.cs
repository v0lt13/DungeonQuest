using UnityEngine;

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
		private AudioSource audioSource;

		void Awake() 
		{
			audioSource = GetComponent<AudioSource>();
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
				audioSource.PlayOneShot(enemySounds[Random.Range(0, enemySounds.Length)]); // Play a random SFX from the array
				
				timeBetwenSounds = Random.Range(minTimeBetwenSounds, maxTimeBetwenSounds);
			}
		}
	}
}
