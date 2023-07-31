
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScriptBehaviour : MonoBehaviour
{
    [Button]
    public void Launch(EcsPackedEntityWithWorld playerEntity)
    {
        if (playerEntity.Unpack(out EcsWorld world, out int entity))
        {
            ref var transform = ref world.GetPool<TransformComponent>().Get(entity);
            AlignLocation(transform.transform);
            SpawnOthers(world);
        }
    }

    private void SpawnOthers(EcsWorld world)
    {
        var spawnPointOthers = transform.GetComponentsInChildren<ScriptSpawnPointOther>();
        int index = 0;
        foreach (var item in spawnPointOthers)
        {
            var instance = Instantiate(item.entityData.prefab, item.transform.position, item.transform.rotation);
            instance.name = $"{item.entityData.prefab.name} {index}";
            var entity = world.Bake(instance);
            world.GetPool<AITag>().Add(entity);
            ref var team = ref world.GetPool<TeamComponent>().Get(entity);
            world.GetPool<AITargetComponent>().Add(entity);
            world.GetPool<AIMoveToTargetComponent>().Add(entity);
            world.GetPool<NewEntityTag>().Add(entity);

            IEntityAuthoring[] entityAuthorings = item.GetComponents<IEntityAuthoring>();
            foreach (var entityAuthoring in entityAuthorings)
            {
                entityAuthoring.Bake(entity, world);
            }

            index++;

            team.id = item.team;
        }
    }

    private void AlignLocation(Transform playerEntityTransform)
    {
        var spawnPlayerPoint = GetComponentInChildren<ScriptSpawnPointPlayer>();

        Quaternion rotation = (playerEntityTransform.rotation * Quaternion.Inverse(spawnPlayerPoint.transform.rotation)) * transform.rotation;
        Vector3 eulerAngles = rotation.eulerAngles;
        eulerAngles.x = 0;
        eulerAngles.z = 0;
        rotation.eulerAngles = eulerAngles;


        transform.rotation = rotation;

        Vector3 position = playerEntityTransform.position - transform.TransformVector(spawnPlayerPoint.transform.localPosition);
        position.y = 0;

        transform.position = position;
    }


}
