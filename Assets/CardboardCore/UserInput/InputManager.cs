using System.Collections.Generic;
using CardboardCore.DI;
using CardboardCore.Utilities;

namespace CardboardCore.UserInput
{
    public abstract class InputManager : CardboardCoreBehaviour
    {
        private readonly List<ActionSet> actionSets = new List<ActionSet>();

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Unbind();
        }

        protected virtual void Update()
        {
            for (int i = 0; i < actionSets.Count; i++)
            {
                ActionSet actionSet = actionSets[i];
                
                if (!actionSet.IsBound)
                {
                    continue;
                }
                
                actionSet.Update();
            }
        }

        protected void AddActionSet(ActionSet actionSet)
        {
            if (actionSets.Contains(actionSet))
            {
                Log.Warn($"ActionSet {actionSet.GetType().Name} was already registered...");
                return;
            }
            
            actionSets.Add(actionSet);
        }

        public void Bind()
        {
            for (int i = 0; i < actionSets.Count; i++)
            {
                actionSets[i].Bind();
            }
        }

        public void Unbind()
        {
            for (int i = 0; i < actionSets.Count; i++)
            {
                actionSets[i].Unbind();
            }
        }
    }
}
