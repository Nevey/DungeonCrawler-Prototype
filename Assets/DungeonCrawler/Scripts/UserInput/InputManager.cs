using CardboardCore.DI;
using UnityEngine;

namespace DungeonCrawler.UserInput
{
    [Injectable(Singleton = true)]
    public class InputManager : CardboardCoreBehaviour
    {
        [SerializeField] private MovementActionSetController movementActionSetControllerPrefab;
        [SerializeField] private CardActionSetController cardActionSetControllerPrefab;

        public MovementActionSetController movementActionSetController { get; private set; }
        public CardActionSetController cardActionSetController { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            movementActionSetController = Instantiate(movementActionSetControllerPrefab, transform);
            cardActionSetController = Instantiate(cardActionSetControllerPrefab, transform);

            DontDestroyOnLoad(gameObject);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            movementActionSetController.Unbind();
            cardActionSetController.Unbind();
        }
    }
}
