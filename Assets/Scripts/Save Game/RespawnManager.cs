using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    public GameObject lastCheckpoint; // Stores the last checkpoint GameObject

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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetCheckpoint(GameObject checkpoint)
    {
        lastCheckpoint = checkpoint; // Update the last checkpoint
        Debug.Log("Checkpoint set: " + checkpoint.name);
    }

    public GameObject GetLastCheckpoint()
    {
        return lastCheckpoint;
    }
}
