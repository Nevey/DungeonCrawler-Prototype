namespace DungeonCrawler.Levels
{
    public enum WalkableState
    {
        Walkable,
        UnWalkable
    }

    public class TileData
    {
        public int x;
        public int y;
        public WalkableState walkableState;
    }
}