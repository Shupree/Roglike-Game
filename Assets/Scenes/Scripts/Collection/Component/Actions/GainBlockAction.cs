using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GainBlockAction : CollectionAction
{
    public int amount;
    public override void Execute(EffectContext context)
    {
        Debug.Log($"<b><color=orange>[2] 플레이어가 보호막을 {amount} 획득!!");
        context.Owner.TakeShield(amount);
    }
    public override string GetDescription() => $"보호막 {amount} 획득";
}
