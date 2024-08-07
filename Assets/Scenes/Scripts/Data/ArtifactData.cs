using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artifact", menuName = "Scriptble Object/ArtifactData")]
public class ArtifactData : ScriptableObject
{
    // 스킬 테이블
    public enum ArtifactType { Common, Rare, Unique, Cursed }

    [Header("# Main Info")]
    public ArtifactType artifactType;   // 유물 등급
    public Sprite sprite;               // 유물 아이콘
    public string target;               // 대상 : Player, Target, All
    public int damage;                  // 데미지
    public int shield;                  // 보호막
    public int effectType;              // 버프/디버프 타입
    public int effect;                  // 버프/디버프 수치
    public int eraseEffectType;         // 지울 버프/디버프 타입
    public int eraseEffect;             // 지울 버프/디버프 수치
    public int numOfUse;                // 사용 횟수    (-1은 무한)
    public bool oneTime;                // 1회성 소멸


    // 00없음 01화상 02중독 03감전 04추위 05빙결 06기절
    // 07공포 08위압 09부식
    // 21철갑 22집중 23흡수 24가시

    [Header("# Condition")]
    // 주의 : conditionType이 EnemyEffect일 시 Trigger로 OnHit, StartTurn만 사용가능함. 
    public string whenToTrigger;    // 전투 시 발동 시점 (Encounter, OnHit, GetHit, StartTurn, Victory, Defeat, UseMP, EnterStore, EnterElite)
    public string conditionType;    // None, PlayerEffect, EnemyEffect, Health, Paint, Gold
    public int conditionEffect;     // Type이 PlayerEffect 혹은 EnemyEffect일 시 : 이펙트 넘버
    public string conditionColor;   // Type이 Paint일 시 : R, B, Y, W
    public int conditionNum;     // 조건 수치        (기본값 = 0)
}