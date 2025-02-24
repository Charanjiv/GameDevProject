using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DDA_Difficulty_Manager : MonoBehaviour
{
    public static DDA_Difficulty_Manager instance;

    [SerializeField] public PlayerManager playerManager;
    public AICharacterManager[]  aiCharacterManager;
    public GameObject[] allEnemies;
    public AIUndeadCombatManager[] undeadCombatManager;
    public float score;
    public int enemyStartHealth = 500;
    private bool increaseDifficulty;
    private bool decreaseDifficulty;
    private bool normalDifficulty;
    public NavMeshAgent[] navMeshAgent;


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

        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        //navMeshAgent = NavMeshAgent navAgent = GetComponentInChildren<NavMeshAgent>();

        List<AICharacterManager> managerTempList = new List<AICharacterManager>();
        List<AIUndeadCombatManager> combatTempList = new List<AIUndeadCombatManager>();
        List<NavMeshAgent> navTempList = new List<NavMeshAgent>();
        
        foreach (GameObject enemy in allEnemies)
        {
            AICharacterManager manager = enemy.GetComponent<AICharacterManager>();
            AIUndeadCombatManager combat = enemy.GetComponent<AIUndeadCombatManager>();
            NavMeshAgent navAgentMesh = enemy.GetComponentInChildren<NavMeshAgent>();
            if (manager != null)
            {
                managerTempList.Add(manager);
            }
            if(combat != null)
            {
                combatTempList.Add(combat);
            }
            if(navAgentMesh != null)
            {
                navTempList.Add(navAgentMesh);
            }
            aiCharacterManager = managerTempList.ToArray();
            undeadCombatManager = combatTempList.ToArray();
            navMeshAgent = navTempList.ToArray();
        }
        //INCREASED DIFFICULTY
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
                    
                    Debug.Log("Health increased");

                    

                }
                foreach(AIUndeadCombatManager combat in undeadCombatManager)
                {
                    combat.baseDamage = 60;
                }

                foreach (NavMeshAgent navAgentMesh in navMeshAgent)
                {
                    navAgentMesh.speed = 100;
                    navAgentMesh.acceleration = 100;
                }
            }
            increaseDifficulty = false;
        }


        //LOWERED DIFFICULTY
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
                    Debug.Log("Health decreased");


                }
                foreach (AIUndeadCombatManager combat in undeadCombatManager)
                {
                    combat.baseDamage = 20;
                }
            }
            decreaseDifficulty = false;
        }

        //NORMAL
        if (normalDifficulty && enemyStartHealth != 500)
        {

            foreach (GameObject enemy in allEnemies)
            {

                foreach (AICharacterManager manager in aiCharacterManager)
                {
                    manager.aiCharacterNetworkManager.maxHealth.Value = 500;
                    enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                    manager.aiCharacterNetworkManager.currentHealth.Value = manager.aiCharacterNetworkManager.maxHealth.Value;
                    enemyStartHealth = manager.aiCharacterNetworkManager.maxHealth.Value;
                   


                }
                foreach (AIUndeadCombatManager combat in undeadCombatManager)
                {
                    combat.baseDamage = 40;
                }
            }
            normalDifficulty = false;
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

    }

    public void DecreaseDifficulty()
    {
        decreaseDifficulty = true;
        Debug.Log("Difficulty gone down");
    }
    public void NormalDifficulty()
    {
        normalDifficulty = true;
        
    }
}
