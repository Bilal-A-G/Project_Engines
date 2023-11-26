using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
   private class EnemyBaker : Baker<EnemyAuthoring>
   {
      public override void Bake(EnemyAuthoring authoring)
      {
         
         var entity = GetEntity(TransformUsageFlags.Dynamic);
         AddComponent<EnemyData>(entity);
      }
      
      public struct EnemyData : IComponentData
      {
         public float Speed; // Speed and Direction
         public float Damage;
         public float Health;
      }
   }
}
