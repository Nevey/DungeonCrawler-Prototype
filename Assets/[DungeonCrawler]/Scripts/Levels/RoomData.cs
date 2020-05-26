namespace DungeonCrawler.Levels
{
    public class RoomData
    {
        public int id;
        public int gridSizeX;
        public int gridSizeY;
        public TileData[,] tiles;

        public RoomData(int id, int gridSizeX, int gridSizeY)
        {
            this.id = id;
            this.gridSizeX = gridSizeX;
            this.gridSizeY = gridSizeY;
        }
    }
}