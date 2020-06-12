using CardboardCore.Loop;
using CardboardCore.DI;

namespace CardboardCore.EC
{
    public class EntityFactory<T> where T : IEntityLoadData
    {
        [Inject] private EntityCollectionLoader entityCollectionLoader;
        [Inject] private EntityRegister entityRegister;
        [Inject] private UpdateLoop updateLoop;

        private EntityDataCollection entityDataCollection;

        public EntityFactory()
        {
            Injector.Inject(this);
        }

        ~EntityFactory()
        {
            Injector.Dump(this);
        }

        protected void Initialize(T entityLoadData)
        {
            entityDataCollection = entityCollectionLoader.Load(entityLoadData);
        }

        public Entity Instantiate(string id)
        {
            EntityData entityData = entityDataCollection.GetEntityWithId(id);
            Entity entity = new Entity(entityData);

            entityRegister.RegisterEntity(entity);
            updateLoop.RegisterGameLoopable(entity);

            return entity;
        }

        public void Destroy(Entity entity)
        {
            updateLoop.UnregisterGameLoopable(entity);
            entity = null;
        }
    }
}
