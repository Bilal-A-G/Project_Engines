using Managers.Pool;
using UnityEngine;

public class EnemyController : MonoBehaviour, IPooledObject, IDamagable
{
    [SerializeField] private EnemySO enemyStats;

    [SerializeField] private Transform enemy;
    [SerializeField] private float distanceToDamageTower;

    private float timeSinceLastHit;
    public float Health { get; set; }


    //Read in health from the SO and update current health
    private void Start()
    {
        timeSinceLastHit = enemyStats.hitCooldown;
        Health = enemyStats._health;
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
        
        //Fuck it, nobody did this so I'm cheesing it -- Gabe
        TurretController.Player.OnTakeDamage(enemyStats._damage);
        
    }

    private void Move()
    {
        //Read in speed here from the SO
        enemy.transform.position += (EnemySpawner.Target - enemy.position).normalized * Time.deltaTime;
    }

  
    public void Die()
    {
        print("DIED");
        KillObject();
        
    }
    public void KillObject()
    {
        print("KILLED");
        gameObject.SetActive(false);
        //PoolManager.ReturnToPool(this, name);
        ParticleManager.SpawnParticle("Confetti", transform.position, Quaternion.identity);
        GameManager.onEnemyDestoryed.Invoke();// This is real lazy, but I'm not gunna do this part.
    }
    
    public void SpawnObject()
    {
        print("Enemy Spawned");
        GameManager.onEnemySpawned.Invoke();
    }

    

    public T GetComponentType<T>() where T : MonoBehaviour
    {
        return this as T;
    }

    /* Unless they die too, they'd hit you once, and only once... I've disabled this - Gabe.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<TurretController>().OnTakeDamage();
        }
    } */
}
