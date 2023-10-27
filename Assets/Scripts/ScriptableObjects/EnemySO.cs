using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemies", menuName = "Enemies/New Enemy")]
public class EnemySO : ScriptableObject
{
    public float hitCooldown;
    [field: SerializeField] public int _health;
    [field: SerializeField] public float _speed;
    [field: SerializeField] public float _damage;

    public enum _attackTypes
    {
        Long_Ranged,
        Short_Ranged,
        Suicide_Bomber
    }

    [SerializeField] public _attackTypes m_AttackTypes;

    public interface Interface
    {
        public void SetAttackType();
    }
}

