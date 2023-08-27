using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShipAimAutoSelectSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{

    private EcsWorld m_World;
    private EcsFilter m_ShipAbilityFilter;
    private EcsFilter m_ShipEnemyFilter;

    private EcsPool<AbilityState> m_PoolAbilityState;
    private EcsPool<AbilityAim> m_PoolAbilityAim;
    private EcsPool<AbilityGroup> m_PoolAbilityGroup;
    private EcsPool<RootComponent> m_PoolRoot;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<TransformComponent> m_PoolTransform;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_ShipAbilityFilter = m_World.Filter<AbilityState>().Inc<AbilityAim>().Inc<AbilityGroup>().Inc<ShipTag>().End();
        m_ShipEnemyFilter = m_World.Filter<ShipTag>().Inc<TransformComponent>().Inc<Team>().Exc<DeadTag>().End();

        m_PoolAbilityState = m_World.GetPool<AbilityState>();
        m_PoolAbilityAim = m_World.GetPool<AbilityAim>();
        m_PoolAbilityGroup = m_World.GetPool<AbilityGroup>();
        m_PoolRoot = m_World.GetPool<RootComponent>();
        m_PoolTeam = m_World.GetPool<Team>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_ShipAbilityFilter)
        {
            ref var abilityState = ref m_PoolAbilityState.Get(entity);
            if (abilityState.isPerfrom && abilityState.isAvailable)
            {
                ref var abilityGroup = ref m_PoolAbilityGroup.Get(entity);
                ref var abilityAim = ref m_PoolAbilityAim.Get(entity);
                ref var root = ref m_PoolRoot.Get(entity);
                int? selectEnemyEntity = null;
                if (root.entity.Unpack(m_World, out int rootEntity))
                {
                    ref var team = ref m_PoolTeam.Get(rootEntity);
                    int countEnemy = 0;
                    foreach (var enemyEntity in m_ShipEnemyFilter)
                    {
                        ref var enemyTeam = ref m_PoolTeam.Get(enemyEntity);
                        if (enemyTeam.id != team.id)
                        {
                            countEnemy++;
                        }
                    }
                    int selectEnemy = Random.Range(0, countEnemy);
                    int indexEnemy = 0;
                    foreach (var enemyEntity in m_ShipEnemyFilter)
                    {
                        ref var enemyTeam = ref m_PoolTeam.Get(enemyEntity);
                        if (enemyTeam.id != team.id)
                        {
                            if (indexEnemy == selectEnemy)
                            {
                                selectEnemyEntity = enemyEntity;
                                break;
                            }
                            indexEnemy++;
                        }
                    }
                }

                if (selectEnemyEntity != null)
                {
                    abilityAim.target = m_World.PackEntity(selectEnemyEntity.Value);
                    ref var enemyTransfrom = ref m_PoolTransform.Get(selectEnemyEntity.Value);
                    abilityAim.point = enemyTransfrom.transform.position;

                    if (abilityGroup.entities != null)
                        foreach (var weaponPackEntity in abilityGroup.entities)
                        {
                            if (weaponPackEntity.Unpack(m_World, out int weaponEntity))
                            {
                                ref var weaponAim = ref m_PoolAbilityAim.Get(weaponEntity);
                                weaponAim.target = abilityAim.target;
                                weaponAim.point = abilityAim.point;
                            }
                        }
                }




                //abilityAim.point = 
            }
        }
    }
}
