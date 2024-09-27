using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    // SkillType(Color), SkillID(int)
    public SkillData noneData;

    public SkillData[] red_SkillData;       // 모든 빨강계열 스킬데이터
    public SkillData[] blue_SkillData;      // 모든 파랑계열 스킬데이터
    public SkillData[] yellow_SkillData;    // 모든 노랑계열 스킬데이터
    public SkillData[] white_SkillData;     // 모든 하양계열 스킬데이터

    private SkillData[] have_SkillData = new SkillData[4];  // 플레이어가 지니고 있는 모든 스킬 데이터
    private List<int> pickedSkillId;
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
        have_SkillData[0] = red_SkillData[1];
        use_SkillData[0] = have_SkillData[0];

        have_SkillData[1] = blue_SkillData[1];
        use_SkillData[1] = have_SkillData[1];

        have_SkillData[2] = yellow_SkillData[1];
        use_SkillData[2] = have_SkillData[2];

        have_SkillData[3] = white_SkillData[1];
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

    public SkillData PickRandomSkill(int colorNum)   // colorNum : 1.빨강, 2.파랑, 3.노랑, 4.하양
    {
        int randomNum = 0;
        SkillData[] skillData = red_SkillData;
        switch (colorNum) {
            case 1:
                skillData = red_SkillData;
                break;
            case 2:
                skillData = blue_SkillData;
                break;
            case 3:
                skillData = yellow_SkillData;
                break;
            case 4:
                skillData = white_SkillData;
                break;
        }

        for (int i = 0; i < 1; i++)
        {
            randomNum = UnityEngine.Random.Range(0, skillData.Length);
            /*for (int a = 0; a < pickedSkillId.Count; a++)
            {
                // 이미 뽑았던 스킬은 재추점 1회 방지 (새로운 스킬 추첨률 증가)
                if (randomNum == pickedSkillId[a]) {
                    i = 0;
                    pickedSkillId.Remove(randomNum);
                }
            }*/

            for (int a = 0; a < have_SkillData.Length; a++)
            {
                // 이미 해당 스킬을 지니고 있는 경우 : 재추점
                if (skillData[randomNum] == have_SkillData[a]) {
                    i = 0;
                }
            }

            // 현재는 각 슬롯마다 다른 색의 스킬이 배치되서 슬롯끼리의 중복확인을 할 필요가 없음.
        }

        return skillData[randomNum];
    }
}
