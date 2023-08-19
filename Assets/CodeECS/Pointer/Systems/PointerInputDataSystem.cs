
using Game.Pointer.Data;
using Game.Pointer.Events;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Game.Pointer.Systems
{
    [UpdateInGroup(typeof(PointerGroup), OrderFirst = true)]
    public partial class PointerInputDataSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            PointerInputHandler.OnDown += OnPointerDown;
            PointerInputHandler.OnUp += OnPointerUp;
        }

        protected override void OnStopRunning()
        {
            PointerInputHandler.OnDown -= OnPointerDown;
            PointerInputHandler.OnUp -= OnPointerUp;
        }

        private EntityQuery entityPointerDataQuery;
        //private EntityQuery entityPointerDownQuery;
        //private EntityQuery entityPointerUpQuery;

        private BeginSimulationEntityCommandBufferSystem beginSimulationEntity;
        private EndSimulationEntityCommandBufferSystem endSimulationEntity;

        protected override void OnCreate()
        {
            beginSimulationEntity = World.GetOrCreateSystemManaged<BeginSimulationEntityCommandBufferSystem>();
            endSimulationEntity = World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>();
            entityPointerDataQuery = SystemAPI.QueryBuilder().WithAll<PointerId>().Build();
            //entityPointerDownQuery = new EntityQueryBuilder(Allocator.Persistent).WithAll<PointerDownEvent>().Build(this);
            //entityPointerUpQuery = new EntityQueryBuilder(Allocator.Persistent).WithAll<PointerUpEvent>().Build(this);
        }

        //protected override void OnDestroy()
        //{
        //    entityPointerDataQuery.Dispose();
        //}


        private Entity GetOrCreatePointer(EntityCommandBuffer entityCommandBuffer, PointerEventData eventData)
        {
            using (var ids = entityPointerDataQuery.ToComponentDataArray<PointerId>(Allocator.Temp))
            {
                using (var entities = entityPointerDataQuery.ToEntityArray(Allocator.Temp))
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (ids[i].value == PointerHelper.GetPointerFromId(eventData.pointerId))
                        {
                            var pointerData = EntityManager.GetComponentObject<PointerData>(entities[i]);
                            pointerData.value = eventData;
                            return entities[i];
                        }
                    }
                    return CreatePointer(entityCommandBuffer, eventData);
                }
            }
        }



        private Entity CreatePointer(EntityCommandBuffer entityCommandBuffer, PointerEventData eventData)
        {
            var entity = entityCommandBuffer.CreateEntity();
            var id = PointerHelper.GetPointerFromId(eventData.pointerId);
            entityCommandBuffer.SetName(entity, $"[Pointer Id {id}]");
            entityCommandBuffer.AddComponent(entity, new PointerId() { value = id });
            entityCommandBuffer.AddComponent(entity, new PointerData() { value = eventData });
            entityCommandBuffer.AddComponent<PointerPosition>(entity);
            entityCommandBuffer.AddComponent<PointerDelta>(entity);
            entityCommandBuffer.AddComponent<PointerPressEntity>(entity);
            entityCommandBuffer.AddComponent<PointerDragEntity>(entity);
            entityCommandBuffer.AddComponent<PointerDropEntity>(entity);
            entityCommandBuffer.AddComponent<PointerRay>(entity);
            entityCommandBuffer.AddComponent<PointerFirstHoveredEntity>(entity);
            entityCommandBuffer.AddBuffer<PointerHoveredEntity>(entity);
            entityCommandBuffer.AddComponent<PointerDownEvent>(entity);
            entityCommandBuffer.SetComponentEnabled<PointerDownEvent>(entity, false);
            entityCommandBuffer.AddComponent<PointerUpEvent>(entity);
            entityCommandBuffer.SetComponentEnabled<PointerUpEvent>(entity, false);
            return entity;
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            var beginEcb = beginSimulationEntity.CreateCommandBuffer();
            var entity = GetOrCreatePointer(beginEcb, eventData);
            beginEcb.SetComponentEnabled<PointerDownEvent>(entity, true);
        }

        private void OnPointerUp(PointerEventData eventData)
        {
            var beginEcb = beginSimulationEntity.CreateCommandBuffer();
            var entity = GetOrCreatePointer(beginEcb, eventData);
            beginEcb.SetComponentEnabled<PointerUpEvent>(entity, true);
        }

        protected override void OnUpdate()
        {

            var endEcb = endSimulationEntity.CreateCommandBuffer();

            foreach (var (downEvent, entity) in SystemAPI.Query<PointerDownEvent>().WithEntityAccess())
            {
                endEcb.SetComponent(entity, new PointerDownEvent());
                endEcb.SetComponentEnabled<PointerDownEvent>(entity, false);
            }

            foreach (var (upEvent, entity) in SystemAPI.Query<PointerUpEvent>().WithEntityAccess())
            {
                endEcb.SetComponent(entity, new PointerUpEvent());
                endEcb.SetComponentEnabled<PointerUpEvent>(entity, false);
            }

            foreach (var (clickEvent, entity) in SystemAPI.Query<PointerClickEvent>().WithEntityAccess())
            {
                endEcb.SetComponent(entity, new PointerClickEvent());
                endEcb.SetComponentEnabled<PointerClickEvent>(entity, false);
            }

            foreach (var (dragEvent, entity) in SystemAPI.Query<PointerBeginDragEvent>().WithEntityAccess())
            {
                endEcb.SetComponent(entity, new PointerBeginDragEvent());
                endEcb.SetComponentEnabled<PointerBeginDragEvent>(entity, false);
            }

            foreach (var (endDragEvent, entity) in SystemAPI.Query<PointerEndDragEvent>().WithEntityAccess())
            {
                endEcb.SetComponent(entity, new PointerEndDragEvent());
                endEcb.SetComponentEnabled<PointerEndDragEvent>(entity, false);
            }

            foreach (var (dropEvent, entity) in SystemAPI.Query<PointerDropEvent>().WithEntityAccess())
            {
                endEcb.SetComponent(entity, new PointerDropEvent());
                endEcb.SetComponentEnabled<PointerDropEvent>(entity, false);
            }

            foreach (var (endDragEvent, entity) in SystemAPI.Query<PointerEnterEvent>().WithEntityAccess())
            {
                endEcb.SetComponent(entity, new PointerEnterEvent());
                endEcb.SetComponentEnabled<PointerEnterEvent>(entity, false);
            }

            foreach (var (dropEvent, entity) in SystemAPI.Query<PointerExitEvent>().WithEntityAccess())
            {
                endEcb.SetComponent(entity, new PointerExitEvent());
                endEcb.SetComponentEnabled<PointerExitEvent>(entity, false);
            }

            //using (var entities = entityPointerDownQuery.ToEntityArray(Allocator.Temp))
            //{
            //    foreach (var entity in entities)
            //    {
            //        endEcb.SetComponentEnabled<PointerDownEvent>(entity, false);
            //    }
            //}

            //using (var entities = entityPointerUpQuery.ToEntityArray(Allocator.Temp))
            //{
            //    foreach (var entity in entities)
            //    {
            //        endEcb.SetComponentEnabled<PointerUpEvent>(entity, false);
            //    }
            //}

            Camera camera = Camera.main;
            foreach (var (dataAspect, data) in SystemAPI.Query<PointerDataAspect, PointerData>())
            {
                dataAspect.UpdateData(data.value, camera);
            }
        }
    }
}
