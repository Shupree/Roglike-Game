using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 전투 승리 보상 버튼UI
public class LootUI : MonoBehaviour
{
    private GameObject[] UIArr = new GameObject[3];     // 전리품 UI 버튼
    private GameObject loot_SkillUI;         // 전리품_스킬 선택 시 열리는 선택지 UI
    private GameObject[] loot_SkillUIArr = new GameObject[3];       // 전리품_스킬 선택지 UI 버튼
    public Sprite[] spriteArr;
    private int[] lootType = new int[3];    // 버튼별 보상 종류 (0:Null, 1:골드, 2:스킬, 3:장신구, 4:걸작)
    private int[] lootDetail = new int[3];  // 버튼별 보상 상세정보
    private int order;      // 보상 개수

    void Awake()
    {
        // UI 불러오기
        for (int i = 0; i < UIArr.Length; i++) 
        {
            UIArr[i] = transform.GetChild(1).GetChild(i).gameObject;
            UIArr[i].SetActive(false);
        }
        loot_SkillUI = transform.GetChild(3).gameObject;
        loot_SkillUI.SetActive(false);
        for (int i = 0; i < loot_SkillUIArr.Length; i++)
        {
            loot_SkillUIArr[i] = loot_SkillUI.transform.GetChild(0).GetChild(i).gameObject;
        }

        // 초기화
        for (int i = 0; i < lootType.Length; i++)
        {
            lootType[i] = 0;
            lootDetail[i] = 0;
        }
    }

    // UI 기능 정지 (이미지, 기능 전부)
    public void DeactivateAllUI()
    {
        gameObject.SetActive(false);
        order = 0;      // 전리품 초기화
    }

    // 보상UI 활성화 : int 보상 종류, int 보상 정보 (골드 수, 스킬 선택지 수, 장신구ID)
    // typeNum : 01.골드, 02.스킬, 03.장신구
    public void SetLootUI(int typeNum, int detail)
    {
        UIArr[order].SetActive(true);
        // 해당 UI 이미지 변경
        switch (typeNum) {
            case 1:     // 골드 보상
                lootType[order] = 1;
                lootDetail[order] = detail;
                UIArr[order].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = spriteArr[0];
                UIArr[order].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = detail.ToString() + " Golds";
                break;
            case 2:     // 일반스킬 보상
                lootType[order] = 2;
                lootDetail[order] = detail;
                UIArr[order].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = spriteArr[1];
                UIArr[order].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Skills";
                break;
            case 3:     // 장신구 보상
                lootType[order] = 3;
                lootDetail[order] = detail;
                UIArr[order].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite
                    = GameManager.instance._ArtifactManager.all_ArtifactData[detail].sprite;
                UIArr[order].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
                    = GameManager.instance._ArtifactManager.all_ArtifactData[detail].name;
                break;
            //case 4:     // 걸작 보상
            //    break;
        }
        order += 1;
    }

    // 보상 버튼 클릭 시
    public void ClickLootBtn(int btnOrder)
    {
        switch (lootType[btnOrder - 1]) {
            case 1:     // 보상: 골드
                GameManager.instance._player.gold += lootDetail[btnOrder - 1];
                UIArr[btnOrder - 1].SetActive(false);
                order -= 1;
                break;
            case 2:     // 보상: 스킬
                // 골드 보상을 먼저 받는 경우 출력 안되는 버그 발생!
                loot_SkillUI.SetActive(true);

                List<int> randomNumList = new List<int>();

                int currentNum = UnityEngine.Random.Range(1, 5);    // 랜덤 색상 1개 추첨
                randomNumList.Add(currentNum);
                for (int i = 0; i < 2; i++)
                {
                    if (randomNumList.Contains(currentNum)) {
                        currentNum = UnityEngine.Random.Range(1, 5);    // 중독제외 랜덤 색상 2회 추가 추첨
                    }
                    else {
                        randomNumList.Add(currentNum);
                        i++;
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    //GameManager.instance._SkillManager.PickRandomSkill(randomNumList[i]);
                    // 여기에 PickRandomSkill() return받아서 UI업데이트
                }
                UIArr[btnOrder - 1].SetActive(false);
                order -= 1;
                break;
            case 3:     // 보상: 장신구
                GameManager.instance._ArtifactManager.AddArtifact(lootDetail[btnOrder - 1]);
                UIArr[btnOrder - 1].SetActive(false);
                order -= 1;
                break;
        }

        if (order <= 0) {   // 모든 보상 획득 시
            DeactivateAllUI();      // 초기화
        }
    }

    void ClickLoot_SkillBtn()
    {

    }
}
