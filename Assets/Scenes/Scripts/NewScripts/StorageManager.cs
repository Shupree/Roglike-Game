using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    [Header("Reference")]
    private CsvSkillLoader skillLoader;

    [Header("UI Object")]
    private GameObject UIObject;
    public GameObject[] skillSlotArr = new GameObject[4];      // 색상별 스킬 슬롯 (빨강, 파랑, 노랑, 흰색)
    //public GameObject[] artifactSlotArr = new GameObject[5];
    //public GameObject MPSlotArr;
    //public GameObject[] themeSkillSlotArr = new GameObject[2];

    [Header("Player_SkillData")]
    private Skill[] skillArr = new Skill[4];    // 색상별 스킬 Data (빨강, 파랑, 노랑, 흰색)

    // 강제 초기화
    public void Initialize()
    {
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
        skillArr[0] = skillLoader.skillList.Find(s => s.name == "FlameShot");
        skillArr[1] = skillLoader.skillList.Find(s => s.name == "Brinicle");
        skillArr[2] = skillLoader.skillList.Find(s => s.name == "Thunder");
        skillArr[3] = skillLoader.skillList.Find(s => s.name == "AcrylShield");
        Debug.Log("모든 스킬 동기화 완료!");
    }

    // 플레이어 스킬 정보 가져오기
    public Skill GetSkillData(int colorType)    // colorType : 0 빨강, 1 파랑, 2 노랑, 3 하양
    {
        return skillArr[colorType];
    }

    public void BagBtn_clicked()
    {
        UIObject.SetActive(!UIObject.activeSelf);
    }

    public void ConvertSkillImage(SkillData.SkillType colorType, SkillData data)     // colorType : 1 빨강, 2 파랑, 3 노랑, 4 하양
    {
        // 색상확인 후 스킬 교체
        switch (colorType) {
            case SkillData.SkillType.Red:
                skillSlotArr[0].GetComponent<Image>().sprite = data.skillIcon;
                break;
            case SkillData.SkillType.Blue:
                skillSlotArr[1].GetComponent<Image>().sprite = data.skillIcon;
                break;
            case SkillData.SkillType.Yellow:
                skillSlotArr[2].GetComponent<Image>().sprite = data.skillIcon;
                break;
            case SkillData.SkillType.White:
                skillSlotArr[3].GetComponent<Image>().sprite = data.skillIcon;
                break;
        }
    }

    /*
    public void ConvertArtifactImage(int order, ArtifactData data)     // order : 슬롯 순서(1 - 5)
    {
        artifactSlotArr[order - 1].GetComponent<Image>().sprite = data.AritfactIcon;
    }
    */
}
