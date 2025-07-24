using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    [Header("Reference")]
    private CsvSkillLoader skillLoader;
    public MPManager _MPManager;
    public ThemeManager themeManager;

    [Header("UI Object")]
    private GameObject UIObject;
    public GameObject[] skillSlotArr = new GameObject[4];      // 색상별 스킬 슬롯 (빨강, 파랑, 노랑, 흰색)
    //public GameObject[] artifactSlotArr = new GameObject[5];
    public GameObject MPSlot;
    //public GameObject[] themeSkillSlotArr = new GameObject[2];

    [Header("Player_SkillData")]
    private Skill[] skillArr = new Skill[4];    // 색상별 스킬 Data (빨강, 파랑, 노랑, 흰색)

    // 강제 초기화
    public void Initialize()
    {
        // 하위 스크립트 초기화
        _MPManager.Initialize();
        themeManager.Initialize();

        skillLoader = GameManager.instance.skillLoader;     // 스크립트 불러오기

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
        MPSlotArr = UIObject.transform.GetChild(2).GetChild(0).gameObject;
        for (int i = 0; i < 2; i++) 
        {
            themeSkillSlotArr[i] = UIObject.transform.GetChild(2).GetChild(1).GetChild(i).gameObject;
        }
        */

        // 테스트용 임시 스킬
        skillArr[0] = skillLoader.skillList.Find(s => s.nameEn == "FlameShot");
        skillArr[1] = skillLoader.skillList.Find(s => s.nameEn == "Brinicle");
        skillArr[2] = skillLoader.skillList.Find(s => s.nameEn == "Thunder");
        skillArr[3] = skillLoader.skillList.Find(s => s.nameEn == "AcrylShield");

        skillSlotArr[0].GetComponent<Image>().sprite = Resources.Load<Sprite>(skillArr[0].icon);
        skillSlotArr[1].GetComponent<Image>().sprite = Resources.Load<Sprite>(skillArr[1].icon);
        skillSlotArr[2].GetComponent<Image>().sprite = Resources.Load<Sprite>(skillArr[2].icon);
        skillSlotArr[3].GetComponent<Image>().sprite = Resources.Load<Sprite>(skillArr[3].icon);
        
        Debug.Log("모든 스킬 동기화 완료!");
    }

    // 플레이어 스킬 정보 가져오기
    public Skill GetSkillData(PaintManager.ColorType colorType)
    {
        return skillArr[(int)colorType];
    }

    public void BagBtn_clicked()
    {
        UIObject.SetActive(!UIObject.activeSelf);
    }

    // 스킬 변경
    public void ConvertSkill(Skill skill)     // colorType : 1 빨강, 2 파랑, 3 노랑, 4 하양
    {
        // 스킬 교체
        skillArr[skill.colorType - 1] = skill;

        // 색상에 따른 스킬 아이콘 교체
        skillSlotArr[skill.colorType - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Skill_Sprite/"+skill.icon);
    }

    /*
    public void ConvertArtifactImage(int order, ArtifactData data)     // order : 슬롯 순서(1 - 5)
    {
        artifactSlotArr[order - 1].GetComponent<Image>().sprite = data.AritfactIcon;
    }
    */
    
    public Skill PickRandomSkill(int colorType)  // 색상받고 랜덤 스킬 뽑기 (1: 빨강, 2: 파랑, 3: 노랑, 4: 하양)
    {
        List <Skill> skillList = skillLoader.skillList
            .Where( skill =>
                skill.colorType == colorType &&      // 첫 번째 조건 : 지정한 색상과 같을 것
                !skillArr.Any(playerSkill => playerSkill.name == skill.name)            // 두 번째 조건 : 중복된 스킬은 제외할 것
            )
            .ToList();      // 중복 스킬을 제외한 리스트 제작

        if (skillList.Count > 0)
        {
            int randomNum = Random.Range(0, skillList.Count);    // 랜덤 색상 1개 추첨
            Skill randomSkill = skillList[randomNum];

            return randomSkill;
        }
        else
        {
            Debug.LogError("변경 가능한 스킬 불러오기 실패");

            return null;
        }
    }
}
