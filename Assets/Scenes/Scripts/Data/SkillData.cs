using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptble Object/SkillData")]
public class SkillData : ScriptableObject
{
    // 스킬 테이블
    public enum SkillType { Red, Blue, Yellow, White }
    public enum AttackType { Single, Bounce, Splash }

    [Header("# Main Info")]
    public SkillType skillType; // 스킬 색상
    public int paintCondition;  // 요구 물감 수
    public AttackType attackType;   // 공격 방식
    public int skillId;         // 스킬 ID
    public string skillName;    // 스킬 이름
    public string skillDesc;    // 스킬 설명
    public Sprite skillIcon;    // 스킬 아이콘

    [Header("# Skill Data")]
    public int baseDamage;  // 기본 데미지
    public int baseCount;   // 기본 타수 (Multiple의 경우)
    public int baseShield;  // 기본 보호막 양
    public int baseHeal;    // 기본 회복량
    public int effectType;  // 버프/디버프 종류
    public int baseEffect;  // 버프/디버프 수치

    [Header("# Increase Figure")]
    public int incDamage;     // 데미지 계수
    public int incShield;     // 보호막 계수
    public int incHeal;       // 회복 계수
    public int incEffect;     // 버프/디버프 계수
}
