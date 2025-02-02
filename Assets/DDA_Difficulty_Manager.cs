using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DDA_Difficulty_Manager : MonoBehaviour
{
    public static DDA_Difficulty_Manager instance;

    [SerializeField] public PlayerManager playerManager;
    public AICharacterManager[]  aiCharacterManager;
    public GameObject[] allEnemies;
    public float score;
    public int enemyStartHealth = 500;
    private bool increaseDifficulty;
    private bool decreaseDifficulty;


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
        //foreach (var enemy in allEnemies)
        //{
        //    aiCharacterManager = GetComponent<AICharacterManager>();
        //}
        



    }
    public void Update()
    {

        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        List<AICharacterManager> tempList = new List<AICharacterManager>();
        foreach (GameObject enemy in allEnemies)
        {
            AICharacterManager manager = enemy.GetComponent<AICharacterManager>();
            if(manager != null)
            {
                tempList.Add(manager);
            }
            aiCharacterManager = tempList.ToArray();
        }
        if (increaseDifficulty && enemyStartHealth == 500)
        {
            
            foreach (GameObject enemy in allEnemies)
            {

                foreach (AICharacterManager manager in aiCharacterManager)
                {
                    manager.aiCharacterNetworkManager.maxHealth.Value = 1000;
                    enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                    manager.aiCharacterNetworkManager.currentHealth.Value = manager.aiCharacterNetworkManager.maxHealth.Value;
                    enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                    Debug.Log("Health changed should work");

                    if (enemyStartHealth != manager.aiCharacterNetworkManager.maxHealth.Value)
                    {
                        
                        
                            enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                            manager.aiCharacterNetworkManager.currentHealth.Value = manager.aiCharacterNetworkManager.maxHealth.Value;
                            enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                            Debug.Log("Health changed should work");
                        

                    }
                    

                }
            }
            increaseDifficulty = false;
        }

        if (decreaseDifficulty && enemyStartHealth == 500)
        {

            foreach (GameObject enemy in allEnemies)
            {

                foreach (AICharacterManager manager in aiCharacterManager)
                {
                    manager.aiCharacterNetworkManager.maxHealth.Value = 300;
                    enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                    manager.aiCharacterNetworkManager.currentHealth.Value = manager.aiCharacterNetworkManager.maxHealth.Value;
                    enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                    Debug.Log("Health changed should work");

                    if (enemyStartHealth != manager.aiCharacterNetworkManager.maxHealth.Value)
                    {


                        enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                        manager.aiCharacterNetworkManager.currentHealth.Value = manager.aiCharacterNetworkManager.maxHealth.Value;
                        enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                        Debug.Log("Health changed should work");


                    }


                }
            }
            decreaseDifficulty = false;
        }



    }
    public void UpdateScore()
    {
            score+= (1.0f/4.0f);
        
        return;
    }

    

    public void IncreaseDifficulty()
    {
        increaseDifficulty = true;
        //foreach(GameObject enemy in allEnemies)
        //{

        //    //aiCharacterManager.aiCharacterNetworkManager.maxHealth.Value = 1000;
        //    //aiCharacterManager = enemy.GetComponent<AICharacterManager>();
        //    //aiCharacterManager.aiCharacterNetworkManager.currentHealth.Value = 1000;
        //    //aiCharacterManager.aiCharacterNetworkManager.maxHealth.Value = 1000;
        //    Debug.Log("Increase Health of enemies");

        //}
        //allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        //aiCharacterManager.aiCharacterNetworkManager.maxHealth.Value = 1000;
        //aiCharacterManager.aiCharacterNetworkManager.currentHealth.Value = 1000;
        //Debug.Log("Enemy health: " + aiCharacterManager.aiCharacterNetworkManager.maxHealth.Value);
        //foreach(var enemy in allEnemies)
        //{
        //    GetComponent<AICharacterNetworkManager>();
        //}
    }

    public void DecreaseDifficulty()
    {
        decreaseDifficulty = true;
        Debug.Log("Difficulty gone down");
    }
}
