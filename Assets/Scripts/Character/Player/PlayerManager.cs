using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;




    [Header("DEBUG MENU")]
    [SerializeField] bool respawnCharacter = false;
    private float pHealth;
    private float deathCount;
    private float pKills;
    [SerializeField] private float totalEnemies;
    [HideInInspector] public float overallPerformance;

    //DDA
    //public float killCount;
    public float playerDeaths;

    public float baseDifficulty = 1.0f;
    public float difficultyIncreaseAmount = 0.2f;
    public float difficultyDecreaseAmount = 0.2f;

    public float highThreshold = 0.8f;
    public float lowThreshold = 0.2f;


    protected override void Awake()
    {
        base.Awake();

        //  DO MORE STUFF, ONLY FOR THE PLAYER

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        deathCount = 0;
        pKills = 0;
        killCount = 0;
        playerDeaths = 0;
    }

    protected override void Update()
    {
        base.Update();

        //  IF WE DO NOT OWN THIS GAMEOBJECT, WE DO NOT CONTROL OR EDIT IT
        if (!IsOwner)
            return;

        //  HANDLE MOVEMENT
        playerLocomotionManager.HandleAllMovement();

        //  REGEN STAMINA
        playerStatsManager.RegenerateStamina();

        DebugMenu();
        HealthPerformance(pHealth);
        //KillPerformance(pKills);
        //LifePerformance(deathCount);
        OverallPerformance();
        Debug.Log("Health Performance:" + HealthPerformance(pHealth));
        Debug.Log("Kill Performance:" + KillPerformance(pKills));
        //Debug.Log("Life Performance:" + KillPerformance(deathCount));
        AdjustDifficultyBasedOnPerformance(overallPerformance);
    }

    protected override void LateUpdate()
    {
        if (!IsOwner)
            return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        //  IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
            WorldSaveGameManager.instance.player = this;

            //  UPDATE THE TOTAL AMOUNT OF HEALTH OR STAMINA WHEN THE STAT LINKED TO EITHER CHANGES
            playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

            //  UPDATES UI STAT BARS WHEN A STAT CHANGES (HEALTH OR STAMINA)
            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
        }

        //  ONLY UPDATE FLOATING HP BAR IF THIS CHARACTER IS NOT THE LOCAL PLAYERS CHARACTER (YOU DONT WANNA SEE A HP BAR FLOATING ABOVE YOUR OWN HEAD)
        if (!IsOwner)
            characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;

        //  STATS
        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

        //  LOCK ON
        playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;

        //  EQUIPMENT
        playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
        playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

        //  FLAGS
        playerNetworkManager.isChargingAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackChanged;

        //  UPON CONNECTING, IF WE ARE THE OWNER OF THIS CHARACTER, BUT WE ARE NOT THE SERVER, RELOAD OUR CHARACTER DATA TO THIS NEWLY INSTANTIATED CHARACTER
        //  WE DONT RUN THIS IF WE ARE THE SERVER, BECAUSE SINCE THEY ARE THE HOST, THEY ARE ALREADY LOADED IN AND DON'T NEED TO RELOAD THEIR DATA
        if (IsOwner && !IsServer)
        {
            LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;

        //  IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
        if (IsOwner)
        {
            //  UPDATE THE TOTAL AMOUNT OF HEALTH OR STAMINA WHEN THE STAT LINKED TO EITHER CHANGES
            playerNetworkManager.vitality.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;

            //  UPDATES UI STAT BARS WHEN A STAT CHANGES (HEALTH OR STAMINA)
            playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenTimer;
        }

        if (!IsOwner)
            characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;
        //  STATS
        playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHP;

        //  LOCK ON
        playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;

        //  EQUIPMENT
        playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
        playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

        //  FLAGS
        playerNetworkManager.isChargingAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackChanged;
    }

    private void OnClientConnectedCallback(ulong clientID)
    {
        WorldGameSessionManager.instance.AddPlayerToActivePlayersList(this);

        //  IF WE ARE THE SERVER, WE ARE THE HOST, SO WE DONT NEED TO LOAD PLAYERS TO SYNC THEM
        //  YOU ONLY NEED TO LOAD OTHER PLAYERS GEAR TO SYNC IT IF YOU JOIN A GAME THATS ALREADY BEEN ACTIVE WITHOUT YOU BEING PRESENT
        if (!IsServer && IsOwner)
        {
            foreach (var player in WorldGameSessionManager.instance.players)
            {
                if (player != this)
                {
                    player.LoadOtherPlayerCharacterWhenJoiningServer();
                }
            }
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
        }
        respawnCharacter = true;
        deathCount++;
        return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        //  CHECK FOR PLAYERS THAT ARE ALIVE, IF 0 RESPAWN CHARACTERS
    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        if (IsOwner)
        {
            isDead.Value = false;
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;
            playerNetworkManager.transform.localPosition = new Vector3(0, 0, 0);
            //  RESTORE FOCUS POINTS

            //  PLAY REBIRTH EFFECTS
            playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
        }
        
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

        currentCharacterData.vitality = playerNetworkManager.vitality.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;
    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;

        playerNetworkManager.vitality.Value = currentCharacterData.vitality;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;

        //  THIS WILL BE MOVED WHEN SAVING AND LOADING IS ADDED
        playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
        playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
        playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
        playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
    }

    public void LoadOtherPlayerCharacterWhenJoiningServer()
    {
        //  SYNC WEAPONS
        playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
        playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);

        //  ARMOR

        //  LOCK ON
        if (playerNetworkManager.isLockedOn.Value)
        {
            playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
        }
    }

    //  DEBUG DELETE LATER
    private void DebugMenu()
    {
        if (respawnCharacter)
        {
            respawnCharacter = false;
            ReviveCharacter();
        }
    }



    private float HealthPerformance(float health)
    {
        float currentHealth = playerNetworkManager.currentHealth.Value;
        float maxHealth = playerNetworkManager.maxHealth.Value;
        health = currentHealth / maxHealth;
        return health;
    }

    private void AccuracyPerformance()
    {

    }

    private float KillPerformance(float enemyKillsPerformance)
    {
        if (killCount <= 1)
        {
            enemyKillsPerformance = DDA_Difficulty_Manager.instance.score / totalEnemies;
        }
        return enemyKillsPerformance;
    }

    private float LifePerformance(float lifePerformance)
    {
        lifePerformance = 1 / playerDeaths;
        return lifePerformance;
    }

    private void OverallPerformance()
    {
        float w1 = 0.4f;
        float w2 = 0.4f;
        float w3 = 0.2f;
        overallPerformance = (HealthPerformance(pHealth) * w1) + (KillPerformance(pKills) * w2) + (LifePerformance(deathCount) * w3);

    }

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

    //public void AddToKillCount()
    //{

    //    killCount += (1.0f / 4f);


    //    Debug.Log("Kill count: " + killCount);
    //}

    public void AddToPlayerDeathCount()
    {

        playerDeaths += (1.0f / 4f);


        Debug.Log("Kill count: " + playerDeaths);
    }

    
}
