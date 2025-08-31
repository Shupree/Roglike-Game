using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 효과가 '무엇을' 할지를 정의하는 모든 행동의 기반 클래스
/// </summary>
[System.Serializable]
public abstract class CollectionAction : CollectionComponent
{
    public abstract void Execute(EffectContext context);
}

// DealDamageToRandomEnemyAction.cs
/*public class DealDamageToRandomEnemyAction : CollectionAction, IValueProvider
{
    public int damage;
    public override void Execute(EffectContext context)
    {
        // TODO: 실제 게임의 적 관리자에서 랜덤 적을 가져오는 로직 필요
        // var randomEnemy = EnemyManager.Instance.GetRandomEnemy();
        // randomEnemy?.TakeDamage(damage);
        Debug.Log($"랜덤 적에게 {damage} 피해!");
    }
    public override string GetDescription() => $"무작위 적에게 {damage}의 피해";
    public void AddValues(Dictionary<string, string> values) => values["damage"] = damage.ToString();
}
*/
// ... ApplyStatusToAllEnemiesAction, DealDamageToTargetAction 등도 유사하게 구현 ...

