using UnityEngine;

public class AIDurkCharacterManager : AIBossCharacterManager
{
    [HideInInspector] public AIDurkSoundFXManager durkSoundFXManager;
    [HideInInspector] public AIDurkCombatManager durkCombatManager;
    [HideInInspector] public AIBossCharacterNetworkManager networkManager;
    [HideInInspector] public CharacterStatsManager characterStatsManager;

    protected override void Awake()
    {
        base.Awake();

        durkSoundFXManager = GetComponent<AIDurkSoundFXManager>();
        durkCombatManager = GetComponent<AIDurkCombatManager>();
        networkManager = GetComponent<AIBossCharacterNetworkManager>();
        characterStatsManager = GetComponent <CharacterStatsManager>();
    }

    protected override void Start()
    {
        base.Start();
        networkManager.vitality.Value = 40;
        networkManager.endurance.Value = 10;
        characterStatsManager.CalculateHealthBasedOnVitalityLevel(networkManager.vitality.Value);
        characterStatsManager.CalculateStaminaBasedOnEnduranceLevel(networkManager.endurance.Value);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        characterNetworkManager.maxHealth.Value = 1000;
        characterNetworkManager.currentHealth.Value = 1000;
        networkManager.currentHealth.OnValueChanged += aiCharacterNetworkManager.CheckHP;

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        networkManager.currentHealth.OnValueChanged -= aiCharacterNetworkManager.CheckHP;
    }
}
