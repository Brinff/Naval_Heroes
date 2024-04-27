using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Game.UI;
using Sirenix.OdinInspector;
using System.Linq;
using Code.Services;

public struct AbilityTargetArea
{
    public Vector3 point;
    public GameObject element;
    public float time;
}

public class PlayerZoomFireSystem : MonoBehaviour, IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsWorld m_World;
    private ZoomToggleWidget m_ZoomToggleWidget;
    private CameraInputWidget m_CameraInputWidget;
    private ZoomFactorWidget m_ZoomFactorWidget;


    private bool m_IsZoom;
    private Vector2 m_Delta;
    private float m_ZoomFactor;
    [SerializeField]
    private Camera m_Camera;

    [SerializeField, Range(0f, 10f)]
    private float m_SensitivityHorizontal = 1;
    [SerializeField, Range(0f, 10f)]
    private float m_SensitivityVertical = 1;
    [SerializeField, Range(0, 10)]
    private float m_SensitivityScale = 1;
    [SerializeField]
    private float m_TiltAngleMin = -10;
    [SerializeField]
    private float m_TiltAngleMax = 10;

    [SerializeField, MinMaxSlider(5, 179, true)]
    private Vector2 m_FieldOfViewAtZoom = new Vector2(30, 60);
    [SerializeField, MinMaxSlider(0, 2, true)]
    private Vector2 m_RotationSensitivityAtZoom = new Vector2(0.1f, 1);

    private EcsFilter m_AbilityFilter;
    private EcsFilter m_AbilityGroupFilter;
    private EcsFilter m_ZoomFilter;


    private EcsPool<ViewComponent> m_PoolView;
    private EcsPool<AbilityState> m_PoolAbilityState;
    private EcsPool<AbilityAim> m_PoolAbilityAim;
    private EcsPool<AbilityGroup> m_PoolAbilityGroup;
    private EcsPool<Ability> m_PoolAbility;

    private EcsFilter m_ShipColliderFilter;
    private EcsPool<ProjectileColliderComponent> m_PoolProjectileCollider;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<DeadTag> m_PoolDeadTag;
    private EcsPool<Root> m_PoolRoot;
    private EcsPool<AbilityTargetArea> m_PoolAbilityTargetArea;
    private EcsFilter m_BattleDataFilter;
    private EcsPool<BattleData> m_PoolBattleData;
    private Bounds m_Bounds;
    private Vector3 m_Point;

    public void Init(IEcsSystems systems)
    {
        m_ZoomToggleWidget = ServiceLocator.Get<UIController>().GetElement<ZoomToggleWidget>();
        m_ZoomToggleWidget.OnToggle += ZoomToggle;

        m_CameraInputWidget = ServiceLocator.Get<UIController>().GetElement<CameraInputWidget>();
        m_CameraInputWidget.OnEndInput += OnCameraEndInput;
        m_CameraInputWidget.OnUpdateInput += OnCameraUpdateInput;

        m_ZoomFactorWidget = ServiceLocator.Get<UIController>().GetElement<ZoomFactorWidget>();
        m_ZoomFactorWidget.OnChangeZoomFactor += OnChangeZoomFactor;


        m_World = systems.GetWorld();
        m_PoolBattleData = m_World.GetPool<BattleData>();

        m_BattleDataFilter = m_World.Filter<BattleData>().End();

        m_ZoomFilter = m_World.Filter<ViewComponent>().Inc<ZoomTag>().End();
        m_AbilityFilter = m_World.Filter<AbilityState>().Inc<AbilityAim>().Inc<AbilityGroup>().Inc<PlayerTag>().End();
        m_AbilityGroupFilter = m_World.Filter<AbilityState>().Inc<AbilityAim>().Inc<AbilityGroup>().End();

        m_PoolView = m_World.GetPool<ViewComponent>();
        m_PoolAbilityState = m_World.GetPool<AbilityState>();
        m_PoolAbilityAim = m_World.GetPool<AbilityAim>();
        m_PoolAbilityGroup = m_World.GetPool<AbilityGroup>();

        m_ShipColliderFilter = m_World.Filter<Root>().Inc<ProjectileColliderComponent>().End();

        m_PoolProjectileCollider = m_World.GetPool<ProjectileColliderComponent>();
        m_PoolTeam = m_World.GetPool<Team>();
        m_PoolRoot = m_World.GetPool<Root>();
        m_PoolDeadTag = m_World.GetPool<DeadTag>();
        m_PoolAbility = m_World.GetPool<Ability>();
        m_PoolAbilityTargetArea = m_World.GetPool<AbilityTargetArea>();
    }

    private void OnChangeZoomFactor(float value)
    {
        m_ZoomFactor = value;
    }

    private void OnCameraEndInput(Vector2 delta)
    {
        m_Delta = Vector2.zero;
    }

    private void OnCameraUpdateInput(Vector2 delta)
    {
        m_Delta = delta;
    }

    public void Destroy(IEcsSystems systems)
    {
        m_ZoomToggleWidget.OnToggle -= ZoomToggle;
        m_CameraInputWidget.OnEndInput -= OnCameraEndInput;
        m_CameraInputWidget.OnUpdateInput -= OnCameraUpdateInput;
        m_ZoomFactorWidget.OnChangeZoomFactor -= OnChangeZoomFactor;

        m_ZoomToggleWidget = null;
        m_ZoomFactorWidget = null;
        m_CameraInputWidget = null;
    }

    public void ZoomToggle(bool value)
    {
        m_IsZoom = value;

        m_ZoomToggleWidget.SetToggle(value);

        if (m_IsZoom)
        {
            var viewZoom = m_World.Filter<ViewComponent>().Inc<ZoomTag>().End().GetSingleton();
            ref var eye = ref m_World.Filter<EyeComponent>().End().GetSingletonComponent<EyeComponent>();


            ref var view = ref m_PoolView.Get(viewZoom.Value);
            view.position = new Vector3(m_Bounds.center.x, m_Bounds.max.y, m_Bounds.max.z);

            eye.view = m_World.PackEntity(viewZoom.Value);

            ServiceLocator.Get<UIController>().compositionModule.Show<UIGameShipZoomComposition>();


        }
        else
        {
            var viewBattle = m_World.Filter<ViewComponent>().Inc<BattleTag>().End().GetSingleton();
            ref var eye = ref m_World.Filter<EyeComponent>().End().GetSingletonComponent<EyeComponent>();
            eye.view = m_World.PackEntity(viewBattle.Value);

            ServiceLocator.Get<UIController>().compositionModule.Show<UIBattleComposition>();

        }
    }
    public Vector2 GetAxis(float sensitivityHorizontal, float sensitivityVertical, float sensitivityScale)
    {
        return Vector2.Scale(m_Delta, new Vector2(sensitivityHorizontal, sensitivityVertical)) * sensitivityScale * Time.deltaTime;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(m_Bounds.center, m_Bounds.size);
        Gizmos.DrawSphere(m_Point, 1);
    }

    private bool m_IsStarted;
    private bool m_IsEnded;

    
    public void Run(IEcsSystems systems)
    {
        var sencitivityScale = m_SensitivityScale * Mathf.Lerp(m_RotationSensitivityAtZoom.y, m_RotationSensitivityAtZoom.x, m_ZoomFactor);
        var fieldOfView = Mathf.Lerp(m_FieldOfViewAtZoom.y, m_FieldOfViewAtZoom.x, m_ZoomFactor);

        Vector3 axis = GetAxis(m_SensitivityHorizontal, m_SensitivityVertical, sencitivityScale);

        Quaternion rotationDeltaY = Quaternion.Euler(new Vector3(0, axis.x, 0));
        Quaternion rotationDeltaX = Quaternion.Euler(new Vector3(-axis.y, 0, 0));

        Plane plane = new Plane(Vector3.up, 0);

        Bounds? bounds = null;

        foreach (var entity in m_ShipColliderFilter)
        {
            ref var collider = ref m_PoolProjectileCollider.Get(entity);
            ref var root = ref m_PoolRoot.Get(entity);
            if (root.entity.Unpack(m_World, out int rootEntity))
            {
                if (m_PoolDeadTag.Has(rootEntity)) continue;
                if (m_PoolTeam.Has(rootEntity))
                {
                    ref var team = ref m_PoolTeam.Get(rootEntity);
                    if (team.id == 0)
                    {
                        if (!bounds.HasValue) bounds = new Bounds(collider.collider.bounds.center, collider.collider.bounds.size);
                        else
                        {
                            var b = bounds.Value;
                            b.Encapsulate(collider.collider.bounds);
                            bounds = b;
                        }
                    }
                }
            }
        }

        if (bounds != null) m_Bounds = bounds.Value;

        foreach (var item in m_ZoomFilter)
        {
            ref var view = ref m_PoolView.Get(item);

            Quaternion rotation = view.rotation;
            rotation = rotationDeltaY * rotation;
            rotation = rotation * rotationDeltaX;

            //Quaternion rotationY = QuaternionUtility.ClampAxisAngle(QuaternionUtility.ProjectOnPlane(rotation, Quaternion.identity, Vector3.up), Quaternion.identity, -45, 45, Vector3.up);
            //Quaternion rotationX = QuaternionUtility.ClampAxisAngle(QuaternionUtility.ProjectOnPlane(rotation, rotationY, Vector3.right), rotationY, m_TiltAngleMin, m_TiltAngleMax, Vector3.right);

            //rotation = QuaternionUtility.ClampAngleY(rotation, -45, 45);

            rotation = QuaternionUtility.ClampAngleYX(rotation, -45, 45, m_TiltAngleMin, m_TiltAngleMax); //QuaternionUtility.ClampAngleX(rotation, m_TiltAngleMin, m_TiltAngleMax);

            view.rotation = rotation;
            //view.rotation *= rotationX;

            view.fieldOfView = fieldOfView;

            Ray ray = new Ray(view.position, view.rotation * Vector3.forward);
            Vector3? point = null;

            if (m_IsZoom)
            {
                List<RaycastHit> hits = new List<RaycastHit>();

                foreach (var entity in m_ShipColliderFilter)
                {
                    ref var collider = ref m_PoolProjectileCollider.Get(entity);
                    ref var root = ref m_PoolRoot.Get(entity);
                    if (root.entity.Unpack(m_World, out int rootEntity))
                    {
                        if (m_PoolTeam.Has(rootEntity))
                        {
                            ref var team = ref m_PoolTeam.Get(rootEntity);
                            if (team.id != 0)
                            {
                                if (collider.collider.Raycast(ray, out RaycastHit hit, 10000))
                                {
                                    hits.Add(hit);
                                }
                            }
                        }
                    }
                }

                if (hits.Count > 0)
                {
                    hits = hits.OrderBy(x => x.distance).ToList();
                    point = hits.First().point;
                }

                if (point == null)
                    if (plane.Raycast(ray, out float hitDistance))
                    {
                        point = ray.GetPoint(hitDistance);
                    }
            }


            foreach (var entity in m_AbilityFilter)
            {
                ref var abilityState = ref m_PoolAbilityState.Get(entity);
                abilityState.isZoom = m_IsZoom;

                if (point.HasValue)
                {
                    ref var ability = ref m_PoolAbility.Get(entity);
                    if (abilityState.isPerfrom)
                    {
                        var newEntity = m_World.NewEntity();
                        ref var newAbility = ref m_PoolAbility.Add(newEntity);
                        newAbility.id = ability.id;
                        ref var newAbilityTragetArea = ref m_PoolAbilityTargetArea.Add(newEntity);
                        newAbilityTragetArea.point = point.Value;
                    }

                    ref var abilityAim = ref m_PoolAbilityAim.Get(entity);
                    abilityAim.point = point.Value;


                }
            }

            if (point != null) m_Point = point.Value;
        }

        foreach (var entity in m_AbilityGroupFilter)
        {
            ref var abilityState = ref m_PoolAbilityState.Get(entity);
            ref var abilityAim = ref m_PoolAbilityAim.Get(entity);
            ref var abilityGroup = ref m_PoolAbilityGroup.Get(entity);


            if (abilityGroup.entities != null)
            {
                for (int i = 0; i < abilityGroup.entities.Count; i++)
                {
                    if (abilityGroup.entities[i].Unpack(m_World, out int childEntity))
                    {
                        ref var childAbilityState = ref m_PoolAbilityState.Get(childEntity);
                        ref var childAbilityAim = ref m_PoolAbilityAim.Get(childEntity);
                        childAbilityState.isZoom = abilityState.isZoom;
                        if (abilityState.isZoom) childAbilityAim.point = abilityAim.point;
                    }
                }
            }
        }

        foreach (var battleDataEntity in m_BattleDataFilter)
        {
            ref var battleData = ref m_PoolBattleData.Get(battleDataEntity);

            if(m_IsStarted!= battleData.isStarted)
            {
                m_IsStarted = battleData.isStarted;

                if (m_IsStarted && battleData.isShooter)
                {
                    ZoomToggle(true);
                }
            }


            if (m_IsEnded != battleData.isEnded)
            {
                m_IsEnded = battleData.isEnded;
                m_IsZoom = false;
            }

            if (battleData.isStarted)
            {
                //if (!m_IsZoomedInStart)
                //{
                //    if (battleData.levelData.startInZoom)
                //    {
                //        ZoomToggle(true);
                //        m_IsZoomedInStart = true;
                //    }
                //}

                //if (battleData.isEnded)
                //{
                //    m_IsZoom = false;
                //    m_IsZoomedInStart = false;
                //}
            }
        }
    }


}
