using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public partial struct PointerTestSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
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
