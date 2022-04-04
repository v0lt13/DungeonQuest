﻿using UnityEngine;
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
		
		private PlayerManager playerManager;
		private SpriteRenderer spriteRenderer;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		void OnTriggerStay2D(Collider2D collider)
		{
			if (!corroutineActivated) StartCoroutine(TriggerTrap());

			if (!isTrapActive) return;

			if (playerManager.collider2D == collider)
			{
				playerManager.DamagePlayer(int.MaxValue);
			}
			else if (collider.CompareTag("Enemy"))
			{
				collider.GetComponent<EnemyManager>().DamageEnemy(int.MaxValue);
			}
		}

		private void ActivateTrap()
		{
			spriteRenderer.sprite = sprites[1];
			audio.PlayOneShot(audioClips[1]);

			isTrapActive = true;
		}

		private void DeactivateTrap()
		{
			spriteRenderer.sprite = sprites[0];
			audio.PlayOneShot(audioClips[0]);

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
