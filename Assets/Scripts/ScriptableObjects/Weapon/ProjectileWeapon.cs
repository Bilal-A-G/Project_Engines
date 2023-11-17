using System.Collections;
using Managers.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapon
{
    public class ProjectileWeapon : WeaponBase
    {
        private bool isBursting;
        //private static readonly WaitForSeconds Seconds = new WaitForSeconds(3);
        protected override void Attack(float percent)
        {
            print("My weapon attacked: " + percent);
            Fire(percent);
            if(weaponStats.BurstAmount != 0) StartCoroutine(FireLoop(percent));
        }

        private IEnumerator FireLoop(float percent)
        {
            isBursting = true;
            for (int i = 0; i < weaponStats.BurstAmount; ++i)
            {
                Fire(percent);
                yield return weaponStats.BurstPeriod;
            }

            isBursting = false;
        }

        protected override bool CanAttack(float percent)
        {
            return base.CanAttack(percent) && !isBursting;
        }

        [SerializeField] private Projectile myBullet;
        private void Fire(float percent)
        {
            for (int i = 0; i < weaponStats.ShotsFired; ++i)
            {
                
                Projectile proj = PoolManager.Spawn<Projectile>("Bullet");
                
                //Projectile proj = Instantiate(myBullet);
                //Destroy(proj.gameObject, 3);
                

                Quaternion randomAngle = Quaternion.Euler(Random.Range(-weaponStats.Spread, weaponStats.Spread),Random.Range(-weaponStats.Spread, weaponStats.Spread),0);
                Transform projTransform = proj.transform;
                projTransform.SetPositionAndRotation(firePoint.position, firePoint.rotation * randomAngle);

                proj.Init(percent, projTransform.forward, weaponStats.Bullet);
                
            }
        }
    }
}
