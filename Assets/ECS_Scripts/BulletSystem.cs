using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Entities;

namespace ECS_Scripts
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct BulletSystem : ISystem
    {    
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }
        [BurstCompile]

        public void OnUpdate(ref SystemState state)
        {
           
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            var bulletJob = new BulletJob()
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                DeltaTime = SystemAPI.Time.DeltaTime
            };

            bulletJob.Schedule(); 
        }
    }

    [StructLayout(LayoutKind.Auto)]
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