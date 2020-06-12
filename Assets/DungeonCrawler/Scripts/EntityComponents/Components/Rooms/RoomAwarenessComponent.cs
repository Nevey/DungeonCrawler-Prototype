using CardboardCore.EC;
using DungeonCrawler.Levels;
using DungeonCrawler.UserInput;

namespace DungeonCrawler.EC.Components
{
    public class RoomAwarenessComponent : Component
    {
        private Entity cameraEntity;
        private CameraTargetComponent cameraTargetComponent;

        private Entity levelEntity;
        private TileRegistryComponent tileRegistryComponent;

        private GridPositionComponent gridPositionComponent;

        public RoomDataComponent currentRoom { get; private set; }

        protected override void OnStart()
        {
            gridPositionComponent = GetComponent<GridPositionComponent>();
        }

        public void Setup(Entity cameraEntity, Entity levelEntity)
        {
            this.cameraEntity = cameraEntity;
            cameraTargetComponent = cameraEntity.GetComponent<CameraTargetComponent>();

            this.levelEntity = levelEntity;
            tileRegistryComponent = levelEntity.GetComponent<TileRegistryComponent>();
        }

        public void EnterRoom(RoomDataComponent currentRoom)
        {
            this.currentRoom = currentRoom;
        }

        public bool CanWalk(MovementInputEventArgs e)
        {
            int targetX = gridPositionComponent.x - currentRoom.offsetX;
            int targetY = gridPositionComponent.y - currentRoom.offsetY;

            switch (e.inputDirection)
            {
                case InputDirection.Horizontal:
                    targetX += e.strength;
                    break;

                case InputDirection.Vertical:
                    targetY += e.strength;
                    break;
            }

            RoomData roomData = currentRoom.roomData;

            if (targetX < 0 || targetX >= roomData.gridSizeX
                || targetY < 0 || targetY >= roomData.gridSizeY)
            {
                // TODO: If the amount of tiles gets too large, start using rooms to create a smaller search area
                // Get tile component with target x and y from tile registry
                int localTargetX = targetX + currentRoom.offsetX;
                int locatTargetY = targetY + currentRoom.offsetY;
                TileDataComponent tileDataComponent = tileRegistryComponent.GetTile(localTargetX, locatTargetY);

                if (tileDataComponent == null)
                {
                    return false;
                }

                bool isAccessible = IsTargetTileAccessible(tileDataComponent.tileData);

                if (isAccessible)
                {
                    EnterRoom(tileDataComponent.parentRoom);
                    cameraTargetComponent.SetTarget(tileDataComponent.parentRoom.owner);
                }

                return isAccessible;
            }

            TileData targetTileData = roomData.tiles[targetX, targetY];

            return IsTargetTileAccessible(targetTileData);
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
            for (int i = 0; i < currentRoom.roomCardDataComponents.Count; i++)
            {
                RoomCardDataComponent roomCardDataComponent = currentRoom.roomCardDataComponents[i];
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
