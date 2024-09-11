using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    // SkillType(Color), SkillID(int)
    public SkillData noneData;
    public SkillData[] all_SkillData;   // 모든 스킬데이터
    private SkillData[] have_SkillData = new SkillData[4];  // 플레이어가 지니고 있는 모든 스킬 데이터
    public SkillData[] use_SkillData = new SkillData[4];    // 사용할 스킬 데이터 (red, blue, yellow, white 순)

    private GameObject UIObject;
    private GameObject[] skillSlots = new GameObject[4];       // 색상별 스킬 슬롯 (빨강, 파랑, 노랑, 흰색)

    private bool isOpend;

    void Awake()
    {
        UIObject = transform.GetChild(0).gameObject;
        UIObject.SetActive(false);
        for (int i = 0; i < 4; i++) 
        {
            skillSlots[i] = UIObject.transform.GetChild(i).gameObject;
            Debug.Log(skillSlots[i]);
        }

        isOpend = false;

        // 테스트용 스킬 데이터 획득
        have_SkillData[0] = all_SkillData[1];
        use_SkillData[0] = have_SkillData[0];

        have_SkillData[1] = all_SkillData[2];
        use_SkillData[1] = have_SkillData[1];

        have_SkillData[2] = all_SkillData[3];
        use_SkillData[2] = have_SkillData[2];

        have_SkillData[3] = all_SkillData[4];
        use_SkillData[3] = have_SkillData[3];

        ConvertImage(1, use_SkillData[0]);
        ConvertImage(2, use_SkillData[1]);
        ConvertImage(3, use_SkillData[2]);
        ConvertImage(4, use_SkillData[3]);
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

    public void ConvertImage(int colorType, SkillData data)     // colorType : 1 빨강, 2 파랑, 3 노랑, 4 하양
    {
        skillSlots[colorType - 1].GetComponent<Image>().sprite = data.skillIcon;
    }
}
