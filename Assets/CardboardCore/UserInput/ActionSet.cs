using System.Collections.Generic;
using CardboardCore.UserInput.Actions;
using UnityEngine;

namespace CardboardCore.UserInput
{
    public abstract class ActionSet
    {
        private readonly List<Action> actions = new List<Action>();
        private bool isBound;

        public bool IsBound => isBound;

        protected AxisAction CreateAxisAction(string axis)
        {
            AxisAction axisAction = new AxisAction(axis);
            actions.Add(axisAction);

            return axisAction;
        }

        protected ButtonAction CreateButtonAction(KeyCode keyCode)
        {
            ButtonAction buttonAction = new ButtonAction(keyCode);
            actions.Add(buttonAction);

            return buttonAction;
        }

        protected abstract void OnBind();
        protected abstract void OnUnbind();

        public void Bind()
        {
            isBound = true;
            OnBind();
        }

        public void Unbind()
        {
            isBound = false;
            OnUnbind();

            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].Reset();
            }
        }

        public virtual void Update()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].Update();
            }
        }
    }
}
