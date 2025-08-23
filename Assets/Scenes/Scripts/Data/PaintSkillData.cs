using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PaintSkill", menuName = "Scriptable Object/PaintSkillData")]
public class PaintSkillData : ScriptableObject
{
    // 일반스킬 스크립터블 오브젝트
    public enum SkillType
    {
        SingleAtk, SplashAtk, BounceAtk, SingleSup, SplashSup
    }

    [Header("# Main Info")]
    // public MP_Color MP_class;
    public string skillName;
    public PaintManager.ColorType colorType;
    [TextArea(3, 5)]
    public string desc;      // 스킬 정보
    public Sprite icon;

    [Header("# Basic Ability")]
    public SkillType skillType;
    public int damage;  // 기본 데미지
    public int count;   // 타수
    public int shield;  // 기본 보호막 양
    public int heal;    // 기본 회복량
    public List<StatusEffectData> effectDatas;    // 효과 분류
    public List<int> effects;        // 효과 수치    (순수 효과 수치)

    /*
    [Header("# Condition Info")]
    public ConditionType conditionType;
    public string conditionDetail;  // 'conditionType = Paint'시, 물감 타입 (Red, Blue, Yellow, White)
    public int perNeed;        // 조건 1회 충족당 필요한 수치 : 조건이 다회 적용 가능 시      (기본값 = 1)
    public int maxStack;        // 조건 최대치 (조건의 최대 중첩 횟수) (기본값 = 1)
    */

    [Header("# Additional Effect")]
    public int perDamage;       // 조건당 데미지    (기본값 = 0)
    public int perCount;        // 조건당 타수      (기본값 = 0)
    public int perShield;       // 조건당 보호막 양     (기본값 = 0)
    public int perHeal;         // 조건당 회복량        (기본값 = 0)
    public List<int> perEffect;     // 조건당 효과 수치     (기본값 = 0)
}
