using System;
using Managers.Pool;
using Unity.Mathematics;
using UnityEngine;

public class Collectable : MonoBehaviour, IPooledObject
{

    private CollectableSO stats;
    private MeshFilter filter;
    private MeshRenderer renderer;

    public void Create(CollectableSO s)
    {
        stats = s;
        if (!filter)
        {
            //Awake is not an option with pool sys currently
            filter = GetComponent<MeshFilter>();
            renderer = GetComponent<MeshRenderer>();
        }

        filter.mesh = s.GetMesh;
        renderer.material = s.GetMaterial;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Bullet")) return; // Super lazy, save this in GM to optimize
        //IMPLEMENT
        TurretController.Player.GiveCurrency(stats.Value);
        KillObject();
    }

    public void SpawnObject()
    {
        
    }

    public void KillObject()
    {
       gameObject.SetActive(false);
       //stats.OnCollected(); //OPTIONAL IMPLEMENT
    }

    public T GetComponentType<T>() where T : MonoBehaviour
    {
        return this as T;
    }
}
