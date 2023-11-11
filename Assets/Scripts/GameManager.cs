using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


//Make this a singleton!!!
[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static Action onEnemyDestoryed;
    public static Action onEnemySpawned;
    public static Action OnRoundEnd;

    private static int remainingEnemies;
    
    [NonSerialized] public int CurrentRound=1; // Why was this serialized?
    UIHandler uiHandler;

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
        uiHandler = gameObject.GetComponent<UIHandler>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            onEnemyDestoryed += () =>
            {
                if (--remainingEnemies <= 0)
                {
                    ++CurrentRound;
                    OnRoundEnd.Invoke();
                }

                uiHandler.UpdateEnemyCount(remainingEnemies);
            };
            onEnemySpawned += () => ++remainingEnemies;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
}

