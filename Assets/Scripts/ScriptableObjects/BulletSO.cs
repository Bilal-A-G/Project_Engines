using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ShootDemo/BulletSO", fileName = "BulletSO", order = 1)]
    public class BulletSo : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Scale { get; private set; }
    }
}
