using UnityEngine;
using UnityEngine.SceneManagement;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletPrefab;
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

    [SerializeField] private float mouseSensitivity = 10;
    
    private SomethingControls ctrls;
    private bool isUnlocked = true;
    private bool activated = true;

    private void Start()
    {
        ctrls = new();
        ctrls.Cringe.Enable();
        ctrls.Cringe.Shoot.performed += _ =>
        {
            if (isUnlocked) Lock();

            if (timeSinceLastFire < cooldown) return;
            
            timeSinceLastFire = 0;
            Rigidbody instantiatedBullet = Instantiate(bulletPrefab);
            instantiatedBullet.transform.position = firePoint.position;
            instantiatedBullet.AddForce(bulletVelocity * firePoint.forward, ForceMode.Impulse);
            instantiatedBullet.GetComponent<Bullet>().bulletDamage = bulletDamage;
            Destroy(instantiatedBullet, bulletLifetime);
        };
     
        Vector2 mousePosition = Vector2.zero;
        ctrls.Cringe.Move.performed += ctx =>
        {
            mousePosition += ctx.ReadValue<Vector2>() * mouseSensitivity;
            //Debug.Log(Mathf.Atan2(mousePosition.y,mousePosition.x) + ", " + ctx.ReadValue<Vector2>() +" ," + angle);
            turret.eulerAngles = new Vector3(0,mousePosition.x,0);

        };

        ctrls.Cringe.Escape.performed += _ => Unlock();


        fsm.UpdateState(activateTower, callingObject, cachedObjects);

        timeSinceLastFire = cooldown;
        currentHealth = maxHealth;

        //updateTower.Subscribe(UpdateTower);
    }

    void Update()
    {
        fsm.UpdateState(updateTower, callingObject, cachedObjects);
/*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activated = !activated;

            fsm.UpdateState(activated ? activateTower : deactivateTower, callingObject, cachedObjects);
        }
*/
        timeSinceLastFire += Time.deltaTime;
    }

    public void Lock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isUnlocked = false;
    }

    public void Unlock()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isUnlocked = true;
    }


    void OnTakeDamage()
    {
        //Listen to enemy attack tower here
        //Decrement current health

        //Reload scene for now
        if (currentHealth <= 0)
            SceneManager.LoadScene("Game");
    }

}
