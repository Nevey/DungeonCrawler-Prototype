using CardboardCore.DI;

namespace CardboardCore.EntityComponents
{
    public abstract class Component
    {
        public void Start()
        {
            //Injector.Inject(this);
            OnStart();
        }

        public void Stop()
        {
            OnStop();
            //Injector.Dump(this);
        }

        public void Update(double deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected virtual void OnStart() { }
        protected virtual void OnStop() { }
        protected virtual void OnUpdate(double deltaTime) { }
    }
}
