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
			bool canTriggerTrap = (collider == playerManager.collider2D && !playerManager.invisible) || collider.CompareTag("Enemy");

			if (!corroutineActivated && canTriggerTrap) StartCoroutine(TriggerTrap());

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

		public IEnumerator TriggerTrap()
		{
			corroutineActivated = true;

			yield return new WaitForSeconds(0.4f);

			ActivateTrap();

			yield return new WaitForSeconds(0.8f);

			DeactivateTrap();

			corroutineActivated = false;
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
	}
}
