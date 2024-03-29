﻿using UnityEngine;
using System.Collections;
using DungeonQuest.Player;

namespace DungeonQuest.Traps
{
	public class ProjectileTrap : MonoBehaviour, ITrap
	{
		[Header("Trap Config")]
		[SerializeField] private float timeBetweenShots;
		[SerializeField] private float rayDistance;
		[SerializeField] private bool drawRay;
		[Space]
		[SerializeField] private GameObject projectilePrefab;

		private bool corroutineActivated;

		private PlayerManager playerManager;
		private AudioSource audioSource;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
			audioSource = GetComponent<AudioSource>();
		}

		void FixedUpdate()
		{
			var mask = 1 << 10;

			RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(-Vector2.up), rayDistance, mask);

			if (hit.collider != null)
			{
				bool itCollided = (hit.collider == playerManager.playerCollider && !playerManager.Invisible) || hit.collider.CompareTag("Enemy");

				if (itCollided && !corroutineActivated) StartCoroutine(TriggerTrap());
			}
		}

		[ExecuteInEditMode]
		void OnDrawGizmos()
		{
			if (drawRay) Debug.DrawRay(transform.position, transform.TransformDirection(-Vector2.up) * rayDistance, Color.green);
		}

		public IEnumerator TriggerTrap()
		{
			corroutineActivated = true;

			yield return new WaitForSeconds(timeBetweenShots);

			ShootProjectile();

			corroutineActivated = false;
		}

		private void ShootProjectile()
		{
			Instantiate(projectilePrefab, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.Euler(0, 0, -180));

			audioSource.Play();
		}
	}
}
