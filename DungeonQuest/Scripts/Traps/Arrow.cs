using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Traps
{
	public class Arrow : MonoBehaviour
	{
		[SerializeField] private int damage;
		[SerializeField] private float speed;

		private bool itHitObject;

		private Vector3 direction = new Vector3(0f, -1f, 0f);

		private Animation fadeOutAnim;
		private PlayerManager playerManager;

		void Awake()
		{
			fadeOutAnim = GetComponent<Animation>();
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		}

		void Update()
		{
			transform.position += direction * speed * Time.deltaTime;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject || playerManager == null) return;

			if (playerManager.collider2D == collider)
			{
				playerManager.DamagePlayer(damage);
				Destroy(gameObject);
			}
			else if (collider.CompareTag("Enemy"))
			{
				collider.GetComponent<Enemy.EnemyManager>().DamageEnemy(damage);
				Destroy(gameObject);
			}
			else if (collider.CompareTag("Blockable"))
			{
				itHitObject = true;
				direction = Vector2.zero;

				fadeOutAnim.Play();
				Destroy(gameObject, 5f);
			}
		}
	}
}
