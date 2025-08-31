using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnTurnEndTrigger : CollectionTrigger
{
    public override void OnBattleStart(IUnit owner)
    {
        Debug.Log($"<b><color=orange>[2] 수집품 활성화!");
        base.OnBattleStart(owner);
        BattleEventManager.OnPlayerTurnEnd += HandleEvent;
    }
    public override void OnBattleEnd()
    {
        Debug.Log($"<b><color=orange>[2] 수집품 비활성화!");
        BattleEventManager.OnPlayerTurnEnd -= HandleEvent;
        base.OnBattleEnd();
    }
    private void HandleEvent()
    {
        Debug.Log($"<b><color=orange>[2] 수집품 발동!");
        onTriggerCallback?.Invoke(new EffectContext(owner));
    }
    public override string GetDescription() => "턴 종료 시";
}
