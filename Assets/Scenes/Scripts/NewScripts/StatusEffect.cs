using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Statement의 구조체들 정의
[System.Serializable]
public class StatusEffect
{
    public string name;            // 이름
    public string type;           // 버프 / 디버프 (Buff or Debuff)
    public int duration;         // 지속 턴 수
    public int stackable;        // 최대 중첩 수
    public int stackCount;        // 중첩 횟수
    public int decreaseRate;     // 매턴 감소율 (Ex: 50 = 50%, 33 = 33%) (매턴 최소 감소량 2)
    public string effectType;    // 효과 유형 (Ex: "BonusDamange", "TurnSkip" 등)
    public int effectValue;      // 효과 값 (Ex: 추가 데미지 값)

    public void ApplyEffect(ITurn target)
    {
        // 상태 효과 적용 로직
        Debug.Log($"{target}에게 {name} 효과를 적용합니다.");
    }

    public void RemoveEffect(ITurn target)
    {
        // 상태 효과 제거 로직
        Debug.Log($"{target}에서 {name} 효과를 제거합니다.");
    }
}

[System.Serializable]
public class StatusEffectContainer
{
    // Json의 최상위 객체 래핑 (배열의 객체화)
    public List<StatusEffect> StatusEffects;
}