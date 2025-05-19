using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class NextStageUI : MonoBehaviour
{
    [Header ("Reference")]
    private StageManager stageManager;
    public Image image;

    [Header ("Stage Info")]
    public StageManager.StageType stageType;

    [Header ("Sprites")]
    public Sprite[] spriteArr = new Sprite[6];
    // 0비어있음, 1일반몹, 2엘리트몹, 3보스몹, 4상자, 5이벤트, 6상점

    void Awake()
    {
        stageManager = GameManager.instance.stageManager;
    }

    // 다음 스테이지 정보 획득
    public void GetStageInfo(StageManager.StageType stageTypeNum)
    {
        stageType = stageTypeNum;
        int imgNum = 0;

        // 스프라이트 변경
        switch (stageType) {
            case StageManager.StageType.none:
                imgNum = 0;     // 흠..
                break;
            case StageManager.StageType.normalB:
                imgNum = 1;
                break;
            case StageManager.StageType.eliteB:
                imgNum = 2;
                break;
            case StageManager.StageType.bossB:
                imgNum = 3;
                break;
            case StageManager.StageType.chest:
                imgNum = 4;
                break;
            case StageManager.StageType.eEvent:
                imgNum = 5;
                break;
            case StageManager.StageType.store:
                imgNum = 6;
                break;
        }

        image.sprite = spriteArr[imgNum];
        // image.sprite = spriteArr[stageType];
    }

    // 버튼 클릭 시
    public void ClickStageBtn() 
    {
        // GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        if (stageType == StageManager.StageType.none) {
            Debug.Log("불가능한 스테이지 진행입니다.");
        }
        else {
            stageManager.SetCurrentStage(stageType);
        }
        /*
        else {
            switch (btnOrder) {
                case 1:
                    stageManager.SetCurrentStage(stageType);
                    break;
                case 2:
                    GameManager.instance.SetCurrntStage(stageType);
                    break;
                case 3:
                    GameManager.instance.SetCurrntStage(stageType);
                    break;
            }
        }
        */
    }
}

