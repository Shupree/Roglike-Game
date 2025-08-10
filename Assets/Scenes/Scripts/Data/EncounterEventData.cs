using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEncounterEvent", menuName = "Scriptable Object/Encounter Event")]
public class EncounterEventData : ScriptableObject
{
    public string eventId;              // 이벤트 ID
    public string title;                // 이벤트 제목
    [TextArea(3, 6)]
    public string description;          // 이벤트 설명

    public GameObject npcPrefab;        // NPC 프리팹
    public Sprite leftSprite;           // 대사창 왼편 스프라이트
    public Sprite rightSprite;          // 대사창 오른편 스프라이트
    
    // public AudioClip voiceClip;      // NPC 보이스

    public List<DialogueLine> lines;    // 대사집
    public EventOption[] options;       // 이벤트 선택지
}

[System.Serializable]
public class EventOption    // 이벤트 선택지 요소
{
    public string optionText;                   // 선택지 텍스트
    public List<DialogueLine> resultLines;      // 결과 Texts

    public EventReward reward;      // 이벤트 보상
    public EventPenalty penalty;    // 이벤트 패널티
}

[System.Serializable]
public class EventReward    // 이벤트 보상 요소
{
    public int gold;
    public int heal;
    public PaintSkillData rewardSkill;
    public AllyData recruitAlly;
    // 기타 특이 보상들
}

[System.Serializable]       // 이벤트 패널티 요소
public class EventPenalty
{
    public int loseGold;
    public int hpDamage;
    public StatusEffect applyDebuff;
    // 기타 패널티
}
