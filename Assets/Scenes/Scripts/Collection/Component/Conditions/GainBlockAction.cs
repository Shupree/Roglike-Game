using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GainBlockAction : CollectionAction
{
    public int amount;
    public override void Execute(EffectContext context)
    {
        context.Owner.GetComponent<Character>()?.AddBlock(amount);
    }
    public override string GetDescription() => $"보호막 {amount} 획득";
}
