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
    private Transform startTransform;
    private void Awake()
    {

    }

    private void Start()
    {
        characterGameObject.transform.position = transform.position;
        WorldAIManager.instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawnCharacter()
    {
        
        if (characterGameObject != null)
        {
            instantiatedGameObject = Instantiate(characterGameObject);
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
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
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            aICharacter.aiCharacterNetworkManager.currentHealth.Value = aICharacter.aiCharacterNetworkManager.maxHealth.Value;
            if (aICharacter.isDead.Value)
            {
                aICharacter.isDead.Value = false;
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Empty", false, false, true, true);
            }
        }
    }

}
