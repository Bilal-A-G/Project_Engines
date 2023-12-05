using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS_Scripts
{
    [BurstCompile]
    public partial struct EnemyControllerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Turret>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            var enemy = new EnemyJob()
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                DeltaTime = SystemAPI.Time.DeltaTime,
                tur = SystemAPI.GetSingleton<Turret>()
            };

            enemy.Schedule(); 
        }
    }
    
    [StructLayout(LayoutKind.Auto)]
    [BurstCompile]
    public partial struct EnemyJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;
        public Turret tur;
        private void Execute(Entity self, ref EnemyData enemy, ref LocalTransform transform)
        {
            if (math.distancesq(transform.Position, float3.zero) < 16)
            {
                //Hurt the player?

                tur.Health -= 1;
                
                ECB.DestroyEntity(self);
                return;
            }
            //Move
            transform.Position += enemy.Direction * DeltaTime;
        }
    }
}
