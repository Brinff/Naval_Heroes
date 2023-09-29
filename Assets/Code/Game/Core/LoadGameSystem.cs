using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameSystem : MonoBehaviour, IEcsInitSystem, IEcsGroup<Update>
{
    [SerializeField]
    private bool m_IsShooter;
    //[SerializeField]
    //private EntityData m_EntityData;

    private EcsWorld m_World;

    private const string KEY = "2dbfd836d4632a62fcea73af7c31bccf9d964c44";

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        var commandSystem = systems.GetSystem<CommandSystem>();


        SmartlookUnity.SetupOptionsBuilder builder = new SmartlookUnity.SetupOptionsBuilder(KEY);
        builder.SetFps(5);
        builder.SetStartNewSession(true);
        SmartlookUnity.Smartlook.SetupAndStartRecording(builder.Build());

        var playerLevelProvider = systems.GetSystem<PlayerMissionSystem>();
        commandSystem.Execute<CreatePlayerCommand>();

        if (playerLevelProvider.level == 1 && GameSettings.Instance.firstLevelisShooter)
        {
            commandSystem.Execute<GoBattleCommand>();
        }
        else commandSystem.Execute<GoHomeCommand>();
    }
}
