using CardboardCore.DI;

namespace CardboardCore.EntityComponents
{
    public abstract class Component
    {
        protected readonly Entity owner;

        public Component(Entity owner)
        {
            this.owner = owner;
        }

        public void Start()
        {
            Injector.Inject(this);
            OnStart();
        }

        public void Stop()
        {
            OnStop();
            Injector.Dump(this);
        }

        public void Update(double deltaTime)
        {
            OnUpdate(deltaTime);
        }

        public T GetComponent<T>(bool throwException = false) where T : Component
        {
            return owner.GetComponent<T>(throwException) as T;
        }

        protected virtual void OnStart() { }
        protected virtual void OnStop() { }
        protected virtual void OnUpdate(double deltaTime) { }
    }
}
