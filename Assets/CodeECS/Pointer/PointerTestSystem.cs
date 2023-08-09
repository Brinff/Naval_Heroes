using Game.Pointer.Data;
using Game.Pointer.Events;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public partial struct PointerTestSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (pointerEvent, entity) in SystemAPI.Query<RefRO<PointerBeginDragEvent>>().WithAll<PointerHandlerTag>().WithEntityAccess())
        {
            Debug.Log($"Begin drag entity: {state.EntityManager.GetName(entity)}");
            foreach (var (pointerId, pointerDragEntity) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerDragEntity>>())
            {
                if (PointerHelper.HasFlag(pointerEvent.ValueRO.value, pointerId.ValueRO.value))
                {
                    Debug.Log($"Drag data: {state.EntityManager.GetName(pointerDragEntity.ValueRO.value)}");
                }
            }
        }


        foreach (var (pointerEvent, entity) in SystemAPI.Query<RefRO<PointerUpdateDragEvent>>().WithAll<PointerHandlerTag>().WithEntityAccess())
        {
            Debug.Log($"Update drag on entity: {state.EntityManager.GetName(entity)}");
            foreach (var (pointerId, pointerDragEntity) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerDragEntity>>())
            {
                if (PointerHelper.HasFlag(pointerEvent.ValueRO.value, pointerId.ValueRO.value))
                {
                    Debug.Log($"Drop data: {state.EntityManager.GetName(pointerDragEntity.ValueRO.value)}");
                }
            }
        }

        foreach (var (pointerEvent, entity) in SystemAPI.Query<RefRO<PointerEndDragEvent>>().WithAll<PointerHandlerTag>().WithEntityAccess())
        {
            Debug.Log($"End drag entity: {state.EntityManager.GetName(entity)}");
            foreach (var (pointerId, pointerDragEntity) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerDragEntity>>())
            {
                if (PointerHelper.HasFlag(pointerEvent.ValueRO.value, pointerId.ValueRO.value))
                {
                    Debug.Log($"Drag data: {state.EntityManager.GetName(pointerDragEntity.ValueRO.value)}");
                }
            }
        }

        foreach (var (pointerEvent, entity) in SystemAPI.Query<RefRO<PointerDropEvent>>().WithAll<PointerHandlerTag>().WithEntityAccess())
        {
            Debug.Log($"Drop on entity: {state.EntityManager.GetName(entity)}");
            foreach (var (pointerId, pointerDropEntity) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerDropEntity>>())
            {
                if (PointerHelper.HasFlag(pointerEvent.ValueRO.value, pointerId.ValueRO.value))
                {
                    Debug.Log($"Drop data: {state.EntityManager.GetName(pointerDropEntity.ValueRO.value)}");
                }
            }
        }

        //foreach (var (pointerEvent, entity) in SystemAPI.Query<RefRO<PointerDownEvent>>().WithAll<PointerHandlerTag>().WithEntityAccess())
        //{
        //    //Debug.Log($"Down on entity: {state.EntityManager.GetName(entity)}");
        //    foreach (var id in SystemAPI.Query<RefRO<PointerId>>())
        //    {
        //        if (pointerEvent.ValueRO.value.HasFlag(id.ValueRO.value))
        //        {
        //            Debug.Log($"Down point: {id.ValueRO.value} {state.EntityManager.GetName(entity)}");
        //        }
        //    }
        //}

        //foreach (var (pointerEvent, entity) in SystemAPI.Query<RefRO<PointerUpEvent>>().WithAll<PointerHandlerTag>().WithEntityAccess())
        //{
        //    //Debug.Log($"Up on entity: {state.EntityManager.GetName(entity)}");
        //    foreach (var id in SystemAPI.Query<RefRO<PointerId>>())
        //    {
        //        if (pointerEvent.ValueRO.value.HasFlag(id.ValueRO.value))
        //        {
        //            Debug.Log($"Up point: {id.ValueRO.value} {state.EntityManager.GetName(entity)}");
        //        }
        //    }
        //}

        //foreach (var (pointerEvent, entity) in SystemAPI.Query<RefRO<PointerClickEvent>>().WithAll<PointerHandlerTag>().WithEntityAccess())
        //{
        //    //Debug.Log($"Click on entity: {state.EntityManager.GetName(entity)}");
        //    foreach (var id in SystemAPI.Query<RefRO<PointerId>>())
        //    {
        //        if (pointerEvent.ValueRO.value.HasFlag(id.ValueRO.value))
        //        {
        //            Debug.Log($"Click point: {id.ValueRO.value} {state.EntityManager.GetName(entity)}");
        //        }
        //    }        
        //}

        //foreach (var (updateEvent, entity) in SystemAPI.Query<PointerUpdateEvent>().WithEntityAccess())
        //{
        //    Debug.Log($"Update {state.EntityManager.GetName(entity)}");
        //}

        //foreach (var (downEvent, position, delta, entity) in SystemAPI.Query<PointerUpEvent, PointerPosition, PointerDelta>().WithEntityAccess())
        //{
        //    Debug.Log($"Up {state.EntityManager.GetName(entity)} position: {position.value} delta: {delta.value}");
        //}

        //foreach (var (downEvent, entity) in SystemAPI.Query<PointerClickEvent>().WithEntityAccess())
        //{
        //    Debug.Log($"Click {state.EntityManager.GetName(entity)}");
        //}
    }
}
