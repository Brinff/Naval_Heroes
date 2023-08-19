using Unity.Entities;
using UnityEngine;

using Game.Money.Components;

namespace Game.Money.Authoring
{
    [AddComponentMenu("Game/Money/Money Soft")]
    public class MoneySoftAuthoring : MonoBehaviour
    {
        public int moneySoft;
    }

    public class MoneySoftBaker : Baker<MoneySoftAuthoring>
    {
        public override void Bake(MoneySoftAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new MoneySoft() { value = authoring.moneySoft });
        }
    }
}