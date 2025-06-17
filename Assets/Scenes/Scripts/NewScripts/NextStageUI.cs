using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class NextStageUI : MonoBehaviour
{
    [Header("Reference")]
    public StageManager stageManager;   // 인스펙터 창 참조
    public Image image;

    [Header("Stage Info")]
    public StageManager.StageType stageType;

    [Header("Sprites")]
    public Sprite[] spriteArr = new Sprite[7];
    // 0비어있음, 1일반몹, 2엘리트몹, 3보스몹, 4상자, 5이벤트, 6상점

    void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
    }

    // 다음 스테이지 정보 획득
    public void GetStageInfo(StageManager.StageType stageTypeNum)
    {
        stageType = stageTypeNum;

        // 스프라이트 변경
        image.sprite = spriteArr[(int)stageType];
    }

    // 버튼 클릭 시
    public void ClickStageBtn()
    {
        // GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        if (stageType == StageManager.StageType.none)
        {
            Debug.Log("불가능한 스테이지 진행입니다.");
        }
        else
        {
            stageManager.SetCurrentStage(stageType);
        }

    }
}

