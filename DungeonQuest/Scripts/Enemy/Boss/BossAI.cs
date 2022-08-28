using UnityEngine;
using DungeonQuest.Grid;
using System.Collections;
using System.Collections.Generic;
using DungeonQuest.Enemy.Boss.Special;

namespace DungeonQuest.Enemy.Boss
{
	public class BossAI : MonoBehaviour
	{
		public enum AIstate
		{
			Idle,
			Chase,
			Attack,
			Special
		}

		[Header("AI Config:")]
		public int damage;
		[SerializeField] private float defaultTimeBetweenAttacks;
		[SerializeField] private float defaultTimeBetweenSpecials;
		[SerializeField] private float bossSpeed;
		[Space]
		[SerializeField] private bool showPath;
		[Space]
		[SerializeField] private SpecialAbility specialAbility;
		[SerializeField] private AudioClip attackSFX;
		[SerializeField] private AudioClip specialSFX;

		[HideInInspector] public AIstate state;
		[HideInInspector] public float timeBetweenSpecials;
		[HideInInspector] public List<PathNode> path;

		private float timeBetweenAttacks;

		private BossManager bossManager;
		private GridGenerator grid;

		void Awake()
		{
			grid = GameObject.Find("GameManager").GetComponent<GridGenerator>();
			bossManager = GetComponent<BossManager>();

			timeBetweenSpecials = defaultTimeBetweenSpecials;
		}

		void Update()
		{
			if (bossManager.playerManager.isDead || !bossManager.IsAwake) return;

			if (timeBetweenSpecials > 0f)
			{
				timeBetweenSpecials -= Time.deltaTime;
			}

			switch (state)
			{
				default:
				case AIstate.Idle:
					Idle();
					break;

				case AIstate.Chase:
					Chase();
					break;

				case AIstate.Attack:
					Attack();
					break;

				case AIstate.Special:
					Idle();
					break;
			}
		}

		private void Idle()
		{
			timeBetweenAttacks = defaultTimeBetweenAttacks;
		}

		private void Chase()
		{
			if (GameManager.INSTANCE.CurrentGameState == GameManager.GameState.Paused) return;

			timeBetweenAttacks = defaultTimeBetweenAttacks;
			FindPathToPlayer(bossManager.playerManager.transform.position, out path);

			if (path != null)
			{
				try
				{
					transform.position = Vector2.MoveTowards(transform.position, new Vector2(path[1].x, path[1].y) * 10f + Vector2.one * 5f, bossSpeed * Time.deltaTime);
				}
				catch (System.ArgumentOutOfRangeException)
				{
					transform.position = Vector2.MoveTowards(transform.position, bossManager.playerManager.transform.position, bossSpeed * Time.deltaTime);
				}
			}
			else
			{
				state = AIstate.Idle;
			}
		}

		private void Attack()
		{
			if (timeBetweenAttacks <= 0f)
			{
				timeBetweenAttacks = defaultTimeBetweenAttacks;
			}
			else
			{
				timeBetweenAttacks -= Time.deltaTime;
			}
		}

		private void FindPathToPlayer(Vector2 targetPosition, out List<PathNode> path)
		{
			int startX, startY, endX, endY;

			grid.pathfinding.GetGrid.GetXY(transform.position, out startX, out startY);
			grid.pathfinding.GetGrid.GetXY(targetPosition, out endX, out endY);

			path = grid.pathfinding.FindPath(startX, startY, endX, endY);

			if (path != null && showPath)
			{
				for (int i = 0; i < path.Count - 1; i++)
				{
					Debug.DrawLine(new Vector2(path[i].x, path[i].y) * 10f + Vector2.one * 5f, new Vector2(path[i + 1].x, path[i + 1].y) * 10f + Vector2.one * 5f, Color.green);
				}
			}
		}

		private void HitPlayer() // Called by Animation event
		{
			bossManager.audioSource.pitch = 1f;
			bossManager.audioSource.PlayOneShot(attackSFX);
			bossManager.playerManager.DamagePlayer(damage);
		}

		private IEnumerator ActivateSpecial(float duration) // Called by Animation event
		{
			specialAbility.Special();

			bossManager.audioSource.pitch = 1f;
			bossManager.audioSource.PlayOneShot(specialSFX);

			yield return new WaitForSeconds(duration);

			timeBetweenSpecials = defaultTimeBetweenSpecials;
		}
	}
}
