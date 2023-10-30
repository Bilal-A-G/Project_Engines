using Managers.Pool;
using UnityEngine;

public class EnemyController : MonoBehaviour, IPooledObject
{
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
        if ((enemy.position - EnemySpawner.Target).magnitude > distanceToDamageTower)
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
        enemy.transform.position += (EnemySpawner.Target - enemy.position).normalized * Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if(currentHealth <= 0) KillObject();
    }

    public void SpawnObject()
    {
        print("Enemy Spawned");

    }

    public void KillObject()
    {
        gameObject.SetActive(false);
        PoolManager.ReturnToPool(this, name);
        --GameManager.Instance.numEnemies;// This is real lazy, but I'm not gunna do this part.

    }

    public T GetComponentType<T>() where T : MonoBehaviour
    {
        return this as T;
    }
}
