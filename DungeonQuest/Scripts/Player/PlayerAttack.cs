using UnityEngine;
using DungeonQuest.Shop;
using DungeonQuest.Menus;
using System.Collections;
using DungeonQuest.Debuging;

namespace DungeonQuest.Player
{
	public class PlayerAttack : MonoBehaviour
	{
		private PlayerManager playerManager;
		private Quaternion swipeRotation;
		private Vector2 swipeDirection;

		[Header("Attack Config:")]
		[SerializeField] private int damage;
		[SerializeField] private float timeBetweenAttacks;
		[Space(10f)]
		[SerializeField] private GameObject swipe;

		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
	    [SerializeField] private AudioClip swipeSFX;

		public int GetDamage { get { return damage; } }
		public bool IsAttacking { get; private set; }

		private float TimeBetweenAttacks { get; set; }

		void Awake() 
		{
			audioSource = GetComponent<AudioSource>();
			playerManager = GetComponent<PlayerManager>();

			TimeBetweenAttacks = timeBetweenAttacks;
		}
		
		void Update()
		{
			if (TimeBetweenAttacks <= 0)
			{
				TimeBetweenAttacks = 0;

				// Update to new pause system
				if (PauseMenu.IS_GAME_PAUSED || DebugConsole.IS_CONSOLE_ON || ShopMenu.IS_SHOP_OPEN) return;

				if (Input.GetButtonDown("Attack"))
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

		public void IncreaseDamage(int amount)
		{
			damage += amount;
		}

		private void SetSwipeTransform()
		{
			switch (playerManager.playerMovement.faceingDir)
			{
				default:
				case PlayerMovement.FaceingDirection.DOWN:
					swipeDirection = new Vector2(transform.position.x, transform.position.y - 5);
					swipeRotation = Quaternion.Euler(0, 0, -90);
					break;

				case PlayerMovement.FaceingDirection.UP:
					swipeDirection = new Vector2(transform.position.x, transform.position.y + 5);
					swipeRotation = Quaternion.Euler(0, 0, 90);
					break;

				case PlayerMovement.FaceingDirection.LEFT:
					swipeDirection = new Vector2(transform.position.x - 5, transform.position.y);
					swipeRotation = Quaternion.Euler(0, 0, 180);
					break;

				case PlayerMovement.FaceingDirection.RIGHT:
					swipeDirection = new Vector2(transform.position.x + 5, transform.position.y);
					swipeRotation = Quaternion.Euler(0, 0, 0);
					break;
			}
		}

		private IEnumerator Attack()
		{
			IsAttacking = true;

			audioSource.PlayOneShot(swipeSFX);

			SetSwipeTransform();
			Instantiate(swipe, swipeDirection, swipeRotation);

			yield return new WaitForSeconds(TimeBetweenAttacks);

			IsAttacking = false;
		}
	}
}
