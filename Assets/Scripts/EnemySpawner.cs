using System;
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

    [SerializeField, Range(0,100)] private float spawnChance;

    [SerializeField] private CollectableSO coinToSpawn; // Modify to spawn diamonds too
    

    private void Start()
    {
        Target = target.position; // Yes, this is faster than making target public.
        GameManager.OnRoundEnd += SpawnEnemies;

        GameManager.onEnemyDestoryed += () =>
        {
            if (spawnChance >= Random.Range(1, 100)) return;
            Collectable c = PoolManager.Spawn<Collectable>("Collectable");
            c.Create(coinToSpawn);
        };
        SpawnEnemies();
    }
    
    void SpawnEnemies()
    {
        //for each gameManager.currentRound spawn an enemy
        print("Spawning Enemies : " + GameManager.Instance.CurrentRound);
        for (int i = 0; i < GameManager.Instance.CurrentRound; i++)
        {
            EnemyController ec = PoolManager.Spawn<EnemyController>("Enemy");
            Vector3 n = (Random.insideUnitCircle.normalized * spawnRadius);
            n.z = n.y;
            n.y = 0;
            ec.transform.position = Target + n;
        }
    }
}
