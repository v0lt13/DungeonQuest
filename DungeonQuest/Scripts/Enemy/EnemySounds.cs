using UnityEngine;

namespace DungeonQuest.Enemy
{
	public class EnemySounds : MonoBehaviour
	{
		[Header ("Sound Config:")]
		[SerializeField] private float minTimeBetweenSounds;
		[SerializeField] private float maxTimeBetweenSounds;
		[Space]
		[SerializeField] private AudioClip[] enemySounds;

		private float timeBetweenSounds;

		private EnemyManager enemyManager;

		void Awake() 
		{
			enemyManager = GetComponent<EnemyManager>();
			
			timeBetweenSounds = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
		}
		
		void Update()
		{
			if (enemyManager.IsDead) return;

			if (timeBetweenSounds > 0)
			{
				timeBetweenSounds -= Time.deltaTime;
			}

			if (timeBetweenSounds <= 0)
			{
				enemyManager.audioSource.pitch = Random.Range(0.7f, 1.5f);

				// Play a random SFX from the array
				enemyManager.audioSource.PlayOneShot(enemySounds[Random.Range(0, enemySounds.Length)]);
				
				timeBetweenSounds = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
			}
		}
	}
}
