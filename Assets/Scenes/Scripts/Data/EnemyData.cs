using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillPattern
{
    public UnitSkillData skill;     // 적 스킬 데이터
    public int turnNumber;      // 특정 턴에만 사용
    public float probability;   // 확률 기반 선택
    public int healthPoint;     // HP가 n% 이하일 시 사용
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptble Object/EnemyData")]
public class EnemyData : ScriptableObject
{
    // 몬스터_스크립터블 오브젝트
    public enum EnemyType { Normal, Elite, Boss }

    [Header("# Main Info")]
    public EnemyType enemyType;
    public int enemyId;
    public string unitName;

    [TextArea(3, 5)]

    public string desc;    // 적 정보
    public Sprite model;
    public int gold;

    [Header("# Ability")]
    public int maxHealth;

    [Header("# Skill Pattern")]
    public List<SkillPattern> skillPatterns;    // 행동 패턴 리스트
}
