using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float bulletDamage;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyController hitEnemy = collision.gameObject.GetComponent<EnemyController>();
        if (hitEnemy)
        {
            hitEnemy.TakeDamage(bulletDamage);
        }
    }
}
