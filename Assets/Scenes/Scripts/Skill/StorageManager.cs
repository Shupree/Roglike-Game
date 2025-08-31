using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    [Header("Reference")]
    public MPManager _MPManager;
    public ThemeManager themeManager;
    public CollectionManager collectionManager;

    [Header("UI Object")]
    private GameObject UIObject;
    public GameObject[] skillSlotArr = new GameObject[4];      // 색상별 스킬 슬롯 (빨강, 파랑, 노랑, 흰색)
    //public GameObject[] artifactSlotArr = new GameObject[5];
    public GameObject MPSlot;
    //public GameObject[] themeSkillSlotArr = new GameObject[2];

    [Header("skillStorage")]
    public List<PaintSkillData> skillDataList;

    [Header("Player_SkillData")]
    private PaintSkillData[] skillArr = new PaintSkillData[4];    // 색상별 스킬 Data (빨강, 파랑, 노랑, 흰색)

    // 강제 초기화
    public void Initialize()
    {
        // 하위 스크립트 초기화
        _MPManager.Initialize();
        themeManager.Initialize();
        collectionManager.Initialize();

        UIObject = transform.GetChild(1).gameObject;    // StorageUI 오브젝트 받아오기
        UIObject.SetActive(false);                      // UI 비활성화

        // 스킬 슬롯 불러오기
        for (int i = 0; i < 4; i++)
        {
            skillSlotArr[i] = UIObject.transform.GetChild(0).GetChild(i).gameObject;
        }

        /*
        for (int i = 0; i < 4; i++)
        {
            artifactSlotArr[i] = UIObject.transform.GetChild(1).GetChild(i).gameObject;
        }
        */

        // 테스트용 임시 스킬
        skillArr[0] = skillDataList.Find(s => s.skillName == "R_Bleed01");
        skillArr[1] = skillDataList.Find(s => s.skillName == "B_Bleed01");
        skillArr[2] = skillDataList.Find(s => s.skillName == "Y_Bounce");
        skillArr[3] = skillDataList.Find(s => s.skillName == "W_Heal01");

        skillSlotArr[0].GetComponent<Image>().sprite = skillArr[0].icon;
        skillSlotArr[1].GetComponent<Image>().sprite = skillArr[1].icon;
        skillSlotArr[2].GetComponent<Image>().sprite = skillArr[2].icon;
        skillSlotArr[3].GetComponent<Image>().sprite = skillArr[3].icon;

        Debug.Log("모든 스킬 동기화 완료!");
    }

    // 플레이어 스킬 정보 가져오기
    public PaintSkillData GetSkillData(PaintManager.ColorType colorType)
    {
        return skillArr[(int)colorType];
    }

    public void OnClickBagBtn()
    {
        UIObject.SetActive(!UIObject.activeSelf);
    }

    // 스킬 변경
    public void ConvertSkill(PaintSkillData skill)     // colorType : 1 빨강, 2 파랑, 3 노랑, 4 하양
    {
        // 스킬 교체
        skillArr[(int)skill.colorType - 1] = skill;

        // 색상에 따른 스킬 아이콘 교체
        skillSlotArr[(int)skill.colorType - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Skill_Sprite/" + skill.icon);
    }

    // 걸작스킬 변경
    public void ConvertMasterPiece(MasterPieceData data)
    {

    }

    public PaintSkillData PickRandomSkill(PaintManager.ColorType colorType)  // 색상받고 랜덤 스킬 뽑기 (1: 빨강, 2: 파랑, 3: 노랑, 4: 하양)
    {
        List<PaintSkillData> skillList = skillDataList
            .Where(skill =>
                skill.colorType == colorType &&      // 첫 번째 조건 : 지정한 색상과 같을 것
                !skillArr.Any(playerSkill => playerSkill.name == skill.name)            // 두 번째 조건 : 중복된 스킬은 제외할 것
            )
            .ToList();      // 중복 스킬을 제외한 리스트 제작

        if (skillList.Count > 0)
        {
            int randomNum = Random.Range(0, skillList.Count);    // 랜덤 색상 1개 추첨
            PaintSkillData randomSkill = skillList[randomNum];

            return randomSkill;
        }
        else
        {
            Debug.LogError("변경 가능한 스킬 불러오기 실패");

            return null;
        }
    }

    public MasterPieceData PickRandomMasterPiece()  // 무작위 걸작 뽑기
    {
        MasterPieceData masterPieceData = null;
        return masterPieceData;
    }
}
