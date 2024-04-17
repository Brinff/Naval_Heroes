using Code.Services;
using Code.States;
using UnityEngine;

namespace Code.Game.States
{
    public class LaunchGameState : MonoBehaviour, IPlayState
    {
        [SerializeField] private GameObject m_Player;

        public void OnPlay(IStateMachine stateMachine)
        {
            var entityManager = ServiceLocator.Get<EntityManager>();

            var instance = Instantiate(m_Player);
            entityManager.world.Bake(instance);

            //var m_World = systems.GetWorld();
            /*var commandSystem = entityManager.GetSystem<CommandSystem>();

            var playerLevelProvider = entityManager.GetSystem<PlayerMissionSystem>();
            commandSystem.Execute<CreatePlayerCommand>();*/

            /*
            if (playerLevelProvider.level == 1 && GameSettings.Instance.firstEnterInBattle)
            {
                commandSystem.Execute<GoBattleCommand>();
            }
            else //commandSystem.Execute<GoHomeCommand>();*/

            stateMachine.Play<HomeState>();
        }
    }
}