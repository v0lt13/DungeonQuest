using UnityEngine;

namespace DungeonQuest
{
	public class FogOfWar : MonoBehaviour
	{
		void Awake()
		{
			GetComponent<SpriteRenderer>().color = Camera.main.backgroundColor;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.CompareTag("Player"))
			{
				Destroy(gameObject);
			}
		}
	}
}
