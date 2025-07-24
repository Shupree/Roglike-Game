using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EncounterEventManager : MonoBehaviour
{
    public LogManager logManager;

    public List<EncounterEventData> allEventList;

    // 랜덤 이벤트 적용
    public void TriggerRandomEvent(StageManager.ArtbookType artbook)
    {
        var eventList = GetEventListByStage(artbook);       // 랜덤 이벤트 받기
        EncounterEventData data = eventList[Random.Range(0, eventList.Count)];      // EncounterEventData로 변환

        GameManager.instance.spawnManager.SpawnPrefab(data.npcPrefab);
        ShowEventLog(data);     // 이벤트 로그 활성화
    }

    // 화집에 따른 랜덤 이벤트 추출
    private List<EncounterEventData> GetEventListByStage(StageManager.ArtbookType artbook)
    {
        return allEventList.Where(e => IsValidForStage(e, artbook)).ToList();
    }

    // 특정 이벤트의 등장 조건 확인
    private bool IsValidForStage(EncounterEventData data, StageManager.ArtbookType artbook)
    {
        // 필요시 stage 조건 등 추가
        return true;
    }

    // 로그 활성화
    private void ShowEventLog(EncounterEventData data)
    {
        if (data.leftSprite != null)
        {
            logManager.ConvertSprite(data.leftSprite, true);
        }
        else
        {
            logManager.ConvertSprite(null, true);
        }

        if (data.rightSprite != null)
        {
            logManager.ConvertSprite(data.rightSprite, false);
        }
        else
        { 
            logManager.ConvertSprite(null, false);
        }

        logManager.LogAction(data.lines.ToArray());     // LogManager 호출
    }

    // 선택지 선택
    public void SelectOption(int optionIndex, EncounterEventData data)
    {
        EventOption option = data.options[optionIndex];

        ApplyReward(option.reward);
        ApplyPenalty(option.penalty);

        // 결과 텍스트 출력 등
    }

    // 선택지 보상
    private void ApplyReward(EventReward reward)
    {
        // 보상 적용 (골드, 힐 등)
    }

    // 선택지 패널티
    private void ApplyPenalty(EventPenalty penalty)
    {
        // 패널티 적용
    }
}
