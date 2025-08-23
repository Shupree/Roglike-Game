using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 효과가 '어떤 조건에서' 발동될지를 정의하는 모든 조건의 기반 클래스
/// </summary>
[System.Serializable]
public abstract class CollectionCondition : CollectionComponent
{
    public abstract bool IsMet(EffectContext context);
}

