using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 효과가 '언제' 발동될지를 정의하는 모든 트리거의 기반 클래스
/// </summary>
[System.Serializable]
public abstract class CollectionTrigger : CollectionComponent
{
    protected System.Action<EffectContext> onTriggerCallback;

    public void Register(System.Action<EffectContext> callback) => onTriggerCallback = callback;
    public void Unregister() => onTriggerCallback = null;
}

// OnResourceUsedTrigger.cs
public class OnResourceUsedTrigger : CollectionTrigger, IValueProvider
{
    public ResourceType resourceType;
    public int amountToTrigger;
    private int currentAmount;

    public override void OnBattleStart(IUnit owner)
    {
        base.OnBattleStart(owner);
        currentAmount = 0;
        BattleEventManager.OnResourceUsed += HandleEvent;
    }
    public override void OnBattleEnd()
    {
        BattleEventManager.OnResourceUsed -= HandleEvent;
        base.OnBattleEnd();
    }
    private void HandleEvent(ResourceType type, int amount)
    {
        if (type == resourceType)
        {
            currentAmount += amount;
            if (currentAmount >= amountToTrigger)
            {
                currentAmount -= amountToTrigger;
                onTriggerCallback?.Invoke(new EffectContext(owner));
            }
        }
    }
    public override string GetDescription() => $"{resourceType} {amountToTrigger}개 사용 시";
    public void AddValues(Dictionary<string, string> values) => values["amountToTrigger"] = amountToTrigger.ToString();
}

// ... OnCombatStartTrigger, OnStatusAppliedToEnemyTrigger 등도 유사하게 구현 ...

