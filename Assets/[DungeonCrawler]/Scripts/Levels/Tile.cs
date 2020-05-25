namespace DungeonCrawler.Levels
{
    public enum WalkableState
    {
        Walkable,
        UnWalkable
    }

    public class Tile
    {
        private int x;
        private int y;
        private WalkableState walkableState;
    }
}