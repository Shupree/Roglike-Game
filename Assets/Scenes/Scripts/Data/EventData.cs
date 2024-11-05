using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterEvent", menuName = "Scriptble Object/EventData")]
public class EncounterEventData : ScriptableObject
{
    // 이벤트 테이블
    //public enum EventType { Public, Encounter, , White }
    //public enum EncounterType { NPC, Enemy }

    [Header("# Main Info")]
    //public EventType skillType;
    public int eventId;         // 이벤트 ID
    public string eventName;    // 이벤트 이름
    public GameObject encounterObject;   // 조우 대상 프리팹
    public Dictionary<int, string[]> dialogArr;    // 대사 Array

    [Header("# Skill Data")]
    public int baseDamage;  // 기본 데미지
    public int baseCount;   // 기본 타수 (Multiple의 경우)
    public int baseShield;  // 기본 보호막 양
    public int baseHeal;    // 기본 회복량
    public int effectType;  // 버프/디버프 종류
    public int baseEffect;  // 버프/디버프 수치

    [Header("# Increase Figure")]
    public int incDamage;     // 데미지 계수
    public int incShield;     // 보호막 계수
    public int incHeal;       // 회복 계수
    public int incEffect;     // 버프/디버프 계수
}
