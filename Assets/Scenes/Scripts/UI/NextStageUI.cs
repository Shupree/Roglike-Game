using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class NextStageUI : MonoBehaviour
{
    public GameObject stageInfoUI;
    public int stageType = 0;
    public Sprite[] spriteArr = new Sprite[6];
    // 0비어있음, 1일반몹, 2엘리트몹, 3보스몹, 4상자, 5이벤트, 6상점

    void Awake()
    {

    }

    // 다음 스테이지 정보 획득
    public void GetStageInfo(int stageTypeNum)
    {
        stageType = stageTypeNum;

        // 스프라이트 변경
        stageInfoUI.GetComponent<Image>().sprite = spriteArr[stageType];
    }

    // 버튼 클릭 시
    public void ClickStageBtn(int btnOrder) 
    {
        // GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        if (stageType == 0) {
            Debug.Log("불가능한 스테이지 진행입니다.");
        }
        else {
            switch (btnOrder) {
                case 1:
                    GameManager.instance.SetCurrntStage(stageType);
                    break;
                case 2:
                    GameManager.instance.SetCurrntStage(stageType);
                    break;
                case 3:
                    GameManager.instance.SetCurrntStage(stageType);
                    break;
            }
        }
    }
}
