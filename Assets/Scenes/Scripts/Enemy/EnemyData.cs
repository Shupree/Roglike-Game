using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptble Object/EnemyData")]
public class EnemyData : ScriptableObject
{
    // 몬스터 테이블
    public enum EnemyType { Normal, Elite, Boss }

    [Header("# Main Info")]
    public EnemyType enemyType;
    public int enemyId;
    public string enemyName;
    public string enemyDesc;
    public Sprite enemyModel;

    [Header("# Ability")]
    public float maxHealth;

    [Header("# EffectType")]
    // 공격1, 2, 3에 부여되는 디버프
    public string effectType;

    // 공격1
    [Header("# Attack01")]
    public float attackDamage_01;
    public float attackEffect_01;

    // 공격2
    [Header("# Attack02")]
    public float attackDamage_02;
    public float attackEffect_02;

    // 공격3
    [Header("# Attack03")]
    public float attackDamage_03;
    public float attackEffect_03;

    // 자신에게 버프 부여
    [Header("# SelfBuff")]
    public float selfEffect;
    public float selfEffectType;

    // 플레이어에게 특수 디버프 부여
    [Header("# PlayerDebuff")]
    public float playerEffectType;
}
