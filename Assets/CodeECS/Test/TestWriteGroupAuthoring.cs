using Unity.Entities;
using UnityEngine;

public class TestWriteGroupAuthoring : MonoBehaviour
{

}

public class TestWriteGroupBaker : Baker<TestWriteGroupAuthoring>
{
    public override void Bake(TestWriteGroupAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new TestA());
        AddComponent(entity, new TestB());
    }
}