using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIHandler : MonoBehaviour
{
    public TMP_Text enemytext;

    public GameObject endScreen;


    public void UpdateEnemyCount(int newVar)
    {
        enemytext.text = newVar.ToString();
    }

    public void ShowEndScreen()
    {
        endScreen.SetActive(true);
    }
}
