using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 효과의 트리거, 조건, 액션 간에 데이터를 전달하는 컨텍스트 클래스
/// </summary>
public class EffectContext
{
    public IUnit Owner { get; }    // 효과를 소유한 유닛 (주로 플레이어)
    public Dictionary<string, object> Data { get; } = new Dictionary<string, object>(); // 이벤트 관련 추가 데이터

    public EffectContext(IUnit owner)
    {
        Owner = owner;
    }
}

