using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptble Object/EnemyData")]
public class EnemyData : ScriptableObject
{
    // 몬스터 스크립터블 오브젝트
    public enum EnemyType { Normal, Elite, Boss }

    [Header("# Main Info")]
    public EnemyType enemyType;
    public int enemyId;
    public string enemyName;
    [TextArea (3, 5)]
    public string enemyDesc;    // 적 정보
    public Sprite enemyModel;
    public int gold;
    public int skillNum;

    [Header("# Ability")]
    public int maxHealth;

    // 스킬은 위에서부터 채워주어야 함.
    // skillNum은 공격 패턴의 개수와 같아야 함.

    // 공격1
    [Header("# Attack01")]
    public int damage_01;
    public int shield_01;
    public int effectType_01;
    public int effectNum_01;
    public int heal_01;

    // 공격2
    [Header("# Attack02")]
    public int damage_02;
    public int shield_02;
    public int effectType_02;
    public int effectNum_02;
    public int heal_02;

    // 공격3
    [Header("# Attack03")]
    public int damage_03;
    public int shield_03;
    public int effectType_03;
    public int effectNum_03;
    public int heal_03;

    // 공격4
    [Header("# Attack04")]
    public int damage_04;
    public int shield_04;
    public int effectType_04;
    public int effectNum_04;
    public int heal_04;

    // 공격5
    [Header("# Attack05")]
    public int damage_05;
    public int shield_05;
    public int effectType_05;
    public int effectNum_05;
    public int heal_05;
}
