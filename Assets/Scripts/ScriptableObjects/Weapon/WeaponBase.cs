using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected WeaponStatsSO weaponStats;
    [SerializeField] protected Transform firePoint;
    
    private Coroutine _currentFireTimer;
    private bool _isOnCooldown;
    private float _currentChargeTime;
    
    private WaitUntil _coolDownEnforce;
    

    private void Start()
    {
        _coolDownEnforce = new WaitUntil(() => !_isOnCooldown);
    }

    private void Update()
    {

        print((1 << gameObject.layer) + (1 << LayerMask.NameToLayer("Bullet")));
        
        if (((int)weaponStats.WeaponType & (int)ECommonType.Uncommon) == (int)ECommonType.Uncommon)
        {
            Debug.Log("It works: " + weaponStats.WeaponType);
        }
    }


    public void StartShooting()
    {
        _currentFireTimer = StartCoroutine(ReFireTimer());
    }

    public void StopShooting()
    {
        StopCoroutine(_currentFireTimer);

        float percent = _currentChargeTime / weaponStats.ChargeUpTime;
        if(percent != 0) TryAttack(percent);

    }

    

    private IEnumerator CooldownTimer()
    {
        _isOnCooldown = true;
        yield return weaponStats.CoolDownWait;
        _isOnCooldown = false;
    }

    private IEnumerator ReFireTimer()
    {
        print("Waiting for cooldown");
        yield return _coolDownEnforce;
        print("Post cooldown");

        while (_currentChargeTime < weaponStats.ChargeUpTime)
        {
            _currentChargeTime += Time.deltaTime;
            yield return null;
        }

        TryAttack(1);
        yield return null;
    }

    private void TryAttack(float percent)
    {
        _currentChargeTime = 0;
        if (!CanAttack(percent)) return;
        
        Attack(percent);

        StartCoroutine(CooldownTimer());
        
        if (weaponStats.IsFullyAuto && percent >= 1) _currentFireTimer = StartCoroutine(ReFireTimer()); // Auto refire
        
    }

    protected virtual bool CanAttack(float percent)
    {
        return !_isOnCooldown && percent >= weaponStats.MinChargePercent;
    }

    protected abstract void Attack(float percent);

    public void SetNewStats(WeaponStatsSO newWeapon)
    {
        weaponStats = newWeapon;
        _isOnCooldown = false;
        StopAllCoroutines();
    }
}
