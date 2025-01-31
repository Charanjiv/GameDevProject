using Unity.Netcode;
using UnityEngine;

public class DDA_Game_Manager : MonoBehaviour
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

    public float baseDifficulty = 1.0f;
    public float difficultyIncreaseAmount = 0.2f;
    public float difficultyDecreaseAmount = 0.2f;

    public float highThreshold = 0.8f;
    public float lowThreshold = 0.2f;


    public void AdjustDifficultyBasedOnPerformance(float performance)
    {
        //If performance is above the high threshold, increase difficulty
        if (performance > highThreshold)
        {
            baseDifficulty += difficultyIncreaseAmount;
            Debug.Log("Increasing difficulty. New Difficulty: " + baseDifficulty);
        }
        else if (performance < lowThreshold)
        {
            baseDifficulty -= difficultyDecreaseAmount;
            Debug.Log("Decreasing difficulty. New Difficulty: " + baseDifficulty);
        }
        else
        {
            Debug.Log("Difficulty is normal");
        }

        baseDifficulty = Mathf.Clamp(baseDifficulty, 0.5f, 2.0f);
    }
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
        AdjustDifficultyBasedOnPerformance(player.overallPerformance);
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
