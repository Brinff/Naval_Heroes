using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
using UnityEngine;

public class AbilityPlayerSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>, IEcsDestroySystem
{
    private AbilityWidget m_AbilityWidget;
    private EcsFilter m_AbilityFilter;
    private EcsFilter m_AbilityAutoFightFilter;
    private EcsFilter m_AbilityOtherFilter;
    private EcsWorld m_World;
    private AbilityDatabase m_AbilityDatabase;
    private AbilityAmmoDatabase m_AbilityAmmoDatabase;
    private EcsFilter m_BattleDataFilter;


    private EcsPool<Ability> m_PoolAbility;
    private EcsPool<AbilityGroup> m_PoolAbilityGroup;
    private EcsPool<AbilityReload> m_PoolAbilityReload;
    private EcsPool<AbilityUI> m_PoolAbilityUI;
    private EcsPool<AbilityState> m_PoolAbilityState;
    private EcsPool<AbilityAmmoUI> m_PoolAbilityAmmoUI;
    private EcsPool<AbilityAmmoAmount> m_PoolAbilityAmmoAmount;
    private EcsPool<AbilityAmmo> m_PoolAbilityAmmo;
    private BeginEntityCommandSystem m_BeginEntityCommandSystem;
    private AutoFightToggleWidget m_AutoFightToggleWidget;
    private EcsPool<Root> m_PoolRoot;
    private EcsPool<DeadTag> m_PoolDeadTag;
    private PlayerPrefsData<bool> m_AutoFightToggle;
    private EcsPool<AbilityAutoUse> m_PoolAutoUse;
    public void Init(IEcsSystems systems)
    {
        var uiService = ServiceLocator.Get<UIRoot>();
        
        m_BeginEntityCommandSystem = systems.GetSystem<BeginEntityCommandSystem>();
        m_World = systems.GetWorld();
        m_AbilityDatabase = systems.GetData<AbilityDatabase>();
        m_AbilityAmmoDatabase = systems.GetData<AbilityAmmoDatabase>();
        m_AbilityWidget = uiService.GetWidget<AbilityWidget>();

        m_AutoFightToggle = new PlayerPrefsData<bool>(nameof(m_AutoFightToggle), false);
        m_AutoFightToggleWidget = uiService.GetWidget<AutoFightToggleWidget>();
        m_AutoFightToggleWidget.isToggle = m_AutoFightToggle.Value;
        m_AutoFightToggleWidget.OnToggle += OnToggleAutoFight;

        m_AbilityFilter = m_World.Filter<Ability>().Inc<AbilityGroup>().Inc<PlayerTag>().Inc<CommanderTag>().End();
        m_AbilityAutoFightFilter = m_World.Filter<AbilityAutoUse>().Inc<PlayerTag>().Inc<CommanderTag>().End();

        m_PoolAbilityGroup = m_World.GetPool<AbilityGroup>();
        m_PoolAbility = m_World.GetPool<Ability>();
        m_PoolAbilityAmmo = m_World.GetPool<AbilityAmmo>();
        m_PoolAbilityReload = m_World.GetPool<AbilityReload>();
        m_PoolAbilityUI = m_World.GetPool<AbilityUI>();
        m_PoolAbilityState = m_World.GetPool<AbilityState>();
        m_PoolAbilityAmmoUI = m_World.GetPool<AbilityAmmoUI>();
        m_PoolAbilityAmmoAmount = m_World.GetPool<AbilityAmmoAmount>();
        m_PoolAutoUse = m_World.GetPool<AbilityAutoUse>();

        m_PoolRoot = m_World.GetPool<Root>();
        m_PoolDeadTag = m_World.GetPool<DeadTag>();

        m_BattleDataFilter = m_World.Filter<BattleData>().End();


    }

    private void OnToggleAutoFight(bool isToggle)
    {
        m_AutoFightToggleWidget.isToggle = isToggle;
        m_AutoFightToggle.Value = isToggle;

        //if (m_AbilityAutoFightFilter.IsAny())
        //{
        //    var buffer = m_BeginEntityCommandSystem.CreateBuffer();
        //    foreach (var entity in m_AbilityAutoFightFilter)
        //    {
        //        buffer.SetComponent(entity, new AbilityAutoUse() { isActive = isToggle });
        //    }
        //}
    }

    public void Run(IEcsSystems systems)
    {
        if (!m_BattleDataFilter.IsAny()) return;
        ref var battleData = ref m_BattleDataFilter.GetSingletonComponent<BattleData>();
        if (!battleData.isStarted || battleData.isEnded) return;

        foreach (var entity in m_AbilityAutoFightFilter)
        {
            ref var autoUse = ref m_PoolAutoUse.Get(entity);
            autoUse.isActive = m_AutoFightToggleWidget.isToggle;
        }

        foreach (var abilityEntity in m_AbilityFilter)
        {
            ref var ability = ref m_PoolAbility.Get(abilityEntity);
            ref var abilityGroup = ref m_PoolAbilityGroup.Get(abilityEntity);
            ref var abilityState = ref m_PoolAbilityState.Get(abilityEntity);

            if (abilityGroup.entities != null && abilityGroup.entities.Count > 0)
            {
                if (!m_PoolAbilityUI.Has(abilityEntity))
                {

                    ref var abilityAmmo = ref m_PoolAbilityAmmo.Get(abilityEntity);
                    var abilityData = m_AbilityDatabase.GetById(ability.id);
                    var abilityAmmoData = m_AbilityAmmoDatabase.GetById(abilityAmmo.id);
                    var abilityItem = m_AbilityWidget.CreateAbility();

                    abilityItem.SetAvailable(true);
                    abilityItem.SetAbilityIcon(abilityData.icon);
                    abilityItem.SetId(abilityData.id);
                    abilityItem.SetReload(0);
                    abilityItem.SetAmmoIcon(abilityAmmoData.icon);

                    abilityItem.OnPerform += OnAbilityPerform;


                    foreach (var item in abilityGroup.entities)
                    {


                        if (item.Unpack(m_World, out int childAbility))
                        {
                            ref var abilityAmmoUI = ref m_PoolAbilityAmmoUI.Add(childAbility);
                            abilityAmmoUI.abilityAmmoItem = new List<AmmoItem>();

                            Debug.Log(m_World.GetPool<Link>().Get(childAbility).transform);

                            ref var abilityAmmoAmount = ref m_PoolAbilityAmmoAmount.Get(childAbility);

                            for (int i = 0; i < abilityAmmoAmount.max; i++)
                            {
                                var ammoItem = abilityItem.CreateAmmoItem();
                                ammoItem.SetReload(abilityAmmoAmount.current > i ? 1 : 0);
                                ammoItem.SetAvailable(true);
                                abilityAmmoUI.abilityAmmoItem.Add(ammoItem);
                            }
                        }
                    }

                    abilityItem.UpdateAmmo();

                    ref var abilityUI = ref m_PoolAbilityUI.Add(abilityEntity);
                    abilityUI.abilityItem = abilityItem;
                }
                else
                {
                    ref var abilityUI = ref m_PoolAbilityUI.Get(abilityEntity);
                    ref var abilityReload = ref m_PoolAbilityReload.Get(abilityEntity);
                    abilityUI.abilityItem.SetReload(abilityReload.progress);
                    bool isAnyAlive = false;
                    foreach (var item in abilityGroup.entities)
                    {
                        if (item.Unpack(m_World, out int childAbility))
                        {
                            ref var abilityAmmoUI = ref m_PoolAbilityAmmoUI.Get(childAbility);
                            ref var abilityAmmoAmount = ref m_PoolAbilityAmmoAmount.Get(childAbility);
                            ref var abilityAmmoReload = ref m_PoolAbilityReload.Get(childAbility);
                            ref var rootAbility = ref m_PoolRoot.Get(childAbility);

                            bool isDead = false;
                            if(rootAbility.entity.Unpack(m_World, out int rootAbilityEntity))
                            {
                                if (m_PoolDeadTag.Has(rootAbilityEntity))
                                {
                                    isDead = true;
                                }
                                else
                                {
                                    isAnyAlive = true;
                                }
                            }

                            for (int i = 0; i < abilityAmmoAmount.max; i++)
                            {
                                var ammoItem = abilityAmmoUI.abilityAmmoItem[i];

                                if (isDead)
                                {
                                    ammoItem.SetAvailable(false);
                                    ammoItem.SetReload(0);
                                    continue;
                                }
                                //ammoItem.SetReload(abilityAmmoReload.progress);
                                //ammoItem.SetReload(abilityAmmoReload.progress);
                                ammoItem.SetReload(abilityAmmoAmount.current > i ? 1 : abilityAmmoAmount.current == i ? abilityAmmoReload.progress : 0);

                            }
                        }
                    }

                    abilityState.isAnyAlive = isAnyAlive;

                    abilityUI.abilityItem.SetAvailable(isAnyAlive);

                    abilityUI.abilityItem.UpdateAmmo();
                }
            }
        }
    }

    private void OnAbilityPerform(int id)
    {
        var abilityData = m_AbilityDatabase.GetById(id);
        foreach (var abilityEntity in m_AbilityFilter)
        {
            ref var ability = ref m_PoolAbility.Get(abilityEntity);
            var abilityState = m_PoolAbilityState.Get(abilityEntity);

            if (id == ability.id && abilityState.isAvailable && abilityState.isAnyAlive)
            {
                var beginEcb = m_BeginEntityCommandSystem.CreateBuffer();
                abilityState.isPerfrom = true;
                beginEcb.SetComponent(abilityEntity, abilityState);

                Debug.Log($"Perform: {abilityData.name}");
            }
        }

    }

    public void Destroy(IEcsSystems systems)
    {
        m_AutoFightToggleWidget.OnToggle -= OnToggleAutoFight;
        m_AutoFightToggleWidget = null;
    }
}
