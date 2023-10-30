using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurretController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private Transform turret;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float maxHealth;

    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletLifetime;

    [SerializeField] private float cooldown;
    private float timeSinceLastFire;
    private float currentHealth;

    [Header("W_I_Zr_Ds")]
    [SerializeField] private EventObject updateTower;
    [SerializeField] private EventObject activateTower;
    [SerializeField] private EventObject deactivateTower;

    [SerializeField] private StateLayerObject fsm;
    [SerializeField] private CachedObjectWrapper cachedObjects;
    [SerializeField] private GameObject callingObject;

    private bool activated = true;

    private void Start()
    {
        fsm.UpdateState(activateTower, callingObject, cachedObjects);

        timeSinceLastFire = cooldown;
        currentHealth = maxHealth;

        updateTower.Subscribe(UpdateTower);
    }

    void Update()
    {
        fsm.UpdateState(updateTower, callingObject, cachedObjects);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activated = !activated;

            if (activated)
            {
                fsm.UpdateState(activateTower, callingObject, cachedObjects);
            }
            else
            {
                fsm.UpdateState(deactivateTower, callingObject, cachedObjects);
            }
        }

        timeSinceLastFire += Time.deltaTime;
    }

    void UpdateTower()
    {
        RotateTowardsTarget();

        if (timeSinceLastFire < cooldown)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            timeSinceLastFire = 0;

            GameObject instantiatedBullet = Instantiate(bulletPrefab);
            instantiatedBullet.transform.position = firePoint.position;
            Rigidbody physicsBody = instantiatedBullet.GetComponent<Rigidbody>();
            physicsBody.AddForce(bulletVelocity * firePoint.forward, ForceMode.Impulse);
            instantiatedBullet.GetComponent<Bullet>().bulletDamage = bulletDamage;

            Destroy(instantiatedBullet, bulletLifetime);
        }
    }

    void OnTakeDamage()
    {
        //Listen to enemy attack tower here
        //Decrement current health

        //Reload scene for now
        if (currentHealth <= 0)
            SceneManager.LoadScene("Game");
    }

    void RotateTowardsTarget()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 worldSpaceMousePos = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 1.0f));
        Vector3 toMousePos = (worldSpaceMousePos - turret.position).normalized;

        turret.transform.rotation = Quaternion.Lerp(turret.transform.rotation,
            Quaternion.LookRotation(new Vector3(toMousePos.x, 0, toMousePos.z).normalized, turret.up), Time.deltaTime * rotationSpeed);
    }
}
