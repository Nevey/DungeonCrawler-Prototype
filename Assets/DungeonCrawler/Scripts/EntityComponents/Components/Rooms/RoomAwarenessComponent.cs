using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;
using DungeonCrawler.UserInput;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RoomAwarenessComponent : Component
    {
        private RoomDataComponent currentRoomDataComponent;
        private GridPositionComponent gridPositionComponent;

        protected override void OnStart()
        {
            gridPositionComponent = GetComponent<GridPositionComponent>();
        }

        public void EnterRoom(RoomDataComponent roomDataComponent)
        {
            currentRoomDataComponent = roomDataComponent;
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

            RoomData roomData = currentRoomDataComponent.roomData;

            if (targetX < 0 || targetX >= roomData.gridSizeX)
            {
                return false;
            }

            if (targetY < 0 || targetY >= roomData.gridSizeY)
            {
                return false;
            }

            TileData targetTileData = roomData.tiles[targetX, targetY];

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

        public bool GetRoomCardAtGridLocation(int x, int y, out RoomCardDataComponent cardDataComponent)
        {
            for (int i = 0; i < currentRoomDataComponent.roomCardDataComponents.Count; i++)
            {
                RoomCardDataComponent roomCardDataComponent = currentRoomDataComponent.roomCardDataComponents[i];
                GridPositionComponent cardGridPositionComponent = roomCardDataComponent.GetComponent<GridPositionComponent>();

                if (cardGridPositionComponent.x == x && cardGridPositionComponent.y == y)
                {
                    cardDataComponent = roomCardDataComponent;
                    return true;
                }
            }

            cardDataComponent = null;
            return false;
        }
    }
}