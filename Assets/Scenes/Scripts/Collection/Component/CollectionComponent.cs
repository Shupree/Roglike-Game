using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 동적 설명을 위한 값을 제공하는 컴포넌트를 위한 인터페이스
/// </summary>
public interface IValueProvider
{
    void AddValues(Dictionary<string, string> values);
}

/// <summary>
/// 모든 효과 컴포넌트(Trigger, Condition, Action)의 기반이 되는 추상 클래스
/// </summary>
[System.Serializable]
public abstract class CollectionComponent
{
    [System.NonSerialized]
    protected IUnit owner;

    /// <summary>
    /// 전투 시작 시 호출되어 상태를 초기화하거나 이벤트를 구독합니다.
    /// </summary>
    public virtual void OnBattleStart(IUnit owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// 전투 종료 시 호출되어 구독한 이벤트를 해제합니다.
    /// </summary>
    public virtual void OnBattleEnd()
    {
        this.owner = null;
    }

    public abstract string GetDescription();
}
