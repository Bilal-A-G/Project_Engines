using System;
using UnityEngine;

namespace ScriptableObjects
{

    [Flags]
    public enum EProjectileType
    {
        Water = 1,
        Poison = 2,
        Fire = 4
    }

    [CreateAssetMenu(menuName = "ShootDemo/BulletSO", fileName = "BulletSO", order = 1)]
    public class BulletSo : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Scale { get; private set; }

        [field: SerializeField] public EProjectileType BulletType { get; private set; }

    }
}
