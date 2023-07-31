using Game.Paterns;
using OceanSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [SerializeField]
    private OceanSimulation m_OceanSimulation;

    public OceanSimulation oceanSimulation => m_OceanSimulation;

}
