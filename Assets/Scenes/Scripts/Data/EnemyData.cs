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
    public int maxHealth;

    // EffectType
    // 00없음 01화상 02중독 03감전 04추위 05빙결 06기절 07공포 08위압 09부식
    // 21철갑 보호막 22집중 23흡수 24가시

    // 공격1
    [Header("# Attack01")]
    public int attackDamage_01;
    public int attackEffect_01;
    public int effectNum_01;

    // 공격2
    [Header("# Attack02")]
    public int attackDamage_02;
    public int attackEffect_02;
    public int effectNum_02;

    // 공격3
    [Header("# Attack03")]
    public int attackDamage_03;
    public int attackEffect_03;
    public int effectNum_03;

    // 공격4
    [Header("# Attack04")]
    public int attackDamage_04;
    public int attackEffect_04;
    public int effectNum_04;

    // 플레이어에게 특수 디버프 부여
    [Header("# PlayerDebuff")]
    public int playerEffectType;
}
