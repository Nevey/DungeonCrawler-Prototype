using System;
using System.Collections.Generic;
using CardboardCore.UserInput;
using CardboardCore.UserInput.Actions;

namespace DungeonCrawler.UserInput
{
    public class MovementInputEventArgs : EventArgs
    {
        public InputDirection inputDirection;
        public int strength;
    }

    public class MovementActionSet : ActionSet
    {
        private MovementInputEventArgs movementInputEventArgs = new MovementInputEventArgs();

        private AxisAction axisInputHorizontal;
        private AxisAction axisInputVertical;

        private Dictionary<InputDirection, bool> inputPreviousFrame = new Dictionary<InputDirection, bool>();

        public event EventHandler<MovementInputEventArgs> InputEvent;

        protected override void OnBind()
        {
            axisInputHorizontal = CreateAxisAction("Horizontal");
            axisInputHorizontal.OnAxisInputEvent += OnAxisInputHorizontal;

            axisInputVertical = CreateAxisAction("Vertical");
            axisInputVertical.OnAxisInputEvent += OnAxisInputVertical;
        }

        protected override void OnUnbind()
        {
            axisInputHorizontal.OnAxisInputEvent -= OnAxisInputHorizontal;
            axisInputVertical.OnAxisInputEvent -= OnAxisInputVertical;
        }

        private void OnAxisInputHorizontal(float strength)
        {
            DispatchMovementInputEvent(InputDirection.Horizontal, StrengthNormalized(strength));
        }

        private void OnAxisInputVertical(float strength)
        {
            DispatchMovementInputEvent(InputDirection.Vertical, StrengthNormalized(strength));
        }

        private int StrengthNormalized(float strength)
        {
            if (strength > 0)
            {
                return 1;
            }
            else if (strength < 0)
            {
                return -1;
            }

            return 0;
        }

        private void DispatchMovementInputEvent(InputDirection inputDirection, int strength)
        {
            if (strength == 0)
            {
                inputPreviousFrame[inputDirection] = false;
                return;
            }

            if (inputPreviousFrame[inputDirection])
            {
                return;
            }

            inputPreviousFrame[inputDirection] = true;

            movementInputEventArgs.inputDirection = inputDirection;
            movementInputEventArgs.strength = strength;

            InputEvent?.Invoke(this, movementInputEventArgs);
        }
    }
}