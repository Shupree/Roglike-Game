using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptble Object/SkillData")]
public class SkillData : ScriptableObject
{
    // 스킬 테이블
    public enum SkillType { Single, Multiple, Splash }

    [Header("# Main Info")]
    public SkillType skillType;
    public int skillId;
    public string skillName;
    // Basic, Spark, frozen, fear, stun
    public string skillDesc;
    public Sprite skillIcon;

    [Header("# Enforce Data")]
    public int baseDamage;
    public int baseCount;
    public int baseEffect;
    public int[] damages;
    public int[] counts;
    public int[] effects;
}
