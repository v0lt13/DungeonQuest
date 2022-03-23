using UnityEngine;
using System.Collections;
using DungeonQuest.Enemy;
using DungeonQuest.Player;

namespace DungeonQuest.Traps
{
	public class SpikeTrap : MonoBehaviour, ITrap
	{
		[Header("Trap Config")]
		[SerializeField] private AudioClip[] audioClips;
		[SerializeField] private Sprite[] sprites;

		private bool isTrapActive;
		private bool corroutineActivated;
		
		private GameObject player;
		private BoxCollider2D playerCollider;
		private PlayerManager playerManager;
		private SpriteRenderer spriteRenderer;
		private AudioSource audioSource;

		void Awake()
		{
			player = GameObject.Find("Player");

			playerManager = player.GetComponent<PlayerManager>();
			playerCollider = player.GetComponent<BoxCollider2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			audioSource = GetComponent<AudioSource>();
		}

		void OnTriggerStay2D(Collider2D collider)
		{
			if (!corroutineActivated) StartCoroutine(TriggerTrap());

			if (!isTrapActive) return;

			if (collider == playerCollider)
			{
				playerManager.DamagePlayer(int.MaxValue);
			}

			if (collider.CompareTag("Enemy"))
			{
				collider.GetComponent<EnemyManager>().DamageEnemy(int.MaxValue);
			}
		}

		private void ActivateTrap()
		{
			spriteRenderer.sprite = sprites[1];
			audioSource.PlayOneShot(audioClips[1]);

			isTrapActive = true;
		}

		private void DeactivateTrap()
		{
			spriteRenderer.sprite = sprites[0];
			audioSource.PlayOneShot(audioClips[0]);

			isTrapActive = false;
		}

		public IEnumerator TriggerTrap()
		{
			corroutineActivated = true;

			yield return new WaitForSeconds(0.5f);

			ActivateTrap();

			yield return new WaitForSeconds(1f);

			DeactivateTrap();

			corroutineActivated = false;
		}
	}
}
