using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 전투 승리 보상 버튼UI (StageManager 내에서 관리)
public class LootManager : MonoBehaviour
{
    // 보상 종류
    public enum LootType
    {
        gold, skill, collection, masterPiece
    }

    [Header("Reference")]
    public StageManager stageManager;

    [Header("UI")]
    public GameObject lootUI;   // 메인 오브젝트
    private GameObject[] UIArr = new GameObject[3];     // 전리품 UI 버튼
    private GameObject loot_SkillUI;        // 전리품_스킬 선택 시 열리는 선택지 UI
    private GameObject loot_SkillSkipBtn;   // 전리품_스킬 스킵 버튼
    private GameObject[] loot_SkillUIArr = new GameObject[3];       // 전리품_스킬 선택지 UI 버튼

    [Header("Sprite")]
    public Sprite[] spriteArr = new Sprite[2];              // 전리품별 아이콘 스프라이트Arr

    [Header("Loot Info")]
    private List<(LootType, int)> lootBtnList = new List<(LootType, int)>();    // 버튼별 보상 종류 List((보상 종류, 보상 정보))
    private int order;      // 전리품 수

    private List<Skill> loot_skillList = new List<Skill>(); // 보상으로 등장하는 스킬 리스트

    void Awake()
    {
        // UI 불러오기
        for (int i = 0; i < UIArr.Length; i++) 
        {
            UIArr[i] = lootUI.transform.GetChild(1).GetChild(i).gameObject;
            UIArr[i].SetActive(false);
        }
        loot_SkillUI = lootUI.transform.GetChild(4).gameObject;
        for (int i = 0; i < loot_SkillUIArr.Length; i++)
        {
            loot_SkillUIArr[i] = loot_SkillUI.transform.GetChild(0).GetChild(i).gameObject;
            loot_SkillUIArr[i].SetActive(false);
        }
        loot_SkillSkipBtn = lootUI.transform.GetChild(5).gameObject;

        // 초기화
        loot_skillList = new List<Skill>();
        loot_SkillUI.SetActive(false);
        loot_SkillSkipBtn.SetActive(false);

        DeactivateAllUI();
    }

    // UI 기능 정지 (이미지, 기능 전부)
    public void DeactivateAllUI()
    {
        // 초기화
        lootBtnList = new List<(LootType, int)>();  // 버튼 정보 초기화
        lootUI.SetActive(false);
        order = 0;      // 전리품 초기화
    }

    // 보상UI 활성화 : (LootType 보상 종류, int 보상 정보) (보상 정보 : 골드 수, 스킬 ???, 장신구ID)
    // 장신구 detail : -10.랜덤, -9.Normal랜덤, -8.Rare랜덤, -7.Unique랜덤, -6.Cursed랜덤, 정수.장신구ID
    public void SetLootUI((LootType, int) lootTuple)
    {
        // UI 활성화
        lootUI.SetActive(true);
        UIArr[order].SetActive(true);
        
        lootBtnList.Add((lootTuple.Item1, lootTuple.Item2));    // 버튼별 Loot 정보 저장

        // 해당 UI 이미지 변경
        switch (lootTuple.Item1)
        {
            case LootType.gold:     // 골드 보상
                UIArr[order].transform.GetChild(0).GetComponent<Image>().sprite = spriteArr[0];     // 스프라이트 변경
                UIArr[order].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = lootTuple.Item2.ToString() + " Golds";    // 텍스트 설정
                break;
            case LootType.skill:     // 일반스킬 보상
                UIArr[order].transform.GetChild(0).GetComponent<Image>().sprite = spriteArr[1];     // 스프라이트 변경
                UIArr[order].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Skills";     // 텍스트 설정
                break;
                /*case LootType.collection:     // 장신구 보상
                    lootBtnList.Add((lootTuple.Item1, lootTuple.Item2));
                    if (lootTuple.Item2 < 0) {       // 랜덤 보상
                        ArtifactData artifactData = GameManager.instance._ArtifactManager.PickRandomArtifact(detail + 10);
                        UIArr[order].transform.GetChild(0).GetComponent<Image>().sprite
                            = artifactData.AritfactIcon;
                        UIArr[order].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
                            = artifactData.name;
                        lootDetail[order] = artifactData.ArtifactId;    // 지급받을 장신구의 ID 저장
                    }
                    else {                  // 특정 보상
                        UIArr[order].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite
                            = GameManager.instance._ArtifactManager.all_ArtifactData[detail].AritfactIcon;
                        UIArr[order].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
                            = GameManager.instance._ArtifactManager.all_ArtifactData[detail].name;
                        lootDetail[order] = detail;     // 지급받을 장신구의 ID 저장
                    }
                    break;
                */
                //case LootType.masterPiece:     // 걸작 보상
                //    break;
        }
        order += 1;
    }

    // 보상 버튼 클릭 시
    public void Click_LootBtn(int btnOrder)
    {
        switch (lootBtnList[btnOrder - 1].Item1) {
            case LootType.gold:     // 보상: 골드
                // 플레이어의 골드 추가
                GameManager.instance.player.gold += lootBtnList[btnOrder - 1].Item2;

                // 해당 전리품 UI Off
                UIArr[btnOrder - 1].SetActive(false);
                order -= 1;

                // 만약 전리품이 더 없을 시 자동 진행
                if (order == 0) {
                    Click_NextBtn();
                }
                break;
            case LootType.skill:     // 보상: 스킬
                // 스킬 선택 UI On
                loot_SkillUI.SetActive(true);
                loot_SkillSkipBtn.SetActive(true);

                // 추첨한 숫자 리스트
                List<int> randomNumList = new List<int>();

                int currentNum = UnityEngine.Random.Range(1, 5);    // 랜덤 색상 1개 추첨
                randomNumList.Add(currentNum);
                for (int i = 0; i < 2;)
                {
                    if (randomNumList.Contains(currentNum)) {
                        currentNum = UnityEngine.Random.Range(1, 5);    // 중복제외 랜덤 색상 2회 추가 추첨
                    }
                    else {
                        randomNumList.Add(currentNum);
                        i++;
                    }
                }

                // 해당 색상의 랜덤 스킬 전리품UI 생성
                for (int i = 0; i < 3; i++)
                {
                    loot_SkillUIArr[i].SetActive(true);
                    loot_skillList.Add(GameManager.instance.storageManager.PickRandomSkill(randomNumList[i]));      // 랜덤 스킬 추첨 후 등록
                    loot_SkillUIArr[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(loot_skillList[i].icon);   // 아이콘 변경
                }

                // 해당 전리품UI Off
                UIArr[btnOrder - 1].SetActive(false);
                order -= 1;
                break;
            /*case 3:     // 보상: 장신구
                // 장신구 추가
                GameManager.instance._ArtifactManager.AddArtifact(lootDetail[btnOrder - 1]);

                // 해당 전리품UI Off
                UIArr[btnOrder - 1].SetActive(false);
                order -= 1;

                // 만약 전리품이 더 없을 시 자동 진행
                if (order == 0) {
                    Click_NextBtn();
                }
                break;
            */
        }
    }

    // 전리품UI Off + 다음 선택지UI ON
    public void Click_NextBtn()
    {
        DeactivateAllUI();
        stageManager.SetNextStageInfo();      // 다음 스테이지 선택지 UI 설정
    }

    public void Click_SkillLootBtn(int btnOrder)
    {
        // 버튼에 따른 스킬 지급
        GameManager.instance.storageManager.ConvertSkill(loot_skillList[btnOrder - 1]);

        // 초기화
        loot_skillList = new List<Skill>();

        // UI 비활성화
        for (int i = 0; i < loot_SkillUIArr.Length; i++)
        {
            loot_SkillUIArr[i].SetActive(false);
        }
        loot_SkillUI.SetActive(false);
        loot_SkillSkipBtn.SetActive(false);

        // 만약 전리품이 더 없을 시 자동 진행
        if (order == 0)
        {
            Click_NextBtn();
        }
    }

    // 스킬 보상 스킵
    public void Click_SkipSkillLootBtn()
    {
        // 초기화
        loot_skillList = new List<Skill>();

        // UI 비활성화
        for (int i = 0; i < loot_SkillUIArr.Length; i++)
        {
            loot_SkillUIArr[i].SetActive(false);
        }
        loot_SkillUI.SetActive(false);
        loot_SkillSkipBtn.SetActive(false);

        // 만약 전리품이 더 없을 시 자동 진행
        if (order == 0)
        {
            Click_NextBtn();
        }
    }
}

