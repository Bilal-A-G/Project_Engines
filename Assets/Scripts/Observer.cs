using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//not in use

public class Observer : MonoBehaviour
{
    private int enemyCount = 0;
    public TMP_Text enemytext;


    public event OnVariableChangeDelegate OnVariableChange;
    public delegate void OnVariableChangeDelegate(int newValue);
    
    public int EnemyCount
    {
        get
        {
            enemytext.text = enemyCount.ToString();
            return enemyCount;
        }
        set
        {
            if (enemyCount == value) return;
            enemyCount = value;
            enemytext.text = value.ToString();
            if (OnVariableChange != null) OnVariableChange(enemyCount);
        }
    }
}
