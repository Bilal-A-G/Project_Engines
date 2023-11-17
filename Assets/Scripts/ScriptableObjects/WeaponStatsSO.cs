using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ShootDemo/WeaponSO", fileName = "WeaponStats", order = 1)]
    public class WeaponStatsSO : ScriptableObject
    {
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float timeBetweenBursts;
        [field: SerializeField] public float ChargeUpTime { get; private set; }
        [field: SerializeField, Range(0, 1)] public float MinChargePercent { get; private set; }
        [field: SerializeField, Range(0, 90)] public float Spread { get; private set; }
        [field: SerializeField] public bool IsFullyAuto { get; private set; }
        public WaitForSeconds CoolDownWait { get; private set; }
        public WaitForSeconds BurstPeriod { get; private set; }

        [field: SerializeField] public BulletSo Bullet { get; private set; }

        [field: SerializeField] public int ShotsFired { get; private set; }
        [field: SerializeField, Min(0)] public int BurstAmount { get; private set; }

        [field: SerializeField] public ECommonType WeaponType { get; private set; }


        private void OnEnable()
        {
            CoolDownWait = new WaitForSeconds(timeBetweenAttacks);
            BurstPeriod = new WaitForSeconds(timeBetweenBursts);
        }
    }

    [Flags]
    public enum ECommonType
    {
        Common = 1, // 000
        Uncommon = 2, // 001
        Rare = 4, // 010
        Epic = 8, // 011
        Lepicary = 24,
        Legendary = 16 // 100
    }

}
