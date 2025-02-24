using UnityEngine;

public class WeaponItem : Item
{
    //  ANIMATOR CONTROLLER OVERRIDE (Change attack animations based on weapon you are currently using)

    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;

    //  WEAPON GUARD ABSORPTIONS (BLOCKING POWER)

    [Header("Weapon Poise")]
    public float poiseDamage = 10;
    //  OFFENSIVE  POISE BONUS WHEN ATTACKING

    [Header("Attack Modifiers")]
    public float light_Attack_01_Modifier = 1.0f;
    public float light_Attack_02_Modifier = 1.2f;
    public float heavy_Attack_01_Modifier = 1.4f;
    public float heavy_Attack_02_Modifier = 1.6f;
    public float charge_Attack_01_Modifier = 2.0f;
    public float charge_Attack_02_Modifier = 2.2f;
    public float running_Attack_01_Modifier = 1.1f;
    public float rolling_Attack_01_Modifier = 1.1f;
    public float backstep_Attack_01_Modifier = 1.1f;

    [Header("Stamina Cost Modifiers")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 1.0f;
    public float heavyAttackStaminaCostMultiplier = 1.3f;
    public float chargedAttackStaminaCostMultiplier = 1.5f;
    public float runningAttackStaminaCostMultiplier = 1.1f;
    public float rollingAttackStaminaCostMultiplier = 1.1f;
    public float backstepAttackStaminaCostMultiplier = 1.1f;


    [Header("Actions")]
    public WeaponItemAction oh_RB_Action;   // ONE HAND RIGHT BUMPER ACTION
    public WeaponItemAction oh_RT_Action;   // ONE HAND RIGHT TRIGGER ACTION
                                            //  ASH OF WAR

    //  BLOCKING SOUNDS
    [Header("Whooshes")]
    public AudioClip[] whooshes;
}
