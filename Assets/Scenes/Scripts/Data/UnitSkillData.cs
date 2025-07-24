using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSkill", menuName = "Scriptable Object/UnitSkillData")]
public class UnitSkillData : ScriptableObject
{
    // 일단 동료(아군)한테 버프 혹은 회복은 걸어주지 못한다는 전제 하에

    // 몬스터 스킬 테이블
    // public enum SkillType { Red, Blue, Yellow, White }
    public enum SkillType { SingleAtk, SplashAtk, SingleSup, SplashSup }    // Bounce 제외

    [Header("# Main Info")]
    public string skillName;    // 스킬 이름
    public string skillDesc;    // 스킬 설명
    public SkillType skillType; // 스킬 타입
    public Sprite skillIcon;    // 스킬 아이콘

    [Header("# Skill Data")]
    public int damage;      // 기본 데미지
    public int count;       // 기본 타수 (Multiple의 경우)
    public int shield;      // 기본 보호막 양
    public int heal;        // 기본 회복량
    public string effectType;      // 버프/디버프 종류
    // None, Burn, Freeze, ElectricShock
    public int effect;      // 버프/디버프 수치*/
}
