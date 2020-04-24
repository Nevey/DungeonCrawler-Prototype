using UnityEngine;

namespace CardboardCore.UserInput.Actions
{
    public class ButtonAction : Action
    {
        private readonly KeyCode keyCode;
        
        private bool isReleased;
        private bool isHeld;
        private bool isPressed;

        /// <summary>
        /// Will be true for one frame when a button is pressed
        /// </summary>
        public bool IsPressed => isPressed;

        /// <summary>
        /// Will be true as long as the button is pressed
        /// </summary>
        public bool IsHeld => isHeld;

        /// <summary>
        /// Will be true for one frame when a button is released
        /// </summary>
        public bool IsReleased => isReleased;

        public event System.Action PressEvent;
        public event System.Action HoldEvent;
        public event System.Action ReleaseEvent;

        public ButtonAction(KeyCode keyCode)
        {
            this.keyCode = keyCode;
        }

        public override void Update()
        {
            isPressed = Input.GetKeyDown(keyCode);
            if (isPressed)
            {
                PressEvent?.Invoke();
            }

            isHeld = Input.GetKey(keyCode);
            if (isHeld)
            {
                HoldEvent?.Invoke();
            }

            isReleased = Input.GetKeyUp(keyCode);
            if (isReleased)
            {
                ReleaseEvent?.Invoke();
            }
        }

        public override void Reset()
        {
            isPressed = false;
            isHeld = false;
            isReleased = false;
        }
    }
}
