using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static readonly Dictionary<string, ParticleSystem> ParticleSet = new Dictionary<string, ParticleSystem>();
    
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem[] ps = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particles in ps)
        {
            ParticleSet.Add(particles.name, particles);
        }
    }

    public static void SpawnParticle(string name, Vector3 position, Quaternion rotation)
    {
        if (!ParticleSet.ContainsKey(name))
        {
            Debug.LogWarning($"There is no particle set called: {name}");
            return;
        }
        ParticleSet[name].transform.SetPositionAndRotation(position, rotation);
        ParticleSet[name].Play();
    }
}
