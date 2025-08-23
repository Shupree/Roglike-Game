using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 수집품 효과의 기반이 되는 추상 클래스.
/// </summary>
public abstract class CollectionEffect : ScriptableObject
{
    [System.NonSerialized]
    protected string collectionName; // 디버깅/로그용

    /// <summary>
    /// 이 효과를 소유한 수집품의 이름을 설정합니다.
    /// </summary>
    public void SetSourceCollectionName(string name)
    {
        collectionName = name;
    }

    /// <summary>
    /// 수집품을 획득했을 때 한 번 호출됩니다. (예: 영구 스탯 증가)
    /// </summary>
    public virtual void Equip(GameObject target) { }

    /// <summary>
    /// 수집품을 잃었을 때 한 번 호출됩니다.
    /// </summary>
    public virtual void Unequip(GameObject target) { }

    /// <summary>
    /// 전투가 시작될 때마다 호출됩니다. (이벤트 구독 및 상태 초기화)
    /// </summary>
    public virtual void OnCombatStart(GameObject target) { }

    /// <summary>
    /// 전투가 끝날 때마다 호출됩니다. (이벤트 구독 해제)
    /// </summary>
    public virtual void OnCombatEnd(GameObject target) { }

    /// <summary>
    /// UI에 표시될 동적 설명을 반환합니다.
    /// </summary>
    public abstract string GetDescription();
}

