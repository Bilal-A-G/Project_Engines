using ECS_Scripts;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnemySpawningAuthor : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float minSpawnRadius = 10;
    [SerializeField, Range(0, 100)] private float maxSpawnRadius = 10;
    [SerializeField, Range(0, 100)] private float minSpawnTime;
    [SerializeField, Range(0, 100)] private float maxSpawnTime;
    [SerializeField] private uint seed;
    [SerializeField] private EnemyAuthoring enemyPrefab;
    
    
    
    private class EnemySystemBaker : Baker<EnemySpawningAuthor>
    {
        public override void Bake(EnemySpawningAuthor authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            
            AddComponent(entity, new EnemySpawnerData
            {
                MinSpawnRange = authoring.minSpawnRadius,
                MaxSpawnRange = authoring.maxSpawnRadius,
                MinSpawnTime = authoring.minSpawnTime,
                MaxSpawnTime = authoring.maxSpawnTime,
                EntityToSpawn = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
                Random = Random.CreateFromIndex(authoring.seed)
            });

            AddComponent(entity, new EnemySpawnerRamp()
            {
                NumToSpawn = 1,
                NumSpawned = 0
            });

        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.DrawWireSphere(position, minSpawnRadius);
        Gizmos.DrawWireSphere(position, maxSpawnRadius);
    }
    #endif
    
   
    
}
public struct EnemySpawnerData : IComponentData
{
    public float MinSpawnRange;
    public float MaxSpawnRange;
    public float MinSpawnTime;
    public float MaxSpawnTime;
    public Entity EntityToSpawn;
    public Random Random;
}



public struct EnemySpawnerRamp : IComponentData
{
    public float TimeToNext;
    public int NumToSpawn;
    public int NumSpawned;
}