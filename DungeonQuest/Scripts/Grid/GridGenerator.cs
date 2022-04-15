using UnityEngine;

namespace DungeonQuest.Grid
{
	public class GridGenerator : MonoBehaviour
	{
		[SerializeField] private int gridX, gridY = 0;
		[SerializeField] private bool drawGrid;
		
		public GridPathfinding pathfinding;

		void Awake() 
		{
			pathfinding = new GridPathfinding(gridX, gridY);

			MarkObstacles("Blockable");
			MarkObstacles("Breakeble");
		}

		[ExecuteInEditMode]
		void OnDrawGizmos()
		{
			var grid = new GridPathfinding(gridX, gridY);

			grid.GetGrid.DrawGrid(drawGrid);
		}

		public void MarkObstacle(Vector2 position, bool walkable)
		{
			int x, y;
			pathfinding.GetGrid.GetXY(position, out x, out y);
			pathfinding.GetNode(x, y).SetIsWalkable(walkable);
		}

		private void MarkObstacles(string obstacleTag)
		{
			var obstacles = GameObject.FindGameObjectsWithTag(obstacleTag);

			foreach (var obstacle in obstacles)
			{
				int x, y;
				pathfinding.GetGrid.GetXY(obstacle.transform.position, out x, out y);
				pathfinding.GetNode(x, y).SetIsWalkable(false);
			}
		}
	}
}
