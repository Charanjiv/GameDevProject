using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Characters")]
    [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] List<AICharacterManager> spawnedInCharacters;

    [Header("Bosses")]
    [SerializeField] List<AIBossCharacterManager> spawnedInBosses;
    private Coroutine spawnAllCharactersCoroutine;
    private Coroutine despawnAllCharactersCoroutine;
    private Coroutine resetAllCharactersCoroutine;

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

    public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            aiCharacterSpawners.Add(aiCharacterSpawner);
            aiCharacterSpawner.AttemptToSpawnCharacter();
        }
    }

    public void AddCharacterToSpawnedCharactersList(AICharacterManager character)
    {
        if (spawnedInCharacters.Contains(character))
            return;

        spawnedInCharacters.Add(character);

        AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

        if (bossCharacter != null)
        {
            if (spawnedInBosses.Contains(bossCharacter))
                return;

            spawnedInBosses.Add(bossCharacter);
        }
    }

    public AIBossCharacterManager GetBossCharacterByID(int ID)
    {
        return spawnedInBosses.FirstOrDefault(boss => boss.bossID == ID);
    }

    public void SpawnAllCharacters()
    {   
        if(spawnAllCharactersCoroutine != null)
            StopCoroutine(spawnAllCharactersCoroutine);
        spawnAllCharactersCoroutine = StartCoroutine(SpawnAllCharactersCoroutine());

    }

    private IEnumerator SpawnAllCharactersCoroutine()
    {
        for(int i = 0; i < aiCharacterSpawners.Count; i++)
        {
            yield return new WaitForFixedUpdate();
            aiCharacterSpawners[i].AttemptToSpawnCharacter();
            
            yield return null;
        }
        yield return null;
    }

    public void ResetAllCharacters()
    {
        if (resetAllCharactersCoroutine != null)
            StopCoroutine(resetAllCharactersCoroutine);
        resetAllCharactersCoroutine = StartCoroutine(ResetAllCharactersCoroutine());
    }

    private IEnumerator ResetAllCharactersCoroutine()
    {
        for (int i = 0; i < spawnedInCharacters.Count; i++)
        {
            yield return new WaitForFixedUpdate();
            aiCharacterSpawners[i].ResetCharacter();
            yield return null;
        }
        yield return null;
    }

    public void DespawnAllCharacters()
    {
        if (despawnAllCharactersCoroutine != null)
            StopCoroutine(despawnAllCharactersCoroutine);
        despawnAllCharactersCoroutine = StartCoroutine(DespawnAllCharactersCoroutine());
    }

    public IEnumerator DespawnAllCharactersCoroutine()
    {
        for (int i = 0; i < spawnedInCharacters.Count; i++)
        {
            yield return new WaitForFixedUpdate();
            spawnedInCharacters[i].GetComponent<NetworkObject>().Despawn();
            yield return null;
        }
        spawnedInCharacters.Clear();
        yield return null;
    }

    private void DisableAllCharacters()
    {
        // TO DO DISABLE CHARACTER GAMEOBJECTS, SYNC DISABLED STATUS ON NETWORK
        // DISABLE GAMEOBJECTS FOR CLIENTS UPON CONNECTING, IF DISABLED STATUS IS TRUE
        // CAN BE USED TO DISABLE CHARACTERS THAT ARE FAR FROM PLAYERS TO SAVE MEMORY
        // CHARACTERS CAN BE SPLIT INTO AREAS (AREA_00_, AREA_01, AREA_02) ECT
    }
}
