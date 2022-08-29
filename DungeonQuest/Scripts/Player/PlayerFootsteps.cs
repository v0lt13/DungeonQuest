using UnityEngine;

namespace DungeonQuest.Player
{
	public class PlayerFootsteps : MonoBehaviour
	{
		[Header("Audio Config:")]
		[SerializeField] private AudioClip[] playerFootsteps;

		private const float DEFAULT_TIME_BETWEEN_FOOTSTEPS = 0.3f;
		private float timeBetwenFootsteps;

		private PlayerManager playerManager;

		void Awake()
		{
			playerManager = GetComponentInParent<PlayerManager>();
		}
		
		void Update()
		{
			if (playerManager.playerMovement.IsMoveing)
			{
				timeBetwenFootsteps -= Time.deltaTime;
			}

			if (timeBetwenFootsteps < 0 && !playerManager.isDead)
			{
				playerManager.audioSource3D.pitch = 1f;

				// Plays a random SFX from the array
				playerManager.audioSource3D.PlayOneShot(playerFootsteps[Random.Range(0, playerFootsteps.Length)]);

				timeBetwenFootsteps = DEFAULT_TIME_BETWEEN_FOOTSTEPS;
			}
		}
	}
}
