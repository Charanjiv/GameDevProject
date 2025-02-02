using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.characterCombatManager.currentTarget != null)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            
        }
        else
        {
            //  RETURN THIS STATE, TO CONTINUALLY SEARCH FOR A TARGET (KEEP THE STATE HERE, UNTIL A TARGET IS FOUND)
            aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
            //Debug.Log("SEARCHING FOR A TARGET");
            return this;
        }
    }
}
