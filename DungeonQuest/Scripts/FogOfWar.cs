using UnityEngine;

namespace DungeonQuest
{
	public class FogOfWar : MonoBehaviour
	{
		private Collider2D playerCollider;

		void Awake()
		{
			playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
			GetComponent<SpriteRenderer>().color = Camera.main.backgroundColor;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider == playerCollider)
			{
				Destroy(gameObject);
			}
		}
	}
}
