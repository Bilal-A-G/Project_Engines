using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurretController : MonoBehaviour
{
    //[SerializeField] private Camera mainCamera;
    //[SerializeField] private float bulletVelocity;
    [SerializeField] private Transform turret;
    //[SerializeField] private float rotationSpeed;

    [SerializeField] private float maxHealth = 10;
    [SerializeField] private WeaponBase weapon;

    private bool isShooting;
    
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

    public static TurretController Player { get; private set; }

    private void Start()
    {
        Player = this;
        ctrls = new();
        ctrls.Cringe.Enable();
        
        ctrls.Cringe.Shoot.performed += _ =>
        {
            isShooting = !isShooting;
            if(isShooting) weapon.StartShooting();
            else weapon.StopShooting();
        };
     
        Vector2 mousePosition = Vector2.zero;
        ctrls.Cringe.Move.performed += ctx =>
        {
            mousePosition += ctx.ReadValue<Vector2>() * mouseSensitivity;
            //Debug.Log(Mathf.Atan2(mousePosition.y,mousePosition.x) + ", " + ctx.ReadValue<Vector2>() +" ," + angle);
            turret.eulerAngles = new Vector3(0,mousePosition.x,0);
        };

        ctrls.Cringe.Escape.performed += _ => ToggleLock();

Lock();
        
        fsm.UpdateState(activateTower, callingObject, cachedObjects);

        currentHealth = maxHealth;

        //updateTower.Subscribe(UpdateTower);
        currentHealth = maxHealth;
        healthBar.maxValue = currentHealth;
        healthBar.value=currentHealth;
    }

    public void SetNewWeapon(WeaponStatsSO newWeapon)
    {
        weapon.SetNewStats(newWeapon);
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


    public void OnTakeDamage(int amount)
    {
        //Listen to enemy attack tower here
        //Decrement current health
        currentHealth -= amount;
        healthBar.value = currentHealth;

        //Reload scene for now
        if (currentHealth <= 0)
        {
            FindObjectOfType<UIHandler>().ShowEndScreen();

            SceneManager.LoadScene("Game");
        }
            
    }

}
