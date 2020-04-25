using CardboardCore.Loop;
using CardboardCore.DI;
using CardboardCore.Utilities;

namespace CardboardCore.EntityComponents
{
    public class EntityFactory<T> where T : IEntityLoadData
    {
        [Inject] private UpdateLoop updateLoop;
        [Inject] private EntityCollectionLoader entityCollectionLoader;

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

            updateLoop.RegisterGameLoopable(entity);

            return entity;
        }
    }
}
