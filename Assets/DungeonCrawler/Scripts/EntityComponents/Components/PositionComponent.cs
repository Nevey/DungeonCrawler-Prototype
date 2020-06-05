using System;
using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class PositionComponent : Component
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public event Action<int, int> PositionUpdatedEvent;

        public PositionComponent(Entity owner) : base(owner)
        {
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;

            PositionUpdatedEvent?.Invoke(x, y);
        }
    }
}