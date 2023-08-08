using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public interface ISystemUpdate : ISystem
{
    new void OnUpdate(ref SystemState state);
}

public interface ISystemCreate : ISystem
{
    new void OnCreate(ref SystemState state);
}

public static class SystemUtility
{

}
