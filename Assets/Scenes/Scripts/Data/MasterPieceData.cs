using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasterPiece", menuName = "Scriptble Object/MasterPieceData")]
public class MasterPieceData : ScriptableObject
{
    // 걸작 스크립터블 오브젝트
    public enum MP_Class { White, Red, Blue, Yellow, Orange, Purple, Green, Black, Hidden }
    public enum AttackType { Single, Multiple, Splash }
    public enum ConditionType { None, Cost, Health, Paint, Gold }
    public enum PaintType { R, B, Y, W }

    // 걸작 형태
    // 1. 조건부 (물감, 체력, 재화) _ 조건에 따른 효과 변화
    // 2. 공격/디버프/버프
    // 3. 공격형태 (Single, Multiple, Splash)
    // 4. 자신 대상 효과
    // 5. 

    [Header("# Main Info")]
    public MP_Class MP_class;
    public int MP_Id;
    public string MP_Name;
    public string MP_Desc;
    public Sprite MP_Sprite;
    public int cost;

    [Header("# Basic Ability")]
    public AttackType attackType;
    public int basicDamage;  // 기본 데미지
    public int count;   // 타수

    // 00없음 01화상 02중독 03감전 04추위 05빙결 06기절
    // 07공포 08위압 09부식
    // 21철갑 보호막 22집중 23흡수 24가시
    // 51물감 강탈 52만개 스택(전용) 53

    public int baseShield;  // 기본 보호막 양
    public int baseHeal;    // 기본 회복량
    public int effectType;  // 효과 분류
    public int basicEffect;   // 효과 수치    (순수 효과 수치)
    public int self_EffectType;     // 자신에 대한 효과 타입    (없을 시 0)
    public int self_Effect;      // 자신에 대한 효과 수치

    [Header("# Condition")]
    public ConditionType conditionType;
    public PaintType conditionColor;  // Type이 Paint일 시
    public int perCondition;    // 조건 1회 충족당 필요한 수치      (기본값 = 1)
    public int maximumCondition;    // 조건 최대치  (cost: stack 최대치 / Paint류,Gold: 조건 최대 횟수)
                                    // 모든 재고 소모 시 -1값으로 설정함.
    public int perDamage;       // 조건당 데미지    (기본값 = 0)
    public int perShield;       // 조건당 보호막 양     (기본값 = 0)
    public int perHeal;         // 조건당 회복량        (기본값 = 0)
    public int perEffect;       // 조건당 효과 수치     (기본값 = 0)

    [Header("Extra Info")]
    public int extraEffectType;    // 추가 효과
    public int extraEffect;         // 추가 효과 수치
}
