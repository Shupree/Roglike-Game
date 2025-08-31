using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHasNoBlockCondition : CollectionCondition
{
    public override bool IsMet(EffectContext context)
    {
        return owner != null && owner.GetStatus(StatusInfo.shield) <= 0;
    }
    public override string GetDescription() => "보호막이 없다면";
}
