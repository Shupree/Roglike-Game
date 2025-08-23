using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 모듈식으로 조립 가능한 범용 수집품 효과 클래스
/// </summary>
[CreateAssetMenu(fileName = "New Modular Collection Effect", menuName = "Collection/Effects/Modular Effect")]
public class ModularCollectionEffect : CollectionEffect
{
    [SerializeReference, SubclassSelector]
    public CollectionTrigger trigger;

    [SerializeReference, SubclassSelector]
    public List<CollectionCondition> conditions = new List<CollectionCondition>();

    [SerializeReference, SubclassSelector]
    public List<CollectionAction> actions = new List<CollectionAction>();

    public override void OnCombatStart(GameObject target)
    {
        trigger?.OnCombatStart(target);
        trigger?.Register(ExecuteEffect);
        conditions.ForEach(c => c.OnCombatStart(target));
        actions.ForEach(a => a.OnCombatStart(target));
    }

    public override void OnCombatEnd(GameObject target)
    {
        trigger?.Unregister();
        trigger?.OnCombatEnd();
        conditions.ForEach(c => c.OnCombatEnd());
        actions.ForEach(a => a.OnCombatEnd());
    }

    private void ExecuteEffect(EffectContext context)
    {
        if (conditions.All(c => c.IsMet(context)))
        {
            Debug.Log($"[{collectionName}] 효과 발동!");
            actions.ForEach(a => a.Execute(context));
        }
    }

    public override string GetDescription()
    {
        var triggerDesc = trigger?.GetDescription() ?? "알 수 없는 조건일 때";
        var actionDesc = string.Join(", ", actions.Select(a => a.GetDescription()));
        var conditionDesc = conditions.Count > 0 ? $" ({string.Join(", ", conditions.Select(c => c.GetDescription()))})" : "";

        return $"{triggerDesc}, {actionDesc}{conditionDesc}";
    }
}

