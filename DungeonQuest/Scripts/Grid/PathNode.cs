namespace DungeonQuest.Grid
{
    public class PathNode
    {
        public int x, y;
        public int gCost, hCost, fCost;
        public bool isWalkable;

        public PathNode cameFromNode;
        private Grid<PathNode> grid;

        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;

            isWalkable = true;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            this.isWalkable = isWalkable;
            grid.TriggerGridObjectChanged(x, y);
        }

        public override string ToString()
        {
            return x + "," + y;
        }
    }
}
