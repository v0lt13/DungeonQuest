using UnityEngine;
using System.Collections;

namespace DungeonQuest
{
	public class SecretRoom : MonoBehaviour
	{
		[SerializeField] private GameObject secretRoom;
		[SerializeField] private GameObject secretRoomText;

		private Collider2D playerCollider;
		private AudioSource audioSource;

		void Awake()
		{
			playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
			audioSource = GetComponent<AudioSource>();			
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider == playerCollider)
			{
				audioSource.Play();
				StartCoroutine(ToogleText());
				secretRoom.SetActive(true);

				GetComponent<BoxCollider2D>().enabled = false;
				GetComponent<SpriteRenderer>().enabled = false;
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
