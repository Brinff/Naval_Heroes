
using Unity.Entities;


public struct TestA : IComponentData
{
    public int value;
}


[WriteGroup(typeof(TestA))]
public struct TestB : IComponentData
{
    public int value;
}

[WriteGroup(typeof(TestA))]
public struct TestC : IComponentData
{
    public int value;
}