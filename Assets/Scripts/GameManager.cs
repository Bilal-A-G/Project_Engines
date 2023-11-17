using System;
using DLLs;
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
    private GameData dat;
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
    
    #if UNITY_EDITOR
    [ContextMenu("ResetSaveData")]
    public void ResetSaveData()
    {
        StupidSaving.SaveGame(new GameData());
    }
#endif
    

 
    private void Awake()
    {
        uiHandler = gameObject.GetComponent<UIHandler>();
        if (instance == null)
        {
            instance = this;
            
            dat = StupidSaving.LoadGame();
            CurrentRound = dat.level;
      
            
            DontDestroyOnLoad(gameObject);
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
            OnRoundEnd += () =>
            {
                StupidSaving.SaveGame(new GameData // Eh im lazy.
                {
                    health = TurretController.Player.CurrentHealth,
                    level = CurrentRound,
                    score =  TurretController.Player.CurrentScore
                });
            };
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        TurretController.Player.CurrentHealth = dat.health;
        TurretController.Player.CurrentScore = dat.score;
    }
}

