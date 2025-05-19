using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ally", menuName = "Scriptble Object/AllyData")]
public class AllyData : ScriptableObject
{
    // 동료_스크립터블 오브젝트
    // public enum AllyType { Normal, Elite, Boss }

    [Header("# Main Info")]
    // public AllyType allyType;
    public int allyId;
    public string unitName;

    [TextArea(3, 5)]

    public string desc;    // 유닛 정보
    public Sprite model;

    [Header("# Ability")]
    public int maxHealth;

    [Header("# Skill Pattern")]
    public List<SkillPattern> skillPatterns;    // 행동 패턴 리스트
}
