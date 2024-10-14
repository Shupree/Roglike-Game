using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private GameObject[] optionBtnArr = new GameObject[3];  // 다음 스테이지 선택 버튼

    public int stageNum = 0;    // 현재 스테이지 수
    // 현재 스테이지 정보 (0비어있음, 1일반몹, 2엘리트몹, 3보스몹, 4상자, 5이벤트, 6상점)
    public int stageInfo = 0;
    private int randomNum = 0;
    public int[] nextStageInfo = new int[3];

    // 초기화
    void Awake() {
        for (int i = 0; i < optionBtnArr.Length; i++)
        {
            optionBtnArr[i] = transform.GetChild(i).gameObject;
            optionBtnArr[i].SetActive(false);
        }

        nextStageInfo[0] = 0;
        nextStageInfo[1] = 0;
        nextStageInfo[2] = 0;
    }

    public void SetNextStageInfo() 
    {
        // 일반몹 스테이지 강제 (첫 스테이지)
        if (stageNum == 1) {
            nextStageInfo[0] = 1;
            nextStageInfo[1] = 1;
            nextStageInfo[2] = 0;
        }
        // 엘리트몹 스테이지 강제
        else if (stageNum == 3) {
            nextStageInfo[0] = 2;
            nextStageInfo[1] = 2;
            nextStageInfo[2] = 0;
        }
        // 상점 스테이지 선택 (보스 전 상점)
        else if (stageNum == 5) {
            nextStageInfo[0] = 1;
            nextStageInfo[1] = 6;
            nextStageInfo[2] = 0;
        }
        // 보스몹 스테이지 강제
        else if (stageNum == 6) {
            nextStageInfo[0] = 3;
            nextStageInfo[1] = 0;
            nextStageInfo[2] = 0;
        }
        // 스테이지 랜덤 설정
        else {
            randomNum = UnityEngine.Random.Range(1, 7);

            switch (randomNum) {
                case 1:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 4;
                    nextStageInfo[2] = 0;
                    break;
                case 2:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 5;
                    nextStageInfo[2] = 0;
                    break;
                case 3:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 5;
                    nextStageInfo[2] = 0;
                    break;
                case 4:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 4;
                    nextStageInfo[2] = 5;
                    break;
                case 5:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 6;
                    nextStageInfo[2] = 0;
                    break;
                case 6:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 5;
                    nextStageInfo[2] = 6;
                    break;
            }
        }
        // 버튼 활성화
        optionBtnArr[0].SetActive(true);
        optionBtnArr[1].SetActive(true);
        optionBtnArr[2].SetActive(true);

        // 버튼 정보 수정
        optionBtnArr[0].GetComponent<NextStageUI>().GetStageInfo(nextStageInfo[0]);
        optionBtnArr[1].GetComponent<NextStageUI>().GetStageInfo(nextStageInfo[1]);
        optionBtnArr[2].GetComponent<NextStageUI>().GetStageInfo(nextStageInfo[2]);
    }

    public void OffOptionBtn()
    {
        optionBtnArr[0].SetActive(false);
        optionBtnArr[1].SetActive(false);
        optionBtnArr[2].SetActive(false);
    }
}
