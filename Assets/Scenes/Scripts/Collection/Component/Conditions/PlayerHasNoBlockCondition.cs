using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHasNoBlockCondition : CollectionCondition
{
    public override bool IsMet(EffectContext context)
    {
        var character = context.Owner.GetComponent<Character>();
        return character != null && character.currentBlock <= 0;
    }
    public override string GetDescription() => "보호막이 없다면";
}
