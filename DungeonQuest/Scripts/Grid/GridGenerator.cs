using UnityEngine;

namespace DungeonQuest.Grid
{
	public class GridGenerator : MonoBehaviour
	{
		[SerializeField] private int gridX, gridY = 0;
		[SerializeField] private bool drawGrid;
		
		public GridPathfinding pathfinding;

		public int GridX { get { return gridX; } }
		public int GridY { get { return gridY; } }

		void Awake() 
		{
			pathfinding = new GridPathfinding(gridX, gridY);
		}

		void Start()
		{
			MarkObstructions();
		}

		[ExecuteInEditMode]
		void OnDrawGizmos()
		{
			GridPathfinding grid = new GridPathfinding(gridX, gridY);
			grid.GetGrid.DrawGrid(drawGrid);
		}

		private void MarkObstructions()
		{
			GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

			foreach (var wall in walls)
			{
				int x, y;
				pathfinding.GetGrid.GetXY(wall.transform.position, out x, out y);
				pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
			}
		}
	}
}
