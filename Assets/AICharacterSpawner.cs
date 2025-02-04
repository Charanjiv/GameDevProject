using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] GameObject characterGameObject;
    [SerializeField] GameObject instantiatedGameObject;
    private AICharacterManager aICharacter;
    public Transform startTransform;
    public Vector3 startPosition;
    private void Awake()
    {

    }

    private void Start()
    {
        startPosition = transform.position;
        Debug.Log("startPosition" +  startPosition);
        characterGameObject.transform.position = transform.position;
        WorldAIManager.instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawnCharacter()
    {

        if (characterGameObject != null)
        {
            instantiatedGameObject = Instantiate(characterGameObject);
            instantiatedGameObject.transform.position = startPosition;
            instantiatedGameObject.transform.rotation = transform.rotation;
            instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
            startPosition = instantiatedGameObject.transform.position;
            aICharacter = instantiatedGameObject.GetComponent<AICharacterManager>();
            if (aICharacter != null)
            {
                WorldAIManager.instance.AddCharacterToSpawnedCharactersList(aICharacter);
            }


        }

    }

    public void ResetCharacter()
    {
        if (instantiatedGameObject != null)
        {
            
                gameObject.SetActive(true);
                Debug.Log("Iscalled");
                instantiatedGameObject.transform.position = gameObject.transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                aICharacter.aiCharacterNetworkManager.currentHealth.Value = aICharacter.aiCharacterNetworkManager.maxHealth.Value;
                if (aICharacter.isDead.Value)
                {
                    aICharacter.isDead.Value = false;
                    aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Empty", false, false, true, true);
                }
                gameObject.SetActive(false);
            
        }

    }
}
