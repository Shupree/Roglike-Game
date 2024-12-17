using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    private GameObject UIObject;
    public GameObject[] skillSlotArr = new GameObject[4];      // 색상별 스킬 슬롯 (빨강, 파랑, 노랑, 흰색)
    public GameObject[] artifactSlotArr = new GameObject[5];
    public GameObject MPSlotArr;
    public GameObject[] themeSkillSlotArr = new GameObject[2];

    private bool isOpend = false;

    void Awake()
    {
        UIObject = transform.GetChild(1).gameObject;    // StorageUI 오브젝트 받아오기
        UIObject.SetActive(false);                      // UI 비활성화
        // 스킬 슬롯 불러오기
        /*
        for (int i = 0; i < 4; i++) 
        {
            skillSlotArr[i] = UIObject.transform.GetChild(0).GetChild(i).gameObject;
        }
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

        isOpend = false;
    }

    public void BagBtn()
    {
        if (isOpend == false) {
            UIObject.SetActive(true);
            isOpend = true;
        }
        else {
            UIObject.SetActive(false);
            isOpend = false;
        }
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

    public void ConvertArtifactImage(int order, ArtifactData data)     // order : 슬롯 순서(1 - 5)
    {
        artifactSlotArr[order - 1].GetComponent<Image>().sprite = data.AritfactIcon;
    }
}
