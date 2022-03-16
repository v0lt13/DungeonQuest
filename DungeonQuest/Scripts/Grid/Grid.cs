using System;
using UnityEngine;

namespace DungeonQuest.Grid
{
	public class Grid<TGridObject>
	{
		public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

		public class OnGridObjectChangedEventArgs : EventArgs
		{
			public int x, y;
		}

		private int width, height;
		private float cellSize;

		private Vector2 originPosition;
		private TGridObject[,] gridArray;

		public int GetWidth { get { return width; } }
		public int GetHeight { get { return height; } }

		public Grid(int width, int height, float cellSize, Vector2 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
		{
			this.width = width;
			this.height = height;
			this.cellSize = cellSize;
			this.originPosition = originPosition;

			gridArray = new TGridObject[width, height];

			for (int x = 0; x < gridArray.GetLength(0); x++)
			{
				for (int y = 0; y < gridArray.GetLength(1); y++)
				{
					gridArray[x, y] = createGridObject(this, x, y);
				}
			}
		}

		public void GetXY(Vector2 worldPosition, out int x, out int y)
		{
			x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
			y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
		}

		public void TriggerGridObjectChanged(int x, int y)
		{
			if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x , y = y });
		}

		/*public void SetGridObject(Vector2 worldPosition, TGridObject value)
		{
			int x, y;
			GetXY(worldPosition, out x, out y);

			SetGridObject(x, y, value);
		}*/

		/*public void SetGridObject(int x, int y, TGridObject value)
		{
			if (x >= 0 && y >= 0 && x < width && y < height)
			{
				gridArray[x, y] = value;

				if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x , y = y });
			}
		}*/

		/*public TGridObject GetGridObject(Vector2 worldPosition)
		{
			int x, y;
			GetXY(worldPosition, out x, out y);

			return GetGridObject(x, y);
		}*/

		public TGridObject GetGridObject(int x, int y)
		{
			if (x >= 0 && y >= 0 && x < width && y < height)
			{
				return gridArray[x, y];
			}
			else
			{
				return default(TGridObject);
			}
		}

		public void DrawGrid(bool drawGird)
		{
			Gizmos.color = new Color(135f, 135f, 135f, 0.3f);

			if (drawGird)
			{
				for (int x = 0; x < gridArray.GetLength(0); x++)
				{
					for (int y = 0; y < gridArray.GetLength(1); y++)
					{
						Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1));
						Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y));
					}
				}
				Gizmos.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height));
				Gizmos.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height));
			}
		}

		private Vector2 GetWorldPosition(int x, int y)
		{
			return new Vector2(x, y) * cellSize + originPosition;
		}
	}
}
