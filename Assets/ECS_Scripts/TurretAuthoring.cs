using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace ECS_Scripts
{
    public class TurretAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform firePoint;
        [SerializeField] private int numProjectiles;
        [SerializeField] private float angle = 45;
        [SerializeField, Range(0,100)] private float minForce = 25;
        [SerializeField, Range(0,100)] private float maxForce = 45;
        class TurretBaker : Baker<TurretAuthoring>
        {
            public override void Bake(TurretAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
            
                AddComponent(entity, new Turret
                {
                    Projectile = GetEntity(authoring.projectile, TransformUsageFlags.Dynamic),
                    FirePoint =  GetEntity(authoring.firePoint, TransformUsageFlags.Dynamic),
                    NumProjectiles = authoring.numProjectiles,
                    Angle = authoring.angle,
                    ForceMin = authoring.minForce,
                    ForceMax = authoring.maxForce,
                    Health = 10
                });
            
                AddComponent<Shooting>(entity);
                AddComponent<PlayerMovementInput>(entity);
                AddComponent<PlayerShootInput>(entity);
                
                
            }
        }
    
    }

    public struct Turret : IComponentData
    {
        public Entity Projectile;
        public Entity FirePoint;
        public int NumProjectiles;
        public float Angle;
        public float ForceMin;
        public float ForceMax;
        public int Health;
    }

    public struct Shooting : IComponentData, IEnableableComponent
    {
    
    }


   
}