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
    // 00없음 01화상 02중독 03감전 04추위 05빙결 06기절
    // 11공포 12위압 13부식
    // 21철갑 보호막 22집중 23흡수 24가시
    public int effectType;
    public int baseEffect;
    public int[] damages;
    public int[] shields;
    public int[] counts;
    public int[] effects;
}
