using UnityEngine;
using DungeonQuest.Player;
using DungeonQuest.Enemy;

namespace DungeonQuest
{
	public class GameManager : MonoBehaviour
	{
		public PlayerManager playerManager;
		public EnemyManager enemyManager;

		public static GameManager INSTANCE { get; private set; }

		void Awake()
		{
			#region SINGLETON_PATTERN
			if (INSTANCE != null)
				Destroy(gameObject);
			else
				INSTANCE = this;
			#endregion

			EnableCursor(false);
		}

		public void EnableCursor(bool toogle)
		{
			Screen.lockCursor = !toogle;
			Screen.showCursor = toogle;
		}
	}
}
