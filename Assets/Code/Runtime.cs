
using Game.UI;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Runtime : MonoBehaviour
{

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    [SerializeField]
    private Transform m_Player;

    private EcsPackedEntityWithWorld m_PlayerEntity;

    private StartGameWidget startGameWidget;
    public void Start()
    {
        int playerEntity = EntityManager.Instance.world.Bake(m_Player);

        EntityManager.Instance.world.GetPool<HomeViewActiveEvent>().Add(playerEntity);
        EntityManager.Instance.world.GetPool<PlayerTagLeo>().Add(playerEntity);

        m_PlayerEntity = EntityManager.Instance.world.PackEntityWithWorld(playerEntity);

        //startGameWidget = UISystem.Instance.GetElement<StartGameWidget>();
        //startGameWidget.OnClick += OnStartGame;
        //UISystem.Instance.compositionModule.Show<UIHomeComposition>();

        //EntityManager.Instance.world.GetPool<PlayerTag>().Add(entity);
    }

    [SerializeField]
    private List<ScriptBehaviour> m_Scripts = new List<ScriptBehaviour>();

    private ScriptBehaviour GetScript()
    {
        return m_Scripts.First();
    }

    [Button]
    public void OnStartGame()
    {

        if (m_PlayerEntity.Unpack(out EcsWorld world, out int entity))
        {         
            world.GetPool<PlayerAimPointComponent>().Add(entity);
            //world.GetPool<OrbitViewActiveEvent>().Add(entity);
        }

        var script = GetScript();
        script.Launch(m_PlayerEntity);
    }

    public void EndGame()
    {

    }
}
