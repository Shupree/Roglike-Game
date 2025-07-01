using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeSkill", menuName = "Scriptble Object/ThemeSkillData")]
public class ThemeSkillData : ScriptableObject
{
    // 걸작 스크립터블 오브젝트
    // public enum MP_Color { White, Red, Blue, Yellow, Orange, Purple, Green, Black }
    public enum ConditionType { None, OverCost, Health, Paint, Gold }

    [Header("# Main Info")]
    // public MP_Color MP_class;
    public string MP_Name;
    [TextArea (3, 5)]
    public string desc;         // 테마스킬 정보
    public Sprite icon;
    public string costName;     // 요구하는 스택의 이름 (패시브 이름)
    public int cost;            // 사용 시 스택 수
    public int maxCost;         // 스택 최대치

    [Header("# Basic Ability")]
    public Skill.SkillType skillType;
    public int damage;  // 기본 데미지
    public int count;   // 타수
    public int shield;  // 기본 보호막 양
    public int heal;    // 기본 회복량
    public string[] effectType;    // 효과 분류
    public int[] effect;        // 효과 수치    (순수 효과 수치)
    public AllyData summonData;     // 소환수 데이터

    [Header("# Condition")]
    public ConditionType conditionType;
    public string conditionDetail;  // 'Paint'시, "Red" "Blue" "Yellow" "White"
    public int perCondition;        // 조건 1회 충족당 필요한 수치 : 조건이 다회 적용 가능 시      (기본값 = 1)
    public int maxCondition;    // 조건 최대치 (조건의 최대 중첩 횟수) (기본값 = 1)
    public int perDamage;       // 조건당 데미지    (기본값 = 0)
    public int perCount;        // 조건당 타수      (기본값 = 0)
    public int perShield;       // 조건당 보호막 양     (기본값 = 0)
    public int perHeal;         // 조건당 회복량        (기본값 = 0)
    public AllyData conditionalSummonData;      // 조건부 소환수 데이터
    public int[] perEffect;       // 조건당 효과 수치     (기본값 = 0)
}
