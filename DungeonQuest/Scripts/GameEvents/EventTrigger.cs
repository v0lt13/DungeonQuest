using UnityEngine;

namespace DungeonQuest.GameEvents
{
	public class EventTrigger : MonoBehaviour
	{
		[SerializeField] private VoidEvent gameEvent;

		private Collider2D playerCollider;

		void Awake()
		{
			playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (playerCollider == collider)
			{
				gameEvent.Invoke();
			}
		}
	}
}
