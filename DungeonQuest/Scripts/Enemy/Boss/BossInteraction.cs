using UnityEngine;
using DungeonQuest.GameEvents;

namespace DungeonQuest.Enemy.Boss
{
	public class BossInteraction : MonoBehaviour
	{
		[SerializeField] private GameObject prompt;
		[SerializeField] private VoidEvent gameEvent;

		private bool canWakeBoss;

		private Collider2D playerCollider;

		void Awake()
		{
			playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
		}

		void Update()
		{
			if (Input.GetButtonDown("Interact") && canWakeBoss)
			{
				gameEvent.Invoke();
			}

			prompt.SetActive(canWakeBoss);
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider == playerCollider) canWakeBoss = true;
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			if (collider == playerCollider) canWakeBoss = false;
		}
	}
}
