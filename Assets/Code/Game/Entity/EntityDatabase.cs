using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDatabase : MonoBehaviour, IEcsData
{
    public EntityData[] entities;

    public EntityData GetEntityByID(int id)
    {
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].id == id) return entities[i];
        }
        return null;
    }
}
