using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ECS_Scripts;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[BurstCompile]
public partial struct BulletEnemySystem : ISystem
{
    
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //Debug.Log(deltaTime);
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            
        foreach (var (enemy, enemyTransform) in SystemAPI.Query<RefRW<EnemyData>, RefRO<LocalTransform>>())
        {
            foreach (var (bullet, bulletTransform) in SystemAPI.Query<RefRO<Bullet>, RefRO<LocalTransform>>())
            {
                if (math.distancesq(enemyTransform.ValueRO.Position, bulletTransform.ValueRO.Position) < 2)
                {
                    Debug.Log("Enemy hit");
                    if(--enemy.ValueRW.Health <= 0) ecb.DestroyEntity(enemy.ValueRO.Self);
                    ecb.DestroyEntity(bullet.ValueRO.Self);
                }
            }
        }
    }
    
}
