using UnityEngine;

public class InstantCharacterEffects : ScriptableObject
{
    [Header("Effect ID")]
    public int instantEffectID;

    public virtual void ProcessEffect(CharacterManager character)
    {

    }
}
