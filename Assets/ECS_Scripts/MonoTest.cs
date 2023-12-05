using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTest : MonoBehaviour
{
    [SerializeField] private int hp;
    public static int Health;

    private void Awake()
    {
        Health = hp;
    }

    private void Update()
    {
        print(Health);
    }
}
