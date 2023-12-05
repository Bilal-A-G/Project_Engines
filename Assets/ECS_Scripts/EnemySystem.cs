using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ECS_Scripts
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [BurstCompile]
    public partial struct EnemySpawningSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //Query all enemies...
            float deltaTime = SystemAPI.Time.DeltaTime;
            //Debug.Log(deltaTime);
            foreach (var ( data, ramp) in SystemAPI.Query<RefRO<EnemySpawnerData>, RefRW<EnemySpawnerRamp>>())
            {
               
                float x = ramp.ValueRW.TimeToNext -= deltaTime;
                
                // float x = ramp.TimeToNext -= deltaTime;
                //Debug.Log(x);
                if (x > 0) continue;
                
                //Debug.Log("Huzzah");
                //Improve this by using an aspect...
                ramp.ValueRW.TimeToNext = Random.Range(data.ValueRO.MinSpawnTime, data.ValueRO.MaxSpawnTime);
                //ramp.TimeToNext = data.Random.NextFloat(data.MinSpawnTime, data.MaxSpawnTime);
                int k = ramp.ValueRO.NumToSpawn;
                //int k = ramp.NumToSpawn;
                if (++ramp.ValueRW.NumSpawned > k)
                    //if (++ramp.NumSpawned > k)
                {
                    ramp.ValueRW.NumToSpawn++;
                    //ramp.NumToSpawn++;
                    ramp.ValueRW.NumSpawned = 0;
                    //ramp.NumSpawned = 0;
                    Debug.Log("Resetting Rate");
                    k++;
                }
             
                float speed = math.min(1, k / 5);
                float3 targ = new float3(0, k, 0);
                for (int i = 0; i < k; ++i)
                {
                    Debug.Log("Spawning Batch");
                    var entity = state.EntityManager.Instantiate(data.ValueRO.EntityToSpawn);
                    //var entity = ECB.Instantiate(data.EntityToSpawn);

                    Vector2 temp = Random.insideUnitCircle.normalized * Random.Range(data.ValueRO.MinSpawnRange, data.ValueRO.MaxSpawnRange);
                    //float2 temp = data.Random.NextFloat2Direction() * data.Random.NextFloat(data.MinSpawnRange, data.MaxSpawnRange);
                    float3 position = new float3(temp.x, k, temp.y);

                    state.EntityManager.SetComponentData(entity, new LocalTransform
                    {
                        Position = position,
                        //Position = transform.Position + position,
                        Rotation = quaternion.identity,
                        Scale = k
                    });
                    state.EntityManager.SetComponentData(entity, new EnemyData()
                    {
                         Direction = math.normalize(targ - position),
                         Damage = 1,
                         Health = 10 * k,
                         Speed = speed,
                         Self = entity
                    });
                }
                
            }

            
            /*
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            if(!ecbSingleton.IsEmpty) Debug.Log(" :( ");
            
            EnemySpawnJob job = new EnemySpawnJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton
            };
                
            job.Run();*/
        }
    }

    [BurstCompile]
    [StructLayout(LayoutKind.Auto)]
    public partial struct EnemySpawnJob : IJobEntity
    {

        public float DeltaTime;
        public EntityCommandBuffer ECB;
    
        [BurstCompile]
        private void Execute(RefRO<LocalTransform> transform, RefRO<EnemySpawnerData> data, RefRW<EnemySpawnerRamp> ramp)
        //private void Execute(in LocalTransform transform, in EnemySpawnerData data, ref EnemySpawnerRamp ramp)
        {
            float x = ramp.ValueRW.TimeToNext -= DeltaTime;
           // float x = ramp.TimeToNext -= DeltaTime;
            if (x > 0) return;
            
          
            
            //Improve this by using an aspect...
            ramp.ValueRW.TimeToNext = data.ValueRO.Random.NextFloat(data.ValueRO.MinSpawnTime, data.ValueRO.MaxSpawnTime);
            //ramp.TimeToNext = data.Random.NextFloat(data.MinSpawnTime, data.MaxSpawnTime);
            int k = ramp.ValueRO.NumToSpawn;
            //int k = ramp.NumToSpawn;
            if (++ramp.ValueRW.NumSpawned > k)
            //if (++ramp.NumSpawned > k)
            {
                ramp.ValueRW.NumToSpawn++;
                //ramp.NumToSpawn++;
                ramp.ValueRW.NumSpawned = 0;
                //ramp.NumSpawned = 0;
            }
            
            for (int i = 0; i < k; ++k)
            {
                var entity = ECB.Instantiate(data.ValueRO.EntityToSpawn);
                //var entity = ECB.Instantiate(data.EntityToSpawn);

                float2 temp = data.ValueRO.Random.NextFloat2Direction() * data.ValueRO.Random.NextFloat(data.ValueRO.MinSpawnRange, data.ValueRO.MaxSpawnRange);
                //float2 temp = data.Random.NextFloat2Direction() * data.Random.NextFloat(data.MinSpawnRange, data.MaxSpawnRange);
                float3 position = new float3(temp.x, 0, temp.y);

                ECB.SetComponent(entity, new LocalTransform
                {
                    Position = transform.ValueRO.Position + position,
                    //Position = transform.Position + position,
                    Rotation = quaternion.identity,
                    Scale = 1
                });
            }
        }
    }
}