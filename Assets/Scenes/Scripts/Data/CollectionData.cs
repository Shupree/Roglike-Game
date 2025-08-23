using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수집품의 등급을 정의하는 Enum
/// </summary>
public enum CollectionRarity
{
    Common,    // 일반
    Uncommon,  // 고급
    Rare,      // 희귀
    Epic,      // 영웅
    Legendary  // 전설
}

/// <summary>
/// 개별 수집품의 데이터를 정의하는 ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "New Collection", menuName = "Collection/CollectionData")]
public class CollectionData : ScriptableObject
{
    [Header("기본 정보")]
    public string collectionName; // 수집품 이름
    [TextArea]
    public string description;    // 수집품 설명
    public Sprite icon;           // 아이콘
    public CollectionRarity rarity; // 희귀도

    [Header("효과 목록")]
        [SerializeReference] // 자식 클래스의 데이터를 직렬화하기 위해 필요
    public List<CollectionEffect> effects; // 이 수집품이 가진 효과 목록

    /// <summary>
    /// UI에 표시될 최종 설명을 반환합니다.
    /// </summary>
    public string GetFormattedDescription()
    {
        // 설명에 있는 {value} 같은 플레이스홀더를 실제 값으로 교체합니다.
        // 이 예제에서는 첫 번째 효과의 설명을 가져옵니다.
        if (effects != null && effects.Count > 0)
            return effects[0].GetDescription();
        return description;
    }

    /// <summary>
    /// 대상에게 이 수집품의 모든 효과를 적용합니다.
    /// </summary>

    public void Equip(GameObject target)
    {
        foreach(var effect in effects)
        {
            effect.SetSourceCollectionName(this.collectionName);
            effect.Equip(target);
        }
    }

    /// <summary>
    /// 대상에게서 이 수집품의 모든 효과를 제거합니다.
    /// </summary>
    public void Unequip(GameObject target)
    {
        effects?.ForEach(effect => effect.Unequip(target));
    }

    /// <summary>
    /// 전투 시작 시 모든 효과를 트리거합니다.
    /// </summary>
    public void TriggerOnCombatStart(GameObject target) => effects?.ForEach(e => e.OnCombatStart(target));

    /// <summary>
    /// 전투 종료 시 모든 효과를 트리거합니다.
    /// </summary>
    public void TriggerOnCombatEnd(GameObject target) => effects?.ForEach(e => e.OnCombatEnd(target));
}
