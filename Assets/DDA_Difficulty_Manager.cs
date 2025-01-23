using UnityEngine;

public class DDA_Difficulty_Manager : MonoBehaviour
{
    //public float currentDifficulty = 1.0f; // Current difficulty level
    //public float difficultyStep = 0.5f;    // How much to increase or decrease difficulty
    //public float lowerThreshold = 0.4f;    // Performance threshold for reducing difficulty
    //public float upperThreshold = 0.8f;    // Performance threshold for increasing difficulty
    //public float minDifficulty = 0.5f;     // Minimum difficulty level
    //public float maxDifficulty = 5.0f;     // Maximum difficulty level

    //// Adjust difficulty based on performance
    //public void AdjustDifficulty(float playerPerformance)
    //{
    //    if (playerPerformance > upperThreshold)
    //    {
    //        currentDifficulty += difficultyStep;
    //        Debug.Log("Increased difficulty to: " + currentDifficulty);
    //    }
    //    else if (playerPerformance < lowerThreshold)
    //    {
    //        currentDifficulty -= difficultyStep;
    //        Debug.Log("Decreased difficulty to: " + currentDifficulty);
    //    }

    //    // Clamp difficulty to defined range
    //    currentDifficulty = Mathf.Clamp(currentDifficulty, minDifficulty, maxDifficulty);
    //}

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



}
