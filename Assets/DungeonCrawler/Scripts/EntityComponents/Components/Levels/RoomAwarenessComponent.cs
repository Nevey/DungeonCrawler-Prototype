using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;
using DungeonCrawler.UserInput;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RoomAwarenessComponent : Component
    {
        private RoomData currentRoomData;
        private GridPositionComponent gridPositionComponent;

        public RoomAwarenessComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            gridPositionComponent = GetComponent<GridPositionComponent>();
        }

        public void EnterRoom(RoomData roomData)
        {
            currentRoomData = roomData;
        }

        public bool CanWalk(MovementInputEventArgs e)
        {
            int targetX = gridPositionComponent.x;
            int targetY = gridPositionComponent.y;

            switch (e.inputDirection)
            {
                case InputDirection.Horizontal:
                    targetX += e.strength;
                    break;

                case InputDirection.Vertical:
                    targetY += e.strength;
                    break;
            }

            if (targetX < 0 || targetX >= currentRoomData.gridSizeX)
            {
                return false;
            }

            if (targetY < 0 || targetY >= currentRoomData.gridSizeY)
            {
                return false;
            }

            TileData targetTileData = currentRoomData.tiles[targetX, targetY];

            return IsTargetTileAccessible(targetTileData);

            // TODO: Check with multiple room support
        }

        public bool IsTargetTileAccessible(TileData targetTileData)
        {
            switch (targetTileData.tileState)
            {
                case TileState.Default:
                    return true;

                case TileState.Unused:
                    return false;

                case TileState.Doorway:
                    return true;

                default:
                    return true;
            }
        }
    }
}