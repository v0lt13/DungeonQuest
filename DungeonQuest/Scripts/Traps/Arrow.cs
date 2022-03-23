using UnityEngine;

namespace DungeonQuest.Traps
{
	public class Arrow : MonoBehaviour
	{
		[SerializeField] private int projectileDamage;
		[SerializeField] private float speed;

		private bool itHitObject;

		private Animation fadeOutAnim;
		private Vector3 direction;
		private Collider2D playerCollider;

		void Awake()
		{
			fadeOutAnim = GetComponent<Animation>();
			playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();

			direction = new Vector3(0f, -1f, 0f);
		}

		void Update()
		{
			transform.position += direction * speed * Time.deltaTime;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject) return;

			if (collider == playerCollider)
			{
				collider.GetComponent<Player.PlayerManager>().DamagePlayer(projectileDamage);
				Destroy(gameObject);
			}
			else if (collider.CompareTag("Enemy"))
			{
				collider.GetComponent<Enemy.EnemyManager>().DamageEnemy(projectileDamage);
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
