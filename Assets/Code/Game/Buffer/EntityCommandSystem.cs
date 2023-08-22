using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI;

public class EntityCommandBuffer : IDisposable
{
    [SerializeField]
    private EcsWorld m_World;

    public EntityCommandBuffer(EcsWorld world)
    {
        m_World = world;
        m_Commands = new Queue<Command>();
    }

    public delegate void Command(EcsWorld world);

    private Queue<Command> m_Commands;

    public void AddComponent<T>(int entity, T value) where T : struct
    {
        var type = typeof(T);
        var packEntity = m_World.PackEntity(entity);
        m_Commands.Enqueue((EcsWorld world) =>
        {
            if (packEntity.Unpack(world, out int targetEntity))
            {
                world.GetPoolByType(type).AddRaw(targetEntity, value);
            }
        });
    }

    public void SetComponent<T>(int entity, T value) where T : struct
    {
        var type = typeof(T);
        var packEntity = m_World.PackEntity(entity);
        m_Commands.Enqueue((EcsWorld world) =>
        {
            if (packEntity.Unpack(world, out int targetEntity))
            {
                world.GetPoolByType(type).SetRaw(targetEntity, value);
            }
        });
    }

    public void RemoveComponent<T>(int entity) where T : struct
    {
        var type = typeof(T);
        var packEntity = m_World.PackEntity(entity);
        m_Commands.Enqueue((EcsWorld world) =>
        {
            if (packEntity.Unpack(world, out int targetEntity))
            {
                world.GetPoolByType(type).Del(targetEntity);
            }
        });
    }

    public void DelEntity(int entity)
    {
        var packEntity = m_World.PackEntity(entity);
        m_Commands.Enqueue((EcsWorld world) =>
        {
            if (packEntity.Unpack(world, out int targetEntity))
            {
                world.DelEntity(targetEntity);
            }
        });
    }

    public void Playback()
    {
        while (m_Commands.TryDequeue(out Command command))
        {
            command.Invoke(m_World);
        }
        m_Commands.Clear();
    }

    public void Dispose()
    {
        m_World = null;
        m_Commands = null;
    }
}

public class EntityCommandSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private Queue<EntityCommandBuffer> m_CommandBuffers = new Queue<EntityCommandBuffer>();

    private EcsWorld m_World;

    public EntityCommandBuffer CreateBuffer()
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(m_World);
        m_CommandBuffers.Enqueue(entityCommandBuffer);
        return entityCommandBuffer;
    }


    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
    }



    public void Run(IEcsSystems systems)
    {
        while (m_CommandBuffers.TryDequeue(out EntityCommandBuffer command))
        {
            command.Playback();
        }
    }
}
