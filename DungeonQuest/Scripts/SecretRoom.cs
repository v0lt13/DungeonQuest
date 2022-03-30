using UnityEngine;
using System.Collections;
using DungeonQuest.GameEvents;

namespace DungeonQuest
{
	public class SecretRoom : MonoBehaviour
	{
		[SerializeField] private GameObject secretRoomText;
		[SerializeField] private VoidEvent gameEvent;

		private Collider2D playerCollider;

		void Awake()
		{
			playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();		
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider == playerCollider)
			{
				GameManager.INSTANCE.SecretCount++;

				gameEvent.Invoke();
				StartCoroutine(ToogleText());
				Destroy(gameObject, 3f);
			}
		}

		private IEnumerator ToogleText()
		{
			secretRoomText.SetActive(true);

			yield return new WaitForSeconds(2f);

			secretRoomText.SetActive(false);
		}
	}
}
