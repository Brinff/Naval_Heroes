using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Execute(EcsWorld world, IEcsSystems systems);
}

public interface ICommand<C>
{
    void Execute(EcsWorld world, IEcsSystems systems, C context);
}
