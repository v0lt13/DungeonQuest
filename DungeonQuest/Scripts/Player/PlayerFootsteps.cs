using UnityEngine;

namespace DungeonQuest.Player
{
	public class PlayerFootsteps : MonoBehaviour
	{
		private const float DEFAULT_TIME_BETWEEN_FOOTSTEPS = 0.3f;
		private float timeBetwenFootsteps;

		[SerializeField] private AudioClip[] playerFootsteps;

		private PlayerManager playerManager;
		private AudioSource footstepSFX;

		void Awake()
		{
			footstepSFX = GetComponent<AudioSource>();
			playerManager = GetComponentInParent<PlayerManager>();

			timeBetwenFootsteps = DEFAULT_TIME_BETWEEN_FOOTSTEPS;
		}
		
		void Update()
		{
			if (playerManager.IsMoveing)
			{
				timeBetwenFootsteps -= Time.deltaTime;
			}

			if (timeBetwenFootsteps <= 0)
			{
				footstepSFX.PlayOneShot(playerFootsteps[Random.Range(0, playerFootsteps.Length)]); // Playes a random SFX from the array

				timeBetwenFootsteps = DEFAULT_TIME_BETWEEN_FOOTSTEPS;
			}
		}
	}
}
