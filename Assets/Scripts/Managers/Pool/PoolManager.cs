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

        public static T Spawn<T>(string name, WaitForSeconds wait = null) where T : MonoBehaviour
        {
            if (!_pool.ContainsKey(name) || _pool[name].Count is 0)
            {
                Debug.LogError("No pool made or pool is empty");
                return null;
            }
            IPooledObject obj = _pool[name].Dequeue();
            
            obj.SpawnObject();
            T ret = obj.GetComponentType<T>();
            ret.gameObject.SetActive(true);
/*
            if (_pool[name].Count is 0)
            {
                T x = obj.GetComponentType<T>();
                _pool[name].Enqueue(obj);
            }*/
            
            if(wait != null) ret.StartCoroutine(LifeTimer(obj, name, wait));
            
            
            
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

        private static IEnumerator LifeTimer(IPooledObject obj, string name, WaitForSeconds wait)
        {
            yield return wait;
            obj.KillObject();
            _pool[name].Enqueue(obj);
        }
    }

    [Serializable]
    public struct SpawnObjectType
    {
        [SerializeField] public GameObject toSpawn;
        public int spawnCount;
    }
    
    
}