using Game.Pointer.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEditor.PackageManager;
using UnityEngine;

[Flags]
public enum PointerHandlerEvent
{
    None = 0,
    Down = 1 << 0,
    Up = 1 << 1,
    Click = 1 << 2,
    Update = 1 << 3,
    BeginDrag = 1 << 4,
    UpdateDrag = 1 << 5,
    EndDrag = 1 << 6,
    Drop = 1 << 7
}


public class PointerHandlerAuthoring : MonoBehaviour
{
    public PointerHandlerEvent events = PointerHandlerEvent.Click;
}

public class PointerHandlerBaker : Baker<PointerHandlerAuthoring>
{
    public override void Bake(PointerHandlerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        Bake(this, entity, authoring.events);
    }

    public static void Bake(IBaker baker, Entity entity, PointerHandlerEvent events)
    {
        baker.AddComponent<PointerHandlerTag>(entity);

        if (events.HasFlag(PointerHandlerEvent.Down))
        {
            baker.AddComponent<PointerDownEvent>(entity);
            baker.SetComponentEnabled<PointerDownEvent>(entity, false);
        }

        if (events.HasFlag(PointerHandlerEvent.Up))
        {
            baker.AddComponent<PointerUpEvent>(entity);
            baker.SetComponentEnabled<PointerUpEvent>(entity, false);
        }

        if (events.HasFlag(PointerHandlerEvent.Click))
        {
            baker.AddComponent<PointerClickEvent>(entity);
            baker.SetComponentEnabled<PointerClickEvent>(entity, false);
        }

        if (events.HasFlag(PointerHandlerEvent.Update))
        {
            baker.AddComponent<PointerUpdateEvent>(entity);
            baker.SetComponentEnabled<PointerUpdateEvent>(entity, false);
        }

        if (events.HasFlag(PointerHandlerEvent.BeginDrag))
        {
            baker.AddComponent<PointerBeginDragEvent>(entity);
            baker.SetComponentEnabled<PointerBeginDragEvent>(entity, false);
        }

        if (events.HasFlag(PointerHandlerEvent.UpdateDrag))
        {
            baker.AddComponent<PointerUpdateDragEvent>(entity);
            baker.SetComponentEnabled<PointerUpdateDragEvent>(entity, false);
        }

        if (events.HasFlag(PointerHandlerEvent.EndDrag))
        {
            baker.AddComponent<PointerEndDragEvent>(entity);
            baker.SetComponentEnabled<PointerEndDragEvent>(entity, false);
        }

        if (events.HasFlag(PointerHandlerEvent.Drop))
        {
            baker.AddComponent<PointerDropEvent>(entity);
            baker.SetComponentEnabled<PointerDropEvent>(entity, false);
        }
    }
}
