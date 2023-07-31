using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSpawnPointOther : ScriptSpawnPoint
{
    [SerializeField, Range(0, 6)]
    private int m_Team = 1;

    public int team => m_Team;

    [SerializeField]
    private EntityData m_EntityData;

    private static readonly Color[] s_TeamsColor = new Color[] { Color.green, Color.red, Color.blue, Color.yellow, Color.black, Color.cyan, Color.magenta };

    protected override Color GetColor()
    {
        return s_TeamsColor[m_Team];
    }

    public EntityData entityData => m_EntityData;
}
