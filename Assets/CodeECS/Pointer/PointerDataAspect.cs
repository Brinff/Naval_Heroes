using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

public readonly partial struct PointerDataAspect : IAspect
{
    //readonly RefRW<PointerId> pointerId;

    readonly RefRW<PointerPosition> pointerPosition;
    readonly RefRW<PointerDelta> pointerDelta;
    readonly RefRW<PointerRay> pointerRay;
    //readonly RefRW<PointerPressEntity> pointerPressEntity;
    //readonly RefRW<PointerDragEntity> pointerDragEntity;


    public void UpdateData(PointerEventData pointerEventData, Camera camera)
    {
        pointerPosition.ValueRW.value = pointerEventData.position;
        pointerDelta.ValueRW.value = pointerEventData.delta;
        var ray = camera.ScreenPointToRay(pointerEventData.position);
        pointerRay.ValueRW.origin = ray.origin;
        pointerRay.ValueRW.direction = ray.direction;
    }
}
