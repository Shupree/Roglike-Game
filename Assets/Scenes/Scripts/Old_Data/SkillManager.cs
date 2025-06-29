using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    /*

    // SkillType(Color), SkillID(int)
    [Header ("Skill Data")]
    public SkillData noneData;

    [Space (10f)]
    public SkillData[] red_SkillData;       // 모든 빨강계열 스킬데이터
    public SkillData[] blue_SkillData;      // 모든 파랑계열 스킬데이터
    public SkillData[] yellow_SkillData;    // 모든 노랑계열 스킬데이터
    public SkillData[] white_SkillData;     // 모든 하양계열 스킬데이터

    //private SkillData[] have_SkillData = new SkillData[4];  // 플레이어가 지니고 있는 모든 스킬 데이터
    
    [Space (10f)]
    public SkillData[] use_SkillData = new SkillData[4];    // 사용할 스킬 데이터 (red, blue, yellow, white 순)

    //private GameObject UIObject;
    //private GameObject[] skillSlots = new GameObject[4];       // 색상별 스킬 슬롯 (빨강, 파랑, 노랑, 흰색)

    //private bool isOpend;

    void Awake()
    {
        
        //UIObject = transform.GetChild(0).gameObject;
        //UIObject.SetActive(false);
        //for (int i = 0; i < 4; i++) 
        //{
        //    skillSlots[i] = UIObject.transform.GetChild(i).gameObject;
        //}
        

        // isOpend = false;

        // 테스트용 스킬 데이터 획득
        use_SkillData[0] = red_SkillData[0];

        use_SkillData[1] = blue_SkillData[0];

        use_SkillData[2] = yellow_SkillData[0];

        use_SkillData[3] = white_SkillData[0];

        GameManager.instance._StorageManager.ConvertSkillImage(SkillData.SkillType.Red, use_SkillData[0]);
        GameManager.instance._StorageManager.ConvertSkillImage(SkillData.SkillType.Blue, use_SkillData[1]);
        GameManager.instance._StorageManager.ConvertSkillImage(SkillData.SkillType.Yellow, use_SkillData[2]);
        GameManager.instance._StorageManager.ConvertSkillImage(SkillData.SkillType.White, use_SkillData[3]);
    }

    /*
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
    */

    /*
    public void ConvertImage(SkillData.SkillType colorType, SkillData data)     // colorType : 1 빨강, 2 파랑, 3 노랑, 4 하양
    {
        // 색상확인 후 스킬 교체
        switch (colorType) {
            case SkillData.SkillType.Red:
                skillSlots[0].GetComponent<Image>().sprite = data.skillIcon;
                break;
            case SkillData.SkillType.Blue:
                skillSlots[1].GetComponent<Image>().sprite = data.skillIcon;
                break;
            case SkillData.SkillType.Yellow:
                skillSlots[2].GetComponent<Image>().sprite = data.skillIcon;
                break;
            case SkillData.SkillType.White:
                skillSlots[3].GetComponent<Image>().sprite = data.skillIcon;
                break;
        }
    }
    */
    
    /*
    public void ConvertSkill(SkillData skillData)
    {
        // 색상확인 후 스킬 교체
        switch (skillData.skillType) {
            case SkillData.SkillType.Red:
                use_SkillData[0] = skillData;
                break;
            case SkillData.SkillType.Blue:
                use_SkillData[1] = skillData;
                break;
            case SkillData.SkillType.Yellow:
                use_SkillData[2] = skillData;
                break;
            case SkillData.SkillType.White:
                use_SkillData[3] = skillData;
                break;
        }

        // 가방 내 스킬 아이콘 교체
        GameManager.instance._StorageManager.ConvertSkillImage(skillData.skillType, skillData);
    }

    public SkillData PickRandomSkill(int colorNum)   // colorNum : 1.빨강, 2.파랑, 3.노랑, 4.하양
    {
        int randomNum = 0;
        SkillData[] skillData = red_SkillData;

        // int errorNum = 0;

        // 스킬 색상 확인
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

        // 무작위 스킬 추첨
        for (int i = 0; i < 1;)
        {
            randomNum = UnityEngine.Random.Range(0, skillData.Length);
            //for (int a = 0; a < pickedSkillId.Count; a++)
            //{
                // 이미 뽑았던 스킬은 재추점 1회 방지 (새로운 스킬 추첨률 증가)
            //    if (randomNum == pickedSkillId[a]) {
            //        i = 0;
            //        pickedSkillId.Remove(randomNum);
            //    }
            //}

            // 이미 해당 스킬을 지니고 있는 경우 : 재추점   (무한 for문 방지용 : 50회 시도 시 오류로 판정)
            if (skillData[randomNum] == use_SkillData[colorNum - 1]) {
                //if (errorNum >= 50) {
                //    Debug.Log("오류 발생!!");
                //    break;
                //}
                //errorNum++;
                continue;
            }
            else {
                i++;
            }

            // 현재는 각 슬롯마다 다른 색의 스킬이 배치되서 슬롯끼리의 중복확인을 할 필요가 없음.
        }

        return skillData[randomNum];
    }
    */
}
