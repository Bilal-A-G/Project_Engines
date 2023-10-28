using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SpawnDecoration : MonoBehaviour
{
    public GameObject objectSpawned;
    public Transform spawnLocation;

    [DllImport("Oct19Activity")]
    private static extern void DecorationSpawn();

    // Start is called before the first frame update
    void Start()
    {
        if (objectSpawned != null && spawnLocation != null) 
        {
            Instantiate(objectSpawned, spawnLocation.position, spawnLocation.rotation);
            DecorationSpawn();
        }
    }

    
}
