using UnityEngine;

public class CharacterCombatManeger : MonoBehaviour
{
    [Header("Attack Target")]
    public CharacterManager currentTarget;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    protected virtual void Awake()
    {

    }
}
