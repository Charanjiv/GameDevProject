using System.Collections.Generic;
using UnityEngine;

public class DDA_Difficulty_Manager : MonoBehaviour
{
    public static DDA_Difficulty_Manager instance;

    [HideInInspector] public PlayerManager playerManager;
    //[SerializeField] AICharacterManager  aiCharacterManager;
    private GameObject Target;
    public float score;

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
        score = 0;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Update()
    {
        
        Debug.Log("Enemies slain " + score);
    }
    public void UpdateScore()
    {
            score+= (1.0f/4.0f);
        
        return;
    }
}
