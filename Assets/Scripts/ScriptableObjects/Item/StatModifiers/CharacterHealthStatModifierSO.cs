using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Stat Modifier", menuName = "ScriptableObjects/Modifiers/Health Modifier")]

public class CharacterHealthStatModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        CharacterStats stats = character.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.AddHealth((int)val);
        }
    }
}
