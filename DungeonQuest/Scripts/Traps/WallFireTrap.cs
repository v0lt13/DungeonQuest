using UnityEngine;
using System.Collections;
using DungeonQuest.Player;

namespace DungeonQuest.Traps
{
	public class WallFireTrap : MonoBehaviour 
	{
		[Header("Trap Config")]
		[SerializeField] private float rayDistance;
		[SerializeField] private bool drawRay;
		[Space(10f)]
		[SerializeField] private GameObject fire;

		private bool hasFireStarted;

		private PlayerManager playerManager;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		}

		void FixedUpdate()
		{
			var mask = 1 << 10;

			RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(-Vector2.up), rayDistance, mask);

			if (hit.collider != null)
			{
				bool itCollided = (hit.collider == playerManager.collider2D && !playerManager.Invisible) || hit.collider.CompareTag("Enemy");

				if (itCollided && !hasFireStarted)
				{
					StartCoroutine(StartFire());
				}
			}
			else if (hasFireStarted)
			{
				StartCoroutine(StopFire());
			}
		}

		[ExecuteInEditMode]
		void OnDrawGizmos()
		{
			if (drawRay) Debug.DrawRay(transform.position, transform.TransformDirection(-Vector2.up) * rayDistance, Color.green);
		}

		private IEnumerator StartFire()
		{
			hasFireStarted = true;

			yield return new WaitForSeconds(0.2f);

			fire.SetActive(true);
			audio.Play();
		}

		private IEnumerator StopFire()
		{
			yield return new WaitForSeconds(1f);

			fire.SetActive(false);
			audio.Stop();

			hasFireStarted = false;
		}
	}
}
