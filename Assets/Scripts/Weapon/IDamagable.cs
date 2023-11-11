using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
   public float Health { get; set; }

   public void TakeDamage(float damage)
   {
      Health -= damage;
      //Debug.Log("Taking Damage: " + Health);

      if (Health <= 0)
      {
         Die();
      }
   }

   public void Die();
}
