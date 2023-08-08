using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[Flags]
public enum PointerHandlerEvent
{
    None = 0, Down = 1, Up = 2, Click = 4, Update = 8
}


public class PointerHandlerAuthoring : MonoBehaviour
{
    public PointerHandlerEvent handle = PointerHandlerEvent.Click;
}

public class PointerHandlerBaker : Baker<PointerHandlerAuthoring>
{
    public override void Bake(PointerHandlerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent<PointerHandlerTag>(entity);

        if (authoring.handle.HasFlag(PointerHandlerEvent.Down))
        {
            AddComponent<PointerDownEvent>(entity);
            SetComponentEnabled<PointerDownEvent>(entity, false);
        }

        if (authoring.handle.HasFlag(PointerHandlerEvent.Up))
        {
            AddComponent<PointerUpEvent>(entity);
            SetComponentEnabled<PointerUpEvent>(entity, false);
        }

        if (authoring.handle.HasFlag(PointerHandlerEvent.Click))
        {
            AddComponent<PointerClickEvent>(entity);
            SetComponentEnabled<PointerClickEvent>(entity, false);
        }

        if (authoring.handle.HasFlag(PointerHandlerEvent.Update))
        {
            AddComponent<PointerUpdateEvent>(entity);
            SetComponentEnabled<PointerUpdateEvent>(entity, false);
        }
    }
}
