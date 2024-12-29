using UnityEngine;

public class PlayerCombatManager : CharacterCombatManeger
{
    PlayerManager player;

    public WeaponItem currentWeaponBeingUsed;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
        //  PERFORM THE ACTION
        weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

        //  NOTIFY THE SERVER WE HAVE PERFORMED THE ACTION, SO WE PERFORM IT FROM THEIR PERSPECTIVE ALSO
    }
}
