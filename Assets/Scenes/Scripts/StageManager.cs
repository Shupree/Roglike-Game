using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private GameObject[] optionBtnArr = new GameObject[3];  // 다음 스테이지 선택 버튼

    [Header ("Stage Info")]
    public int stageNum = 0;    // 현재 스테이지 수
    // 현재 스테이지 정보 (0비어있음, 1일반몹, 2엘리트몹, 3보스몹, 4상자, 5이벤트, 6상점)
    public int stageInfo = 0;
    private int randomNum = 0;
    private int[] nextStageInfo = new int[3];

    // 초기화
    public void Init() {
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
        int[][] spInfoArr = new int[][] {       // 강제 스테이지 종류
                new int[]{1,1,0}, new int[]{2,2,0},
                new int[]{1,6,0}, new int[]{3,0,0}
            };
        // 일반몹 스테이지 강제 (첫 스테이지)
        if (stageNum == 1) {
            nextStageInfo = spInfoArr[0];
        }
        // 엘리트몹 스테이지 강제
        else if (stageNum == 3) {
            nextStageInfo = spInfoArr[1];
        }
        // 상점 스테이지 선택 (보스 전 상점)
        else if (stageNum == 5) {
            nextStageInfo = spInfoArr[2];
        }
        // 보스몹 스테이지 강제
        else if (stageNum == 6) {
            nextStageInfo = spInfoArr[3];
        }
        // 스테이지 랜덤 설정
        else {
            int[][] infoArr = new int[][] {     // 다음 스테이지 종류
                new int[]{1,4,0}, new int[]{1,5,0},
                new int[]{1,5,0}, new int[]{1,5,4},
                new int[]{1,6,0}, new int[]{1,5,6}
            };

            //randomNum = UnityEngine.Random.Range(1, 7); // 난수 뽑기
            randomNum = 4;  // 테스트용
            
            nextStageInfo = infoArr[randomNum - 1];     // 다음 스테이지 정보 설정
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
