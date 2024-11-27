using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();

    }

    protected override void Update()
    {
        base.Update();

        //  IF WE DO NOT OWN THIS GAMEOBJECT, WE DO NOT CONTROL OR EDIT IT
        if (!IsOwner)
            return;

        //  HANDLE MOVEMENT
        playerLocomotionManager.HandleAllMovement();
    }
}
