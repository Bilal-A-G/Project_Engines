using Managers.Pool;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EnemySO[] possibleEnemyTypes;
    [SerializeField] private float spawnRadius;

    [SerializeField] private Transform target;
    public static Vector3 Target { get; private set; }

    private void Update()
    {
        //if gameManager.numEnemies is 0
        Target = target.position; // Yes, this is faster than making target public.
        
        //WHY IS THIS NOT OBSERVER? THAT'S LITERALLY THE PURPOSE OF OBSERVER?
        if (GameManager.Instance.numEnemies == 0)
        {
            GameManager.Instance.currentRound++;
            SpawnEnemies();
        }
        //Increment wave count
        //Spawn enemies
    }

    void SpawnEnemies()
    {
        //for each gameManager.currentRound spawn an enemy
        for (int i = 0; i < GameManager.Instance.currentRound; i++)
        {
            Debug.Log("Hello");
            EnemyController ec = PoolManager.Spawn<EnemyController>("Enemy");
            Vector3 n = (Random.insideUnitCircle.normalized * spawnRadius);
            n.z = n.y;
            n.y = 0;
            ec.transform.position = Target + n;
            GameManager.Instance.numEnemies++;// This is real lazy, but I'm not gunna do this part.
        }

        //Factory:
        //Spawn a random enemy type
        //Spawn it at a random spawner
        //Assign the tower variable


        //Increment num enemies
        //GameManager.Instance.numEnemies++;
        //Add object pooling here
    }
}
