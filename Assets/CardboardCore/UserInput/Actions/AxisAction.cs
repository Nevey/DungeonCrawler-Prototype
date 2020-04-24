using UnityEngine;

namespace CardboardCore.UserInput.Actions
{
    public class AxisAction : Action
    {
        private readonly string axis;
        private float axisInput;

        public float AxisInput => axisInput;

        public AxisAction(string axis)
        {
            this.axis = axis;
        }
        
        public override void Update()
        {
            axisInput = Input.GetAxis(axis);
        }

        public override void Reset()
        {
            axisInput = 0f;
        }
    }
}
