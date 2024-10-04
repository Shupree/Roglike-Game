using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    private GameObject storeUI;

    private GameObject[] skillSlotArr = new GameObject[4];
    private GameObject[] artifactSlotArr = new GameObject[3];
    private GameObject masterpieceSlot;
    public SkillManager _SkillManager;
    private ArtifactManager _ArtifactManager;
    private MPManager _MasterpieceManager;

    private int[] skillIdArr = new int[4];
    private List<int> artifactIdList;
    private int masterpieceId;

    void Awake()
    {
        // 변수 할당
        storeUI = transform.GetChild(0).gameObject;

        for (int i = 0; i < skillSlotArr.Length; i++)
        {
            skillSlotArr[i] = storeUI.transform.Find("DisplayPlace").GetChild(0).gameObject;
        }
        for (int i = 0; i < artifactSlotArr.Length; i++)
        {
            artifactSlotArr[i] = storeUI.transform.Find("DisplayPlace").GetChild(1).gameObject;
        }
        masterpieceSlot = storeUI.transform.Find("DisplayPlace").GetChild(2).gameObject;
        
        // 할당 안되는 버그 발생!!
        //_SkillManager = GameManager.instance._SkillManager;
        //_ArtifactManager = GameManager.instance._ArtifactManager;
        //_MasterpieceManager = GameManager.instance._MasterPieceManager;

        // 초기화
        storeUI.SetActive(false);
    }

    public void SetUpProduct()
    {
        storeUI.SetActive(true);

        // 색상별 스킬 추첨
        skillIdArr[0] = Random.Range(0, _SkillManager.red_SkillData.Length);
        skillIdArr[1] = Random.Range(0, _SkillManager.blue_SkillData.Length);
        skillIdArr[2] = Random.Range(0, _SkillManager.yellow_SkillData.Length);
        skillIdArr[3] = Random.Range(0, _SkillManager.white_SkillData.Length);

        // 장신구 추첨 (중복x)
        int artifactId = Random.Range(0, _ArtifactManager.all_ArtifactData.Length);
 
        for (int i = 0; i < artifactSlotArr.Length;)
        {
            if (artifactIdList.Contains(artifactId))
            {
                artifactId = Random.Range(0, _ArtifactManager.all_ArtifactData.Length);
            }
            else
            {
                artifactIdList.Add(artifactId);
                i++;
            }
        }

        // 걸작 추첨
        masterpieceId = Random.Range(0, _MasterpieceManager.all_MPData.Length);

        // 상품 이미지 변경
        skillSlotArr[0].GetComponent<Image>().sprite = _SkillManager.red_SkillData[skillIdArr[0]].skillIcon;
        skillSlotArr[1].GetComponent<Image>().sprite = _SkillManager.red_SkillData[skillIdArr[1]].skillIcon;
        skillSlotArr[2].GetComponent<Image>().sprite = _SkillManager.red_SkillData[skillIdArr[2]].skillIcon;
        skillSlotArr[3].GetComponent<Image>().sprite = _SkillManager.red_SkillData[skillIdArr[3]].skillIcon;

        for (int i = 0; i < artifactSlotArr.Length; i++)
        {
            artifactSlotArr[i].GetComponent<Image>().sprite = _ArtifactManager.all_ArtifactData[artifactIdList[i]].sprite;
        }

        masterpieceSlot.GetComponent<Image>().sprite = _MasterpieceManager.all_MPData[masterpieceId].MP_Sprite;
    }
}
