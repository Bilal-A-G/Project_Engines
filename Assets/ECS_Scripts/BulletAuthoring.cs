using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace ECS_Scripts
{
    public class BulletAuthoring : MonoBehaviour
    {
        private class BulletBaker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Bullet
                {
                    lifeTime = 3
                });
                //AddComponent<URPMaterialPropertyBaseColor>(entity);
            }
        }
    }

    public struct Bullet : IComponentData
    {
        public float lifeTime;
        public Entity Self;
    }
}


