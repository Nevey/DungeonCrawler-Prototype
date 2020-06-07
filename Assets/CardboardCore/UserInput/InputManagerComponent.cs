using System.Collections.Generic;
using CardboardCore.EntityComponents;

namespace CardboardCore.UserInput
{
    public abstract class InputManagerComponent : Component
    {
        private readonly List<ActionSet> actionSets = new List<ActionSet>();

        protected InputManagerComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStop()
        {
            Unbind();
        }

        protected override void OnUpdate(float deltaTime)
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

        protected T AddActionSet<T>() where T : ActionSet, new()
        {
            T actionSet = new T();
            actionSets.Add(actionSet);

            return actionSet;
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
