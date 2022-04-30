using UnityEngine;
using System.Collections;
using DungeonQuest.Player;

namespace DungeonQuest.Traps
{
	public class ArrowTrap : MonoBehaviour, ITrap
	{
		[Header("Trap Config")]
		[SerializeField] private float timeBetweenShots;
		[SerializeField] private float rayDistance;
		[SerializeField] private bool drawRay;
		[Space(10f)]
		[SerializeField] private GameObject arrowPrefab;

		private bool corroutineActivated;

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
				bool itCollided = (hit.collider == playerManager.collider2D && !playerManager.invisible) || hit.collider.CompareTag("Enemy");

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

			ShootArrow();

			corroutineActivated = false;
		}

		private void ShootArrow()
		{
			Instantiate(arrowPrefab, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.Euler(0, 0, -180));

			audio.Play();
		}
	}
}
