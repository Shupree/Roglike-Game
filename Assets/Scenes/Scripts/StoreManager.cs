using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{
    private GameObject storeUI;

    private GameObject[] skillSlotArr = new GameObject[4];
    private GameObject[] artifactSlotArr = new GameObject[3];
    private GameObject masterpieceSlot;

    private SkillManager _SkillManager;
    private ArtifactManager _ArtifactManager;
    private MPManager _MasterpieceManager;

    private int skillPrice;
    private int[] artifactPrice = new int[4];    // 등급별 가격 (Common, Rare, Unique, Cursed)
    private int masterpiecePrice;

    private SkillData[] skillDataArr = new SkillData[4];
    private List<ArtifactData> artifactDataList = new List<ArtifactData>();
    private int masterpieceId;

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
        _SkillManager = GameManager.instance._SkillManager;
        _ArtifactManager = GameManager.instance._ArtifactManager;
        _MasterpieceManager = GameManager.instance._MasterPieceManager;

        // 기본 가격 설정
        skillPrice = 60;
        artifactPrice[0] = 80;      // Common급 장신구 가격
        artifactPrice[1] = 100;     // Rare급 장신구 가격
        artifactPrice[2] = 130;     // Unique급 장신구 가격
        artifactPrice[3] = 77;      // Cursed급 장신구 가격
        masterpiecePrice = 150;

        // 초기화
        storeUI.SetActive(false);
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

        // 걸작 추첨    (_MasterpieceManager 내부에서 랜덤으로 걸작 뽑기 함수를 만들 것!)
        masterpieceId = Random.Range(0, _MasterpieceManager.all_MPData.Length);

        // 상품 이미지 변경 & 가격 설정_스킬
        for (int i = 0; i < skillSlotArr.Length; i++)
        {
            skillSlotArr[i].GetComponent<Image>().sprite = skillDataArr[i].skillIcon;
            skillSlotArr[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = skillPrice.ToString();
        }

        // 상품 이미지 변경 & 가격 설정_장신구
        for (int i = 0; i < artifactSlotArr.Length; i++)
        {
            artifactSlotArr[i].GetComponent<Image>().sprite = artifactDataList[i].sprite;
            switch (artifactDataList[i].artifactRate) {
                case ArtifactData.ArtifactRate.Common:
                    artifactSlotArr[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = artifactPrice[0].ToString();
                    break;
                case ArtifactData.ArtifactRate.Rare:
                    artifactSlotArr[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = artifactPrice[1].ToString();
                    break;
                case ArtifactData.ArtifactRate.Unique:
                    artifactSlotArr[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = artifactPrice[2].ToString();
                    break;
                case ArtifactData.ArtifactRate.Cursed:
                    artifactSlotArr[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = artifactPrice[3].ToString();
                    break;
            }
        }

        // 상품 이미지 변경_걸작
        masterpieceSlot.GetComponent<Image>().sprite = _MasterpieceManager.all_MPData[masterpieceId].MP_Sprite;
        masterpieceSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = masterpiecePrice.ToString();
    }
}
