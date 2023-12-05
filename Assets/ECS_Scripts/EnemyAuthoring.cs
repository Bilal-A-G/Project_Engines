using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS_Scripts
{
   public class EnemyAuthoring : MonoBehaviour
   {
      [SerializeField] private EnemySO stats;
      private class EnemyBaker : Baker<EnemyAuthoring>
      {
         public override void Bake(EnemyAuthoring authoring)
         {
         
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyData()
            {
               Speed = authoring.stats._speed,
               Damage = authoring.stats._damage,
               Health = authoring.stats._health,
            });
         }
      }
   }
   public struct EnemyData : IComponentData
   {
      public float Speed; // Speed and Direction
      public float Damage;
      public float Health;
      public float3 Direction;
      public Entity Self;
   }
}
