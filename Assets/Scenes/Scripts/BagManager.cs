using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    // SkillType(Color), SkillID(int)
    public SkillData noneData;
    public SkillData[] all_SkillData;
    private SkillData[] have_SkillData = new SkillData[4];
    public SkillData[] use_SkillData = new SkillData[4];    // red, yellow, blue, white 순

    private GameObject UIObject;
    private GameObject[] redSlot = new GameObject[3];
    private GameObject[] yellowSlot = new GameObject[3];
    private GameObject[] blueSlot = new GameObject[3];
    private GameObject[] whiteSlot = new GameObject[3];

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

        have_SkillData[0] = all_SkillData[0];
        use_SkillData[0] = have_SkillData[0];

        have_SkillData[1] = all_SkillData[1];
        use_SkillData[1] = have_SkillData[1];

        have_SkillData[2] = all_SkillData[2];
        use_SkillData[2] = have_SkillData[2];

        have_SkillData[3] = all_SkillData[3];
        use_SkillData[3] = have_SkillData[3];
    }

    private void BagBtn()
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
