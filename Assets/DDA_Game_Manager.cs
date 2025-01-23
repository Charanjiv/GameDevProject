using Unity.Netcode;
using UnityEngine;

public class DDA_Game_Manager : NetworkBehaviour
{
    public static DDA_Game_Manager instance;
    public PlayerManager player;
    //public DDA_Difficulty_Manager difficultyManager; 
    //private float playerPerformance;           // Example performance metric
    //public float successRate;                  // Player's success rate
    //private int actionsCompleted;              // Actions completed by the player
    //private int totalActions;                  // Total possible actions
    //public DDA_Difficulty_Manager difficultyManager;
    public float killCount;
    public float playerDeaths;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        killCount = 0;
        playerDeaths = 0;
    }

    public void AddToKillCount()
    {
        
        killCount += (1.0f/4f);

        
        Debug.Log("Kill count: " + killCount);
    }

    public void AddToPlayerDeathCount()
    {

        playerDeaths += (1.0f / 4f);


        Debug.Log("Kill count: " + playerDeaths);
    }

    void Update()
    {

    }

    private void AdjustGameParameters()
    {
        
    }

    private void HealthPerformance()
    {

    }

    private void AccuracyPerformance()
    {

    }

    private void KillPerformance()
    {

    }

    private void LifePerformance()
    {

    }

    private void OverallPerformance()
    {

    }

}
