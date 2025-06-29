using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Statement의 구조체들 정의
[System.Serializable]
public class StatusEffect
{
    public string name;             // 이름
    public string desc;             // 설명
    public string icon;             // 아이콘
    public string type;             // 버프 / 디버프 (Buff or Debuff)
    public int maxStack;            // 최대 중첩 수
    public int stackCount;          // 중첩 수
    public bool isConsumable;       // 사용 시 소모되는 버프/디버프인가?
    public int duration;            // 지속 턴 수 / 사용 시 소모 수 (-1일 시, 영구 버프)
    public string whenIsTrigger;    // 발동 시점 (startTurn, endTurn, whenHit, whenAttack, always)
    public string effectInfo;       // 효과 정보 (statUP, substitute, convert, heal, reflection)
    public string efffectDetail;    // 효과 상세 (statUP - attack, shield / substitute - red, blue, HP / change - 타 상태이상)
    public int needStack;           // 발동 스택 수

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