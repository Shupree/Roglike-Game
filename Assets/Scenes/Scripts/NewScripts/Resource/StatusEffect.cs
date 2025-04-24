using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Statement의 구조체들 정의
[System.Serializable]
public class StatusEffect
{
    public string Name { get; set; }        // 이름
    public bool Type { get; set; }          // 버프 / 디버프 (Buff or Debuff)
    public int Duration { get; set; }       // 지속 턴 수
    public int Stackable { get; set; }      // 최대 중첩 수
    public int StackCount { get; set; }     // 중첩 횟수
    public int DecreaseRate { get; set; }   // 매턴 감소율 (Ex: 50 = 50%, 33 = 33%) (매턴 최소 감소량 2)
    public string EffectType { get; set; }  // 효과 유형 (Ex: "BonusDamange", "TurnSkip" 등)
    public int EffectValue { get; set; }    // 효과 값 (Ex: 추가 데미지 값)

    public void ApplyEffect(ITurn target)
    {
        // 상태 효과 적용 로직
        Debug.Log($"{target}에게 {Name} 효과를 적용합니다.");
    }

    public void RemoveEffect(ITurn target)
    {
        // 상태 효과 제거 로직
        Debug.Log($"{target}에서 {Name} 효과를 제거합니다.");
    }
}