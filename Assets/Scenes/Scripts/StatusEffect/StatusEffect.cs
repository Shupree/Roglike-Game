using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StatusEffect의 구조체들 정의
[System.Serializable]
public class StatusEffect
{
    public StatusEffectData data;   // 상태이상 원본 데이터 (ScriptableObject)
    public int stackCount;          // 현재 중첩 수
    public int duration;            // 남은 지속시간

    public StatusEffectLogic logic { get; private set; } // 이 효과의 실제 행동 로직

    // 생성자
    public StatusEffect(StatusEffectData sourceData)
    {
        data = sourceData;
        stackCount = 0;
        duration = sourceData.duration;

        // 데이터에 정의된 타입에 따라 적절한 로직 클래스를 생성
        switch (sourceData.logicType)
        {
            case StatusEffectData.EffectLogicType.bleed:
                logic = new Bleed_Logic();
                break;
            case StatusEffectData.EffectLogicType.block:
                logic = new Block_Logic();
                break;
            case StatusEffectData.EffectLogicType.weak:
                logic = new Weak_Logic();
                break;
            // ... 다른 상태이상 로직들 ...
            default:
                logic = null;
                break;
        }
    }
}

[System.Serializable]
public class StatusEffectContainer
{
    // Json의 최상위 객체 래핑 (배열의 객체화)
    public List<StatusEffect> StatusEffects;
}