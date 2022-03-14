using UnityEngine;
using DungeonQuest.Menus;
using System.Collections;
using DungeonQuest.DebugConsole;

namespace DungeonQuest.Player
{
	public class PlayerAttack : MonoBehaviour
	{
		private PlayerManager playerManager;
		private Vector2 swipeDirection;
		private Quaternion swipeRotation;
		private AudioSource swipeSFX;

		[Header("Attack Config:")]
		public int damage;
		[SerializeField] private float timeBetweenAttacks;
		[SerializeField] private GameObject swipe;

		public float TimeBetweenAttacks { private get; set; }
		public bool IsAttacking { get; private set; }

		void Awake() 
		{
			swipeSFX = GetComponent<AudioSource>();
			playerManager = GetComponent<PlayerManager>();

			TimeBetweenAttacks = timeBetweenAttacks;
		}
		
		void Update()
		{
			if (TimeBetweenAttacks <= 0)
			{
				TimeBetweenAttacks = 0;

				if (PauseMenu.IS_GAME_PAUSED || DebugController.IS_CONSOLE_ON) return;

				if (Input.GetMouseButtonDown(0))
				{
					TimeBetweenAttacks = timeBetweenAttacks;
					StartCoroutine(Attack());
				}
			}
			else
			{
				TimeBetweenAttacks -= Time.deltaTime;
			}

		}

		private void SetSwipeTransform()
		{
			if (playerManager.FaceingDirection == new Vector2(1, 0)) // Right
			{
				swipeDirection = new Vector2(transform.position.x + 5, transform.position.y);
				swipeRotation = Quaternion.Euler(0, 0, 0);
			}
			else if (playerManager.FaceingDirection == new Vector2(-1, 0)) // Left
			{
				swipeDirection = new Vector2(transform.position.x - 5, transform.position.y);
				swipeRotation = Quaternion.Euler(0, 0, 180);
			}
			else if (playerManager.FaceingDirection.x >= -1 && playerManager.FaceingDirection.y > 0) // Up
			{
				swipeDirection = new Vector2(transform.position.x, transform.position.y + 5);
				swipeRotation = Quaternion.Euler(0, 0, 90);
			}
			else if (playerManager.FaceingDirection.x >= -1 && playerManager.FaceingDirection.y < 0) // Down
			{
				swipeDirection = new Vector2(transform.position.x, transform.position.y - 5);
				swipeRotation = Quaternion.Euler(0, 0, -90);
			}
			else // Defaults to Down
			{
				swipeDirection = new Vector2(transform.position.x, transform.position.y - 5);
				swipeRotation = Quaternion.Euler(0, 0, -90);
			}
		}

		private IEnumerator Attack()
		{
			IsAttacking = true;

			swipeSFX.PlayOneShot(swipeSFX.clip);

			SetSwipeTransform();
			Instantiate(swipe, swipeDirection, swipeRotation);

			yield return new WaitForSeconds(TimeBetweenAttacks);

			IsAttacking = false;
		}
	}
}
