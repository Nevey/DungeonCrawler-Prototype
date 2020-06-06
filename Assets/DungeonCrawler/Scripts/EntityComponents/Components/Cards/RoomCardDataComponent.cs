using CardboardCore.EntityComponents;
using DungeonCrawler.Cards;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RoomCardDataComponent : CardDataComponent<RoomCardData>
    {
        public RoomCardDataComponent(Entity owner) : base(owner)
        {
        }
    }
}