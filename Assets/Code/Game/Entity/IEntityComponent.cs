using System.Collections;
using UnityEngine;
using Leopotam.EcsLite;

public interface IEntityAuthoring
{
    public bool isEnable { get; }
    public void Bake(int entity, EcsWorld ecsWorld); 
}

//public interface IEntityEnableComponent
//{
//    void OnEntityEnable(Entity entity);
//}

//public interface IEntityDisableComponent
//{
//    void OnEntityDisable(Entity entity);
//}