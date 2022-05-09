using UnityEngine;
using DungeonQuest.Grid;
using System.Collections;
using DungeonQuest.GameEvents;

namespace DungeonQuest
{
	public class SecretRoom : MonoBehaviour
	{
		[SerializeField] private GameObject secretRoomText;
		[SerializeField] private VoidEvent gameEvent;

		private Collider2D playerCollider;
		private GridGenerator grid;

		void Awake()
		{
			playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
		}

		void Start()
		{
			grid = GameManager.INSTANCE.GetComponent<GridGenerator>();

			GameManager.INSTANCE.totalSecretCount++;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider == playerCollider)
			{
				GameManager.INSTANCE.secretCount++;

				gameEvent.Invoke();
				grid.MarkObstacles("Blockable");

				StartCoroutine(ToggleText());
				Destroy(gameObject, 3f);
			}
		}

		private IEnumerator ToggleText()
		{
			secretRoomText.SetActive(true);

			yield return new WaitForSeconds(2f);

			secretRoomText.SetActive(false);
		}
	}
}
