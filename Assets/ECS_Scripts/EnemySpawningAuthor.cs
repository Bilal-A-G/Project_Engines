using Unity.Entities;
using UnityEngine;

public class EnemySpawningAuthor : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 10;
    [SerializeField] private EnemySO enemyPrefab;
    
    
    
    private class EnemySystemBaker : Baker<EnemySpawningAuthor>
    {
        public override void Bake(EnemySpawningAuthor authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            
            AddComponent(entity, new EnemySpawnerData
            {
                Round = GameManager.Instance.CurrentRound
            });
           
        }
    }

    //#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    

    //#endif
    
   
    
}
public struct EnemySpawnerData : IComponentData
{
    public int Round;
}