using Leopotam.EcsLite;
using System.Collections;
using UnityEngine;

public class GradeLevelAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private int m_Amount;

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var gradeLevel = ref ecsWorld.GetPool<GradeLevel>().Add(entity);
        gradeLevel.amount = m_Amount;
    }
}
