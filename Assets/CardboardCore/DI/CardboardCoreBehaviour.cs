namespace CardboardCore.DI
{
    /// <summary>
    /// Extends Unity's MonoBehaviour, automatically injects and dumps any fields
    /// having the Inject attribute
    /// </summary>
    public abstract class CardboardCoreBehaviour : UnityEngine.MonoBehaviour
    {
        protected virtual void Awake()
        {
            Injector.Inject(this);
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            
        }

        protected virtual void OnDestroy()
        {
            Injector.Dump(this);
        }
    }
}