using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private Transform turret;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float maxHealth = 10;

    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletLifetime;

    [SerializeField] private float cooldown;
    private float timeSinceLastFire;


    public float currentHealth;
    public Slider healthBar;

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
            Debug.Log(mousePosition.x);

        };

        ctrls.Cringe.Escape.performed += _ => ToggleLock();

Lock();
        
        fsm.UpdateState(activateTower, callingObject, cachedObjects);

        timeSinceLastFire = cooldown;
        currentHealth = maxHealth;

        //updateTower.Subscribe(UpdateTower);
        currentHealth = maxHealth;
        healthBar.maxValue = currentHealth;
        healthBar.value=currentHealth;
    }

    private void ToggleLock()
    {
        if (isUnlocked) Lock();
        else Unlock();
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


    public void OnTakeDamage()
    {
        //Listen to enemy attack tower here
        //Decrement current health
        currentHealth--;
        healthBar.value = currentHealth;

        //Reload scene for now
        if (currentHealth <= 0)
        {
            FindObjectOfType<UIHandler>().ShowEndScreen();

            SceneManager.LoadScene("Game");
        }
            
    }

}
