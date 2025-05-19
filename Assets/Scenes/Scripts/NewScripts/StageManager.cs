using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public enum StageType
    {
        none, normalB, eliteB, bossB, chest, eEvent, store
    }

    private GameObject[] optionBtnArr = new GameObject[3];  // 다음 스테이지 선택 버튼

    [Header ("Stage Info")]
    private int stageNum;    // 현재 스테이지 수
    // 현재 스테이지 정보 (0비어있음, 1일반몹, 2엘리트몹, 3보스몹, 4상자, 5이벤트, 6상점)
    public StageType curStageType = StageType.none;
    private int randomNum = 0;
    private StageType[] nextStageInfo = new StageType[3];
    private StageType[][] stageArr = new StageType[][] {     // 다음 스테이지 종류
                new StageType[]{StageType.normalB,StageType.store,StageType.none},
                new StageType[]{StageType.normalB,StageType.eEvent,StageType.none},
                new StageType[]{StageType.normalB,StageType.eEvent,StageType.none},
                new StageType[]{StageType.normalB,StageType.eEvent,StageType.store},
                new StageType[]{StageType.normalB,StageType.chest,StageType.none},
                new StageType[]{StageType.normalB,StageType.eEvent,StageType.chest}
            };
    [Header ("EnemyId Info")]
    private int[] enemyIdArr = new int[4]{0,0,0,0};     // 임시 몬스터ID 저장용

    // 초기화
    public void Awake() {
        stageNum = 1;  // 스테이지 넘버 가져오기

        for (int i = 0; i < optionBtnArr.Length; i++)
        {
            optionBtnArr[i] = transform.GetChild(i).gameObject;
            optionBtnArr[i].SetActive(false);
        }

        nextStageInfo[0] = 0;
        nextStageInfo[1] = 0;
        nextStageInfo[2] = 0;
    }

    // 다음 스테이지 선택지 3종류 결정
    public void SetNextStageInfo() 
    {
        // 일반몹 스테이지 강제 (첫 스테이지)
        if (stageNum == 1) {
            nextStageInfo = new StageType[]{StageType.normalB,StageType.normalB,StageType.none};
        }
        // 엘리트몹 스테이지 강제
        else if (stageNum == 3) {
            nextStageInfo = new StageType[]{StageType.eliteB,StageType.eliteB,StageType.none};
        }
        // 상점 스테이지 선택 (보스 전 상점)
        else if (stageNum == 5) {
            nextStageInfo = new StageType[]{StageType.normalB,StageType.store,StageType.none};
        }
        // 보스몹 스테이지 강제
        else if (stageNum == 6) {
            nextStageInfo = new StageType[]{StageType.bossB,StageType.none,StageType.none};
        }
        // 스테이지 랜덤 설정
        else {
            randomNum = UnityEngine.Random.Range(1, 7);     // 난수 뽑기
            //randomNum = 4;  // 테스트용
            
            nextStageInfo = stageArr[randomNum - 1];         // 다음 스테이지 정보 설정
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

    // 스테이지 선택지 가리기
    public void OffOptionBtn()
    {
        optionBtnArr[0].SetActive(false);
        optionBtnArr[1].SetActive(false);
        optionBtnArr[2].SetActive(false);
    }

    // 스테이지 적용
    public void SetCurrentStage(StageType stageType)
    {
        // 스테이지 정보 입력
        curStageType = stageType;
        OffOptionBtn();

        switch (stageType) {
            // 일반 몹 스테이지
            case StageType.normalB:
                // 적 정보 획득 및 적용 (임시)
                enemyIdArr[0] = 1;
                enemyIdArr[1] = 2;
                GameManager.instance.spawnManager.EnemySpawn(enemyIdArr);

                GameManager.instance.turnManager.BattleStart();      // 전투 시작
                /*
                int randomSetNum = UnityEngine.Random.Range(0, 3);
                Debug.Log(randomSetNum);
                enemyID[0] = EnemySetData[randomSetNum].monster01;
                enemyID[1] = EnemySetData[randomSetNum].monster02;
                enemyID[2] = EnemySetData[randomSetNum].monster03;
                enemyID[3] = EnemySetData[randomSetNum].monster04;
                
                BattleStart();      // 전투 시작
                */
                break;
            // 보물
            case StageType.chest:
                // 보물상자 스폰
                GameManager.instance.spawnManager.TreasureChestSpawn(1);    // Common급
                break;
            // 상점
            case StageType.store:
                // 상점NPC 스폰
                GameManager.instance.spawnManager.StoreNPCSpawn();
                break;
        }

        GameManager.instance.stage++;    // 스테이지 + 1
        Debug.Log("스테이지 : "+stageNum);
    }

    
    /*
    public void SetNextStageUI()
    {
        _StageManager.SetNextStageInfo();   // 다음 스테이지 선택지 설정
    }
    */
}
