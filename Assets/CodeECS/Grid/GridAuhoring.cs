
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public class GridAuhoring : MonoBehaviour
{
    public Vector2 center = Vector2.zero;
    public Vector2 scale = Vector2.one;
    public RectInt[] rects = new RectInt[1];
    //public Vector2 test;
    private void OnDrawGizmos()
    {
        using (new GizmosScope(transform.localToWorldMatrix))
        {
            for (int i = 0; i < rects.Length; i++)
            {
                var rect = rects[i];
                for (int y = 0; y < rect.size.y; y++)
                {
                    for (int x = 0; x < rect.size.x; x++)
                    {
                        Gizmos.DrawWireCube(new Vector3(rect.position.x * scale.x + center.x + x * scale.x, 0, rect.position.y * scale.y + center.y + y * scale.y), new Vector3(scale.x, 0, scale.y));
                    }
                }
            }

        }
    }
}

public class GridBaker : Baker<GridAuhoring>
{
    public override void Bake(GridAuhoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new GridScale() { value = authoring.scale });
        AddComponent(entity, new GridCenter() { value = authoring.center });

        var gridBuffer = AddBuffer<GridRect>(entity);
        foreach (var item in authoring.rects)
        {
            gridBuffer.Add(new GridRect() { position = new int2(item.position.x, item.position.y), size = new int2(item.size.x, item.size.y) });
        }
       
        quaternion rotation = quaternion.RotateX(math.radians(90));
        float4x4 gridMatrix = float4x4.TRS(math.rotate(rotation, new float3(authoring.center, 0)), rotation, new float3(authoring.scale, 0));
        AddComponent(entity, new GridMatrix() { value = gridMatrix });
    }
}
