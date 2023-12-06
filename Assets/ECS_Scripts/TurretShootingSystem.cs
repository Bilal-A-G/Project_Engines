using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Collider = Unity.Physics.Collider;
using Random = UnityEngine.Random;

namespace ECS_Scripts
{
    [UpdateAfter(typeof(TurretRotationSystem))]
    public partial struct TurretShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        { 
            state.RequireForUpdate<PlayerShootInput>();
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.GetSingleton<PlayerShootInput>().IsShooting) return;
            

            //Debug.Log("Trying to shoot");
            Vector3 up = new Vector3(0, 0, 20);
            foreach (var (turret, localToWorld) in SystemAPI.Query<Turret, RefRO<LocalToWorld>>())
            { 
                if(turret.Health <= 0) continue;
               Quaternion original = localToWorld.ValueRO.Rotation;
                for (uint i = 0; i < turret.NumProjectiles; ++i)
                {
                    Entity instance = state.EntityManager.Instantiate(turret.Projectile);
                    Quaternion random = original * Quaternion.Euler(Random.Range(-turret.Angle, turret.Angle), Random.Range(-turret.Angle, turret.Angle), 0);

                    state.EntityManager.SetComponentData(instance, new LocalTransform
                    {
                        Position = SystemAPI.GetComponent<LocalToWorld>(turret.FirePoint).Position,
                        Rotation = random,
                        Scale = SystemAPI.GetComponent<LocalTransform>(turret.Projectile).Scale
                    });
                
                    state.EntityManager.SetComponentData(instance, new PhysicsVelocity
                    {
                        Linear = random * up * Random.Range(turret.ForceMin, turret.ForceMax)
                    });


                    float3 color = new float3(80,87,37) * Random.Range(0.7f, 1.3f) / 255;
                    
                    state.EntityManager.SetComponentData(instance, new URPMaterialPropertyBaseColor()
                    {
                        Value = new float4(color.xyz, 1)
                    });

                    
                    state.EntityManager.SetComponentData(instance, new Bullet
                    {
                        lifeTime = UnityEngine.Random.Range(2f, 4),
                        Self = instance
                    });
                }

                //Debug.Log("Spawned Projectile: + " + localToWorld.ValueRO.Up * 20.0f);

                /*
                state.EntityManager.SetComponentData(instance, new URPMaterialPropertyBaseColor
                {
                    Value = turret.Color
                }); */
            }
        }  
    }
    /*
    [BurstCompile]
    [StructLayout(LayoutKind.Auto)]
    public partial struct TurretShoot : IJobEntity
    {
        public float DeltaTime;
        [BurstCompile]
        private void Execute(ref LocalToWorld transform, in TurretAspect input)
        {
            
        }
    }*/
}
