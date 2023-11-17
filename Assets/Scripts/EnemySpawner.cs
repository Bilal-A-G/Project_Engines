using Managers.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EnemySO[] possibleEnemyTypes;
    [SerializeField] private float spawnRadius;

    [SerializeField] private Transform target;
    public static Vector3 Target { get; private set; }

  
    

    private void Start()
    {
        Target = target.position; // Yes, this is faster than making target public.
        GameManager.OnRoundEnd += SpawnEnemies;
        SpawnEnemies();
    }
    
    void SpawnEnemies()
    {
        //for each gameManager.currentRound spawn an enemy
        int k = GameManager.Instance.CurrentRound + 1;
        print("Spawning Enemies : " + k);
        for (int i = 0; i < k; i++)
        {
            EnemyController ec = PoolManager.Spawn<EnemyController>("Enemy");
            Vector3 n = (Random.insideUnitCircle.normalized * spawnRadius);
            n.z = n.y;
            n.y = 0;
            ec.transform.position = Target + n;
        }
    }
}
