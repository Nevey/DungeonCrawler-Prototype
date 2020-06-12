using CardboardCore.DI;

namespace CardboardCore.EC
{
    public abstract class Component
    {
        public readonly Entity owner;

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

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        public T GetComponent<T>(bool throwException = false) where T : Component
        {
            return owner.GetComponent<T>(throwException) as T;
        }

        protected virtual void OnStart() { }
        protected virtual void OnStop() { }
        protected virtual void OnUpdate(float deltaTime) { }
    }
}
