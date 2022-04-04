using UnityEngine;

namespace DungeonQuest.Player
{
	public class PlayerFootsteps : MonoBehaviour
	{
		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip[] playerFootsteps;

		private const float DEFAULT_TIME_BETWEEN_FOOTSTEPS = 0.3f;
		private float timeBetwenFootsteps;

		private PlayerManager playerManager;

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			playerManager = GetComponentInParent<PlayerManager>();

			timeBetwenFootsteps = DEFAULT_TIME_BETWEEN_FOOTSTEPS;
		}
		
		void Update()
		{
			if (playerManager.playerMovement.IsMoveing)
			{
				timeBetwenFootsteps -= Time.deltaTime;
			}

			if (timeBetwenFootsteps <= 0)
			{
				// Plays a random SFX from the array
				audioSource.PlayOneShot(playerFootsteps[Random.Range(0, playerFootsteps.Length)]);

				timeBetwenFootsteps = DEFAULT_TIME_BETWEEN_FOOTSTEPS;
			}
		}
	}
}
