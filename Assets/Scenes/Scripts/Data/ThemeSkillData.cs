using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health_Condition   // 조건 : 플레이어 HP 소모
{
    public int valuePerApply = 1;
}

public class StatusEffect_Condition     // 조건 : 특정 상태이상 혹은 패시브 효과
{
    public List<StatusEffectData> effectDatas;
    public List<int> effects;
}

[System.Serializable]
public class EnhancementCondition
{
    /// 강화 조건의 종류입니다.
    public enum ConditionType
    {
        [Tooltip("플레이어의 체력을 소모하여 스킬을 강화합니다.")]
        health,
        [Tooltip("특정 상태이상 또는 패시브 스택을 소모하여 스킬을 강화합니다.")]
        statusEffect
    }

    [Tooltip("스킬 강화에 사용할 조건 유형입니다.")]
    public ConditionType type;


    [Tooltip("조건 유형이 'statusEffect'일 경우, 소모할 상태이상(패시브) 데이터입니다.")]
    public StatusEffectData statusEffectData;

    [Tooltip("강화 효과를 1회 적용하는 데 필요한 자원의 양입니다.\n" +
             "health: 소모할 체력\n" +
             "paint: 소모할 물감 양\n" +
             "statusEffect: 소모할 스택 수")]
    public int valuePerApply = 1;

    [Tooltip("강화 효과를 최대로 적용할 수 있는 횟수입니다.")]
    public int maxApplyCount = 1;
}

[CreateAssetMenu(fileName = "ThemeSkill", menuName = "Scriptable Object/ThemeSkillData")]
public class ThemeSkillData : ScriptableObject
{
    // 테마스킬 스크립터블 오브젝트

    [Header("# Main Info")]
    public string skillName;
    [TextArea(3, 5)]
    public string desc;         // 테마스킬 정보
    public Sprite icon;
    public List<PaintManager.ColorType> colorTypeList;      // 요구 메인 물감

    [Header("# Basic Ability")]
    public PaintSkillData.SkillType skillType;
    public int damage;  // 기본 데미지
    public int count;   // 타수
    public int shield;  // 기본 보호막 양
    public int heal;    // 기본 회복량
    public StatusEffectData[] effectDatas;     // 효과 분류
    public int[] effect;            // 효과 수치    (순수 효과 수치)
    public AllyData summonData;     // 소환수 데이터

    [Header("# Enhancement Condition")]
    public List<EnhancementCondition> conditions;

    [Header("# Additional Effects")]
    public int perDamage;       // 조건당 데미지    (기본값 = 0)
    public int perCount;        // 조건당 타수      (기본값 = 0)
    public int perShield;       // 조건당 보호막 양     (기본값 = 0)
    public int perHeal;         // 조건당 회복량        (기본값 = 0)
    public int[] perEffect;       // 조건당 효과 수치     (기본값 = 0)
    public AllyData conditionalSummonData;      // 조건부 소환수 데이터
}
