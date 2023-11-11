using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="Collectable", menuName = "ShooterDemo/Collectable")]
public class CollectableSO : ScriptableObject
{
    [field: SerializeField] public int Value { get; private set; }
    [field: SerializeField] public Mesh GetMesh { get; private set; }
    [field: SerializeField] public Material GetMaterial { get; private set; }
    [SerializeField] private UnityEvent onCollected;

    public void OnCollected() => onCollected?.Invoke();

}

