using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSkill", menuName = "Scriptble Object/MonsterSkillData")]
public class MonsterSkillData : ScriptableObject
{
    // 스킬 테이블
    public enum SkillType { Red, Blue, Yellow, White }
    public enum ConditionType { Basic, HP }

    [Header("# Main Info")]
    public SkillType skillType; // 스킬 색상
    public ConditionType conditionType;  // 발동 조건 : 종류
    public int conditionNum;    // 발동 조건 : 수치
    public int availableNum;    // 사용가능 횟수
    public int skillId;         // 스킬 ID
    public string skillName;    // 스킬 이름
    [TextArea (3, 5)]
    public string skillDesc;    // 스킬 설명
    public Sprite skillIcon;    // 스킬 아이콘

    [Header("# Skill Data")]
    public int baseDamage;  // 기본 데미지
    public int baseCount;   // 기본 타수 (Multiple의 경우)
    public int baseShield;  // 기본 보호막 양
    public int baseHeal;    // 기본 회복량
    public int effectType;  // 버프/디버프 종류
    public int baseEffect;  // 버프/디버프 수치
}
