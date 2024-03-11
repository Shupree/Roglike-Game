using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptble Object/SkillData")]
public class SkillData : ScriptableObject
{
    // 스킬 테이블
    public enum SkillType { Red, Blue, Yellow }

    [Header("# Main Info")]
    public SkillType skillType;
    // 공격 타입 : Single, Multiple, Splash
    public string attackType;
    public int skillId;
    public string skillName;
    // Basic, Spark, frozen, fear, stun
    public string skillDesc;
    public Sprite skillIcon;

    [Header("# Skill Data")]
    public int baseDamage;
    public int baseShield;
    public int baseCount;
    // 0없음, 1화상, 2중독, 3감전, 4추위, 5빙결, 6집중
    public int effectType;
    public int baseEffect;
    public int[] damages;
    public int[] shields;
    public int[] counts;
    public int[] effects;
}
