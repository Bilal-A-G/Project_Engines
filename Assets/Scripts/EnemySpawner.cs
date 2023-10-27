using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<EnemySO> possibleEnemyTypes;
    [SerializeField] private List<Transform> spawnPoints;

    [SerializeField] private Transform tower;

    void Start()
    {
        GameManager gManager = GameManager.Instance;

        //For testing purposes, delete
        GameObject instantiatedEnemy = Instantiate(enemyPrefab);
        instantiatedEnemy.transform.position = spawnPoints[0].position;
        instantiatedEnemy.GetComponent<EnemyController>().enemyStats = possibleEnemyTypes[0];
        instantiatedEnemy.GetComponent<EnemyController>().tower = tower;
    }

    private void Update()
    {
        //if gameManager.numEnemies is 0
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
        }

        //Factory:
        //Spawn a random enemy type
        //Spawn it at a random spawner
        //Assign the tower variable


        //Increment num enemies
        GameManager.Instance.numEnemies++;
        //Add object pooling here
    }
}
