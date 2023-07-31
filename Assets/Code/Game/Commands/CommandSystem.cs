using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Paterns;
using Leopotam.EcsLite;
public class CommandSystem : Singleton<CommandSystem>, IEcsRunSystem, IEcsGroupUpdateSystem, IEcsDestroySystem
{

    public delegate void Command(EcsWorld world, IEcsSystems systems);

    private Queue<Command> m_QueueCommands = new Queue<Command>();



    public void Execute<T, C>(C context) where T : ICommand<C>
    {
        var command = GetComponentInChildren<T>();
        m_QueueCommands.Enqueue((EcsWorld world, IEcsSystems systems) => { Debug.Log(command); command.Execute(world, systems, context); });
    }

    public void Execute<T>() where T : ICommand
    {
        var command = GetComponentInChildren<T>();
        m_QueueCommands.Enqueue((EcsWorld world, IEcsSystems systems) => { Debug.Log(command); command.Execute(world, systems); });
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
}
