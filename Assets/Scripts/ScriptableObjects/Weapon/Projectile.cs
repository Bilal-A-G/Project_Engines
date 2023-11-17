using Managers.Pool;
using ScriptableObjects;
using UnityEngine;

namespace Weapon
{
    public class Projectile : MonoBehaviour, IPooledObject
    {

    
        private Rigidbody rb;
        private float trueDamage;
        //private static readonly Collider[] MaxCollisions = new Collider[20];
        private static int _targetLayer = -1;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (_targetLayer == -1)
            
                _targetLayer = LayerMask.NameToLayer("Enemy");
        }

        public void Init(float chargePercent, Vector3 fireDirection, BulletSo stats)
        {
            transform.localScale = Vector3.one * stats.Scale;
            rb.AddForce(stats.Speed * chargePercent * fireDirection, ForceMode.Impulse);
            trueDamage = chargePercent * stats.Damage;
        }
    

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != _targetLayer) return;
            if (other.transform.TryGetComponent(out IDamagable hitTarget))
            {
                
                switch (other.transform.tag)
                {
                    case "Head":
                        trueDamage *= 2;
                        break;
                    case "Limb":
                        trueDamage *= 0.8f;
                        break;
                }
                hitTarget.TakeDamage(trueDamage);
            }
            KillObject();
        }

        public void SpawnObject()
        {
        
        
        }

        public void KillObject()
        {
            gameObject.SetActive(false);
            //PoolManager.ReturnToPool(this, name);
        }

        public T GetComponentType<T>() where T : MonoBehaviour
        {
            return this as T;
        }
    }
}
