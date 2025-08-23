using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 강화 조건의 종류입니다.
/// </summary>
public enum ConditionType
{
    none,
    playerHealth,           // 플레이어 HP 소모
    StatusEffect,           // 특정 상태이상 소모
    StatusEffectType        // 특정 타입 상태이상 보유량
}

[System.Serializable]
public class Condition_HP
{
    [Tooltip("강화 효과를 1회 적용하는 데 필요한 자원의 양입니다.")]
    public int valuePerApply = 1;

    [Tooltip("강화 효과를 최대로 적용할 수 있는 횟수입니다.")]
    public int maxApplyCount = 1;
}

[System.Serializable]
public class Condition_Effect
{
    [Tooltip("조건의 대상이 적인지, 플레이어인지 확인합니다.")]
    public bool isTargetEnemy = false;

    [Tooltip("소모할 상태이상(패시브) 데이터입니다.")]
    public StatusEffectData statusEffectData;

    [Tooltip("강화 효과를 1회 적용하는 데 필요한 자원의 양입니다.")]
    public int valuePerApply;

    [Tooltip("강화 효과를 최대로 적용할 수 있는 횟수입니다.")]
    public int maxApplyCount = 1;
    //[Tooltip("강화 효과를 대상마다 개별적으로 계산할지 여부입니다.")]
    //public bool enhancePerTarget = false;

    [Tooltip("이 조건을 확인한 후 플레이어의 중첩을 소모할지 여부입니다.")]
    public bool consumeStack = true;
}

[System.Serializable]
public class Condition_EffectType
{
    [Tooltip("조건의 대상이 적인지, 플레이어인지 확인합니다.")]
    public bool isTargetEnemy = false;

    [Tooltip("소모할 상태이상(패시브) 종류(effectType)입니다.")]
    public StatusEffectData.EffectType effectType;

    [Tooltip("강화 효과를 1회 적용하는 데 필요한 자원의 양입니다.")]
    public int valuePerApply = 1;

    [Tooltip("강화 효과를 최대로 적용할 수 있는 횟수입니다.")]
    public int maxApplyCount = 1;
    
    //[Tooltip("강화 효과를 대상마다 개별적으로 계산할지 여부입니다.")]
    //public bool enhancePerTarget = false;

    //[Tooltip("이 조건을 확인한 후 적들의 중첩을 소모할지 여부입니다.")]
    //public bool consumeStack = false;
}

public enum SpecialEffectType
{
    None, TransformStatusEffect
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
    public List<StatusEffectData> effectDatas;     // 효과 분류
    public List<int> effects;            // 효과 수치    (순수 효과 수치)
    public AllyData summonData;     // 소환수 데이터

    [Header("# Enhancement Condition")]
    public ConditionType conditionType;

    public Condition_HP condition_HP;
    public Condition_Effect condition_Effect;
    public Condition_EffectType condition_EffectType;

    [Header("# Additional Effects")]
    public int perDamage;       // 조건당 데미지    (기본값 = 0)
    public int perCount;        // 조건당 타수      (기본값 = 0)
    public int perShield;       // 조건당 보호막 양     (기본값 = 0)
    public int perHeal;         // 조건당 회복량        (기본값 = 0)
    public List<int> perEffect;       // 조건당 효과 수치     (기본값 = 0)
    public AllyData conditionalSummonData;      // 조건부 소환수 데이터
}
