using UnityEngine;
using DungeonQuest.Shop;
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

		[Header("Attack Config:")]
		[SerializeField] private int damage;
		[SerializeField] private float timeBetweenAttacks;
	    [SerializeField] private AudioSource swipeSFX;
		[SerializeField] private GameObject swipe;

		public int GetDamage { get { return damage; } }
		public bool IsAttacking { get; private set; }

		private float TimeBetweenAttacks { get; set; }

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

				if (PauseMenu.IS_GAME_PAUSED || DebugController.IS_CONSOLE_ON || ShopMenu.IS_SHOP_OPEN) return;

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
			switch (playerManager.faceingDir)
			{
				default:
				case PlayerManager.FaceingDirection.DOWN:
					swipeDirection = new Vector2(transform.position.x, transform.position.y - 5);
					swipeRotation = Quaternion.Euler(0, 0, -90);
					break;

				case PlayerManager.FaceingDirection.UP:
					swipeDirection = new Vector2(transform.position.x, transform.position.y + 5);
					swipeRotation = Quaternion.Euler(0, 0, 90);
					break;

				case PlayerManager.FaceingDirection.LEFT:
					swipeDirection = new Vector2(transform.position.x - 5, transform.position.y);
					swipeRotation = Quaternion.Euler(0, 0, 180);
					break;

				case PlayerManager.FaceingDirection.RIGHT:
					swipeDirection = new Vector2(transform.position.x + 5, transform.position.y);
					swipeRotation = Quaternion.Euler(0, 0, 0);
					break;
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
