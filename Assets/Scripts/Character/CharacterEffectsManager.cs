using UnityEngine;


public class CharacterEffectsManager : MonoBehaviour
{
    //  PROCESS INSTANT EFFECTS (TAKE DAMAGE, HEAL)

    //  PROCESS TIMED EFFECTS (POISON, BUILD UPS)

    //  PROCESS STATIC EFFECTS (ADDING/REMOVING BUFFS FROM TALISMANS ECT)

    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffects effect)
    {
        effect.ProcessEffect(character);
    }
}

