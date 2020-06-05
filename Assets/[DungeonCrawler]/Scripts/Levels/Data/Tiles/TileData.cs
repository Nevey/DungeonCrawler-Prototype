namespace DungeonCrawler.Levels
{
    public enum WalkableState
    {
        UnWalkable,
        Walkable,
    }

    public enum TileState
    {
        Default,
        Unused,
        Doorway,
    }

    public class TileData
    {
        public int x;
        public int y;
        public TileState tileState;
        public WalkableState walkableState;
    }
}