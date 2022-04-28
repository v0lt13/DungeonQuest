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
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider == playerCollider)
			{
				GameManager.INSTANCE.SecretCount++;

				gameEvent.Invoke();
				grid.MarkObstacles("Blockable");

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
