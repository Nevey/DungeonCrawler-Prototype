using CardboardCore.EntityComponents;
using CardboardCore.Utilities;
using DG.Tweening;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardViewComponent : ViewComponent
    {
        private GridPositionComponent gridPositionComponent;

        protected override void OnStart()
        {
            base.OnStart();
            LoadFinishedEvent += OnLoadFinished;

            gridPositionComponent = GetComponent<GridPositionComponent>();
        }

        protected override void OnStop()
        {
            base.OnStop();
            LoadFinishedEvent -= OnLoadFinished;
        }

        private void OnLoadFinished(ViewComponent viewComponent)
        {
            PlaceOnGridBacksideUp();
        }

        private void PlaceOnGridBacksideUp()
        {
            positionComponent.SetPosition(gridPositionComponent.x, 0.1f, gridPositionComponent.y);

            rotationComponent.SetRotation(90f, 0f, 0f);
            rotationComponent.SetRandomRotationZ();
        }

        public void PlayPickupAnimation(Entity targetEntity)
        {
            PositionComponent targetPositionComponent = targetEntity.GetComponent<PositionComponent>();
            RotationComponent targetRotationComponent = targetEntity.GetComponent<RotationComponent>();

            UnityEngine.Vector3 direction = targetPositionComponent.position - positionComponent.position;
            UnityEngine.Quaternion lookAtRotation = UnityEngine.Quaternion.LookRotation(direction, UnityEngine.Vector3.up);

            UnityEngine.Vector3 liftPosition = positionComponent.position;
            liftPosition.y += 2f;

            UnityEngine.Vector3 cardFromTargetOffset = new UnityEngine.Vector3(0f, -1.1f, 1.1f);
            UnityEngine.Vector3 cardFromTargetRelativePosition = RotationUtil.GetVectorSimple(0, targetRotationComponent.euler.y, 0, cardFromTargetOffset);

            // Rotate card to face target
            // Move card up above it's tile
            // Move card toward target
            Sequence sequence = DOTween.Sequence();
            sequence.Append(rotationComponent.SetRotationAnimated(lookAtRotation, 1f));
            sequence.Insert(0f, positionComponent.SetPositionAnimated(liftPosition, 0.5f));
            sequence.Insert(0.5f, positionComponent.SetPositionAnimated(targetPositionComponent.position + cardFromTargetRelativePosition, 0.75f));
            sequence.Play();
        }
    }
}
