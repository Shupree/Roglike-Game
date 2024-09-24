using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 전투 승리 보상 버튼UI
public class LootUI : MonoBehaviour
{
    private GameObject[] UIArr = new GameObject[3];
    public Sprite[] SpriteArr;
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
                UIArr[order].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = SpriteArr[0];
                UIArr[order].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = detail.ToString() + " Golds";
                break;
            case 2:     // 일반스킬 보상
                lootType[order] = 2;
                lootDetail[order] = detail;
                UIArr[order].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = SpriteArr[1];
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
                UIArr[btnOrder - 1].SetActive(false);
                order -= 1;
                break;
            case 3:     // 보상: 장신구
                break;
        }

        if (order <= 0) {   // 모든 보상 획득 시
            DeactivateAllUI();      // 초기화
        }
    }
}
