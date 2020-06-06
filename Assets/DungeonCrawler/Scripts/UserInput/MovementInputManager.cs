using System;
using CardboardCore.UserInput;

namespace DungeonCrawler.UserInput
{
    public class MovementInputManager : InputManager
    {
        private MovementActionSet movementActionSet;

        public event EventHandler<MovementInputEventArgs> InputEvent;

        protected override void Awake()
        {
            base.Awake();

            movementActionSet = AddActionSet<MovementActionSet>();
            movementActionSet.InputEvent += OnMovementInput;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            movementActionSet.InputEvent -= OnMovementInput;
        }

        private void OnMovementInput(object sender, MovementInputEventArgs e)
        {
            InputEvent?.Invoke(this, e);
        }
    }
}