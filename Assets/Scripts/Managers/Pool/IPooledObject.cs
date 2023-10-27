using UnityEngine;

namespace Managers.Pool
{
    public interface IPooledObject
    {
        public void SpawnObject();
        public void KillObject();



        public T GetComponentType<T>() where T : MonoBehaviour;
    }
}
