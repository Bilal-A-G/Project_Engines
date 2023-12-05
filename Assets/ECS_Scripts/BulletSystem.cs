using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics.Systems;

namespace ECS_Scripts
{
    [UpdateInGroup(typeof(PhysicsSimulationGroup))]
    [BurstCompile]
    public partial struct BulletSystem : ISystem
    {    
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        }
        [BurstCompile]

        public void OnUpdate(ref SystemState state)
        {
           
            var ecbSingleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            
            var bulletJob = new BulletJob()
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                DeltaTime = SystemAPI.Time.DeltaTime
            };

            bulletJob.Schedule(); 
        }
    }

    [StructLayout(LayoutKind.Auto)]
    [BurstCompile]
    public partial struct BulletJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public float DeltaTime;
        private void Execute(Entity entity, ref Bullet bullet)
        {
            bullet.lifeTime -= DeltaTime;
            if(bullet.lifeTime < 0) ECB.DestroyEntity(entity);
        }
    }
}