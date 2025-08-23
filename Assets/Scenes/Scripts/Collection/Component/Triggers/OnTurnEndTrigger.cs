using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnTurnEndTrigger : CollectionTrigger
{
    public override void OnCombatStart(GameObject owner)
    {
        base.OnCombatStart(owner);
        GameEventManager.OnPlayerTurnEnd += HandleEvent;
    }
    public override void OnCombatEnd()
    {
        GameEventManager.OnPlayerTurnEnd -= HandleEvent;
        base.OnCombatEnd();
    }
    private void HandleEvent()
    {
        onTriggerCallback?.Invoke(new EffectContext(owner));
    }
    public override string GetDescription() => "턴 종료 시";
}
