using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StoreManager : MonoBehaviour
{
    private Player _Player;
    private SkillManager _SkillManager;
    private ArtifactManager _ArtifactManager;
    private MPManager _MasterpieceManager;

    private GameObject storeUI;

    private GameObject[] skillSlotArr = new GameObject[4];  // UI Arr
    private GameObject[] artifactSlotArr = new GameObject[3];
    private GameObject masterpieceSlot;

    private int[] skillPriceArr = new int[4];       // 상품별 가격
    private int[] artifactPriceArr = new int[3];    // 상품별 가격
    private int masterpiecePrice;

    private SkillData[] skillDataArr = new SkillData[4];    // 진열 상품의 Data
    private List<ArtifactData> artifactDataList = new List<ArtifactData>();
    private MasterPieceData masterpieceData;

    public Sprite soldOutImg;   // 판매 완료 이미지

    void Awake()
    {
        // 변수 할당
        storeUI = transform.GetChild(0).gameObject;

        for (int i = 0; i < skillSlotArr.Length; i++)
        {
            skillSlotArr[i] = storeUI.transform.Find("DisplayPlace").GetChild(0).GetChild(i).gameObject;
        }
        for (int i = 0; i < artifactSlotArr.Length; i++)
        {
            artifactSlotArr[i] = storeUI.transform.Find("DisplayPlace").GetChild(1).GetChild(i).gameObject;
        }
        masterpieceSlot = storeUI.transform.Find("DisplayPlace").GetChild(2).GetChild(0).gameObject;
        
        // 참조
        _Player = GameManager.instance._player;
        _SkillManager = GameManager.instance._SkillManager;
        _ArtifactManager = GameManager.instance._ArtifactManager;
        _MasterpieceManager = GameManager.instance._MasterPieceManager;

        // 기본 가격 설정
        for (int i = 0; i < skillPriceArr.Length; i++)
        {
            skillPriceArr[i] = 60;
        }
        for (int i = 0; i < artifactPriceArr.Length; i++)
        {
            artifactPriceArr[i] = 80;      // Common급 장신구 가격
        }
        masterpiecePrice = 150;


        // 초기화
        storeUI.SetActive(false);
    }

    public void CloseStore()
    {
        storeUI.SetActive(false);

        GameManager.instance.SetNextStageUI();  // 다음 스테이지로
    }

    public void SetUpProduct()
    {
        storeUI.SetActive(true);

        // 색상별 스킬 추첨
        for (int i = 0; i < skillSlotArr.Length; i++)
        {
            skillDataArr[i] = _SkillManager.PickRandomSkill(i + 1);
        }

        // 장신구 추첨
        artifactDataList = _ArtifactManager.PickRandomArtifact(3);

        // 걸작 추첨
        masterpieceData = _MasterpieceManager.PickRandomMasterPiece();

        // 상품 이미지 변경 & 가격 설정_스킬
        for (int i = 0; i < skillSlotArr.Length; i++)
        {
            skillSlotArr[i].GetComponent<Image>().sprite = skillDataArr[i].skillIcon;
            skillSlotArr[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = skillPriceArr[i].ToString();
        }

        // 상품 이미지 변경 & 가격 설정_장신구
        for (int i = 0; i < artifactSlotArr.Length; i++)
        {
            artifactSlotArr[i].GetComponent<Image>().sprite = artifactDataList[i].sprite;
            switch (artifactDataList[i].artifactRate) {
                case ArtifactData.ArtifactRate.Common:
                    break;
                case ArtifactData.ArtifactRate.Rare:
                    artifactPriceArr[i] = 100;
                    break;
                case ArtifactData.ArtifactRate.Unique:
                    artifactPriceArr[i] = 130;
                    break;
                case ArtifactData.ArtifactRate.Cursed:
                    artifactPriceArr[i] = 110;
                    break;
            }
            artifactSlotArr[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = artifactPriceArr[i].ToString();
        }

        // 상품 이미지 변경_걸작
        masterpieceSlot.GetComponent<Image>().sprite = masterpieceData.MP_Sprite;
        masterpieceSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = masterpiecePrice.ToString();
    }

    // 상품 버튼 클릭 시 : 구매
    public void BuyProduct(int productType, int btnOrder)   // productType : 1스킬, 2장신구, 3걸작
    {
        // 버튼에 따른 스킬 지급
        switch (productType) {
            case 1:     // 스킬 구매
                if (_Player.gold >= skillPriceArr[btnOrder - 1]) {      // 구매 가능 여부 확인
                    _Player.gold -= skillPriceArr[btnOrder - 1];
                    _SkillManager.use_SkillData[btnOrder - 1] = skillDataArr[btnOrder - 1];     // 스킬 교체

                    skillSlotArr[btnOrder - 1].GetComponent<Image>().sprite = soldOutImg;       // 판매 완료 UI 출력
                    skillSlotArr[btnOrder - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Thanks!";
                }
                else {
                    // 구매 실패
                }
                break;
            case 2:
                if (_Player.gold >= artifactPriceArr[btnOrder - 1]) {      // 구매 가능 여부 확인
                    _Player.gold -= artifactPriceArr[btnOrder - 1];
                    _ArtifactManager.AddArtifact(artifactDataList[btnOrder - 1].ArtifactId);

                    artifactSlotArr[btnOrder - 1].GetComponent<Image>().sprite = soldOutImg;    // 판매 완료 UI 출력
                    artifactSlotArr[btnOrder - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Thanks!";
                }
                else {
                    // 구매 실패
                }
                break;
            case 3:
                if (_Player.gold >= masterpiecePrice) {      // 구매 가능 여부 확인
                    _Player.gold -= masterpiecePrice;
                    _MasterpieceManager.ChangeMasterPiece(masterpieceData.MP_Id);   // MP 교체 함수 만들 걳

                    masterpieceSlot.GetComponent<Image>().sprite = soldOutImg;      // 판매 완료 UI 출력
                    masterpieceSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Thanks!";
                }
                break;
        }
    }
}