using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Make this a singleton!!!
public class GameManager : MonoBehaviour
{
    public int numEnemies;
    public int currentRound;


    public Observer _observer;


    private static GameManager instance;//GameManager is a singleton with just 1 instance of it existing the whole game.

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                GameObject singleton = new GameObject("GameManager");
                instance = singleton.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        _observer = GameObject.FindObjectOfType<Observer>();
        _observer.OnVariableChange += UpdateEnemyCount;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void UpdateEnemyCount(int newVar)
    {
        _observer.EnemyCount = numEnemies;
    }
}
