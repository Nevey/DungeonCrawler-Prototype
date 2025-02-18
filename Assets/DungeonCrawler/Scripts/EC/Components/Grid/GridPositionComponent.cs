using System;
using CardboardCore.EC;

namespace DungeonCrawler.EC.Components
{
    public class GridPositionComponent : Component
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public event Action<int, int> PositionUpdatedEvent;

        private void DispatchPositionUpdatedEvent()
        {
            PositionUpdatedEvent?.Invoke(x, y);
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;

            DispatchPositionUpdatedEvent();
        }

        public void UpdateXY(int amountX, int amountY)
        {
            x += amountX;
            y += amountY;

            DispatchPositionUpdatedEvent();
        }

        public void UpdateX(int amount)
        {
            x += amount;

            DispatchPositionUpdatedEvent();
        }

        public void UpdateY(int amount)
        {
            y += amount;

            DispatchPositionUpdatedEvent();
        }
    }
}
