using UnityEngine;
using System.Collections;

namespace DungeonQuest.Player
{
	public class PlayerAttack : MonoBehaviour
	{
		private PlayerManager playerManager;
		private Quaternion swipeRotation;
		private Vector2 swipeDirection;

		[Header("Attack Config:")]
		public int damage;
		[SerializeField] private float defaultTimeBetweenAttacks;
		[Space(10f)]
		[SerializeField] private GameObject swipePrefab;

		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
	    [SerializeField] private AudioClip swipeSFX;

		public bool IsAttacking { get; private set; }
		private float TimeBetweenAttacks { get; set; }

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			playerManager = GetComponent<PlayerManager>();

			TimeBetweenAttacks = defaultTimeBetweenAttacks;
		}
		
		void Update()
		{
			if (TimeBetweenAttacks <= 0)
			{
				TimeBetweenAttacks = 0;

				if (GameManager.INSTANCE.CurrentGameState == GameManager.GameState.Paused) return;

				if (Input.GetButtonDown("Attack") && !playerManager.invisible)
				{
					TimeBetweenAttacks = defaultTimeBetweenAttacks;
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
			Instantiate(swipePrefab, swipeDirection, swipeRotation);

			yield return new WaitForSeconds(TimeBetweenAttacks);

			IsAttacking = false;
		}
	}
}
