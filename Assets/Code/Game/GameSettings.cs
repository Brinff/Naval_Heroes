using Game.Paterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameSettings : Singleton<GameSettings>
{
    [InlineEditor]
    public MergeSettings merge;


    [SerializeField]
    public bool firstLevelisShooter;
    [SerializeField]
    public float timeLockScreen = 0.3f;
}
