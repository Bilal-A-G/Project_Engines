using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector] public Transform tower;
    [HideInInspector] public EnemySO enemyStats;

    [SerializeField] private Transform enemy;
    [SerializeField] private float distanceToDamageTower;

    private float timeSinceLastHit;
    private float currentHealth;

    //Read in health from the SO and update current health
    private void Start()
    {
        timeSinceLastHit = enemyStats.hitCooldown;    
    }

    private void Update()
    {
        if ((enemy.position - tower.position).magnitude > distanceToDamageTower)
        {
            Move();
            return;
        }

        timeSinceLastHit += Time.deltaTime;
        if (timeSinceLastHit < enemyStats.hitCooldown)
            return;

        Attack();
    }

    private void Attack()
    {
        timeSinceLastHit = 0;
        Debug.Log("Take that!!");
        //Add attack events here, that will be observed on the turret causing it's health to decrease
        //Add attack damage on the SO
    }

    private void Move()
    {
        //Read in speed here from the SO
        enemy.transform.position += (tower.position - enemy.position).normalized * Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
            Destroy(enemy.gameObject);

        //Decrement gameManager.numEnemies
    }
}
