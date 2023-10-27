using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Pool
{
    [DefaultExecutionOrder(-100)]
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private SpawnObjectType[] spawnObjects;

        private static Dictionary<string, Queue<IPooledObject>> _pool = new();

        // Start is called before the first frame update
        void Start()
        {
            _pool = new();
            foreach (SpawnObjectType obj in spawnObjects)
            {
                Queue<IPooledObject> objs = new();
                string poolName = obj.toSpawn.name;
                Transform go = new GameObject(poolName).transform;
                go.parent = transform;
                for (int i = 0; i < obj.spawnCount; ++i)
                {
                    GameObject summoned = Instantiate(obj.toSpawn, go);
                    summoned.SetActive(false);
                    objs.Enqueue(summoned.GetComponent<IPooledObject>());
                }
                _pool.Add(poolName, objs);
                Debug.Log("Creating pool: " + obj.toSpawn.name);
            }
        }

        public static T Spawn<T>(string name, float lifeTime = 0) where T : MonoBehaviour
        {
            if (!_pool.ContainsKey(name) || _pool[name].Count is 0)
            {
                Debug.LogError($"WARNING: pool, {name} is EMPTY! Please allocate more resources or cull previous objects!");
                return null;
            }
            IPooledObject obj = _pool[name].Dequeue();
            
            obj.SpawnObject();
            T ret = obj.GetComponentType<T>();
            ret.gameObject.SetActive(true);
            if(lifeTime > 0) ret.StartCoroutine(LifeTimer(obj, name, lifeTime));
            return ret;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void ReturnToPool(IPooledObject obj, string poolName)
        {
            poolName = poolName.Remove(poolName.Length-7);
            if (!_pool.ContainsKey(poolName))
            {
                Debug.LogError("Failed to repool object into pool: " + poolName);
                return;
            }

            _pool[poolName].Enqueue(obj);
        }

        private static IEnumerator LifeTimer(IPooledObject obj, string name, float time)
        {
            yield return new WaitForSeconds(time);
            obj.KillObject();
        }
    }

    [Serializable]
    public struct SpawnObjectType
    {
        [SerializeField] public GameObject toSpawn;
        public int spawnCount;
    }
    
    
}