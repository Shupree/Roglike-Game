using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // SkillType(Color), SkillID(int)
    public SkillData noneData;
    public SkillData[] all_SkillData;   // 모든 스킬데이터
    private SkillData[] have_SkillData = new SkillData[4];  // 플레이어가 지니고 있는 모든 스킬 데이터
    public SkillData[] use_SkillData = new SkillData[4];    // 사용할 스킬 데이터 (red, yellow, blue, white 순)

    private GameObject UIObject;
    private GameObject[] redSlot = new GameObject[3];       // Red 스킬 대기 슬롯
    private GameObject[] yellowSlot = new GameObject[3];    // Yellow 스킬 대기 슬롯
    private GameObject[] blueSlot = new GameObject[3];      // Blue 스킬 대기 슬롯
    private GameObject[] whiteSlot = new GameObject[3];     // White 스킬 대기 슬롯

    private bool isOpend;

    void Start()
    {
        UIObject = transform.GetChild(0).gameObject;
        UIObject.SetActive(false);
        for (int i = 0; i < 3; i++) 
        {
            redSlot[i] = UIObject.transform.GetChild(0).GetChild(i).gameObject;
            yellowSlot[i] = UIObject.transform.GetChild(1).GetChild(i).gameObject;
            blueSlot[i] = UIObject.transform.GetChild(2).GetChild(i).gameObject;
            whiteSlot[i] = UIObject.transform.GetChild(3).GetChild(i).gameObject;
        }

        isOpend = false;

        // 테스트용 스킬 데이터 획득
        have_SkillData[0] = all_SkillData[0];
        use_SkillData[0] = have_SkillData[0];

        have_SkillData[1] = all_SkillData[1];
        use_SkillData[1] = have_SkillData[1];

        have_SkillData[2] = all_SkillData[2];
        use_SkillData[2] = have_SkillData[2];

        have_SkillData[3] = all_SkillData[3];
        use_SkillData[3] = have_SkillData[3];
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

    public void ConvertImage()
    {

    }
}
