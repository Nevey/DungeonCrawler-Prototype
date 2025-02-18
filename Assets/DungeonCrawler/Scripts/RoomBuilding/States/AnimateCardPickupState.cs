﻿using CardboardCore.DI;
using CardboardCore.EC;
using DungeonCrawler.EC.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class AnimateCardPickupState : RoomBuilderState
    {
        [Inject] private EntityRegister entityRegister;

        protected override void OnEnter()
        {
            Entity entity = entityRegister.FindEntity("GameplayCameraEntity");

            CardViewComponent cardViewComponent = roomCardDataComponent.GetComponent<CardViewComponent>();
            cardViewComponent.PlayPickupAnimation(entity, () => { owner.ToNextState(); });
        }

        protected override void OnExit()
        {

        }
    }
}
