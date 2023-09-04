using Leopotam.EcsLite;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GradeLevelAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField, FormerlySerializedAs("m_Amount")]
    public int amount;

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var gradeLevel = ref ecsWorld.GetPool<GradeLevel>().Add(entity);
        gradeLevel.amount = amount;
    }
}
