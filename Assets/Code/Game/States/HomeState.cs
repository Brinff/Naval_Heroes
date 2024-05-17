using Code.Services;
using Code.States;
using Game.UI;
using Leopotam.EcsLite;
using Sirenix.Utilities;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Code.Game.States
{
    public class HomeState : MonoBehaviour, IPlayState, IStopState
    {
        private PlayerSlotsSystem m_PlayerSlotsSystem;
        private StartGameWidget m_StartGameWidget;
        private CommandSystem m_CommandSystem;
        private IStateMachine m_StateMachine;
        [SerializeField]
        private GameObject m_BattleVirtualCamera;
        public void OnPlay(IStateMachine stateMachine)
        {
            m_StateMachine = stateMachine;
            
            var entityManager = ServiceLocator.Get<EntityManager>();
            var world = entityManager.world;
            
            PlayerMissionSystem playerMissionSystem = entityManager.GetSystem<PlayerMissionSystem>();

            //StartCoroutine(WaitLockBattle());
            
            m_PlayerSlotsSystem = entityManager.GetSystem<PlayerSlotsSystem>();
            m_PlayerSlotsSystem.slotCollection.OnChange += OnChangeSlotCollection;


            m_StartGameWidget = ServiceLocator.Get<UIController>().GetWidget<StartGameWidget>();
            m_StartGameWidget.SetLevel(playerMissionSystem.level);
            m_StartGameWidget.OnClick += OnClickBattle;
            m_StartGameWidget.SetBlock(!m_PlayerSlotsSystem.IsAnyRadyBattle());

            var navigateMenu = ServiceLocator.Get<UIController>().GetWidget<NavigateMenuWidget>();
            var item = navigateMenu.items.First(x => x.name == "Fleet");
            item.SetLock(false, true);
            navigateMenu.Select(item, true);


            ServiceLocator.Get<UIController>().compositionModule.Show<UIHomeComposition>();
            m_CommandSystem = entityManager.GetSystem<CommandSystem>();
            m_CommandSystem.Execute<ClearBattleDataCommand>();


            m_BattleVirtualCamera.SetActive(false);

            /*            var viewHome = world.Filter<ViewComponent>().Inc<HomeTag>().End().GetSingleton();
                        ref var eye = ref world.Filter<EyeComponent>().End().GetSingletonComponent<EyeComponent>();
                        eye.view = world.PackEntity(viewHome.Value);*/

            var slotSystem = entityManager.GetSystem<PlayerSlotsSystem>();

            var slots = slotSystem.slotCollection.GetSlots<SlotBuy>();
            foreach (var slot in slots)
            {
                slot.SetMissionAmount(playerMissionSystem.level);
            }

            slotSystem.Show();

            //m_CommandSystem.Execute<SetupPlayerCommand>();

            entityManager.GetSystem<TutorialSystem>().HomeTutorial();
        }

        private void OnChangeSlotCollection(SlotCollection collection)
        {
            m_StartGameWidget.SetBlock(!m_PlayerSlotsSystem.IsAnyRadyBattle());
        }

        private void OnClickBattle()
        {
            if (!m_PlayerSlotsSystem.IsAnyRadyBattle())
            {
                return;
            }
            
            m_StateMachine.Play<BattleState>();
            //m_CommandSystem.Execute<GoBattleCommand>();
        }

        public void OnStop(IStateMachine stateMachine)
        {
            m_StartGameWidget.OnClick -= OnClickBattle;
            m_PlayerSlotsSystem.slotCollection.OnChange -= OnChangeSlotCollection;
        }
    }
}