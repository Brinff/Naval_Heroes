using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Paterns;
using Leopotam.EcsLite;
using System;
using System.Linq;

public class CommandSystem : Singleton<CommandSystem>, IEcsRunSystem, IEcsPostRunSystem, IEcsDestroySystem, IEcsGroup<Update>
{

    public delegate void Command(EcsWorld world, IEcsSystems systems);

    private Queue<Command> m_QueueCommands = new Queue<Command>();
    private Queue<Type> m_QueueCommandsType = new Queue<Type>();

    public bool HasCommand<T>()
    {
        var type = typeof(T);
        return m_QueueCommandsType.Any(x => x == type);
    }

    public void Execute<T, C>(C context) where T : ICommand<C>
    {
        var command = GetComponentInChildren<T>();
        m_QueueCommands.Enqueue((EcsWorld world, IEcsSystems systems) => { Debug.Log(command); command.Execute(world, systems, context); });
        m_QueueCommandsType.Enqueue(typeof(T));
    }

    public void Execute<T>() where T : ICommand
    {
        var command = GetComponentInChildren<T>();
        m_QueueCommands.Enqueue((EcsWorld world, IEcsSystems systems) => { Debug.Log(command); command.Execute(world, systems); });
        m_QueueCommandsType.Enqueue(typeof(T));
    }

    public void Run(IEcsSystems systems)
    {
        if (m_QueueCommands.TryDequeue(out Command command))
        {
            command.Invoke(systems.GetWorld(), systems);

        }
    }

    public void Destroy(IEcsSystems systems)
    {
        m_QueueCommands.Clear();
    }

    public void PostRun(IEcsSystems systems)
    {
        if (m_QueueCommandsType.Count > 0)
            m_QueueCommandsType.Dequeue();
    }
}
