using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하위의 HUD 오브젝트를 관리하는 HUDManager
// (Enemy의 HUD를 관리하기 위함.)

public class HUDManager : MonoBehaviour
{
    private GameObject[] ObjArr = new GameObject[4];

    void Awake()
    {
        // 오브젝트 할당
        for (int i = 0; i < ObjArr.Length; i++) {
            ObjArr[i] = transform.GetChild(i).gameObject;
            ObjArr[i].SetActive(false);
        }
    }

    // 적 HUD 활성화
    public void ActivateHUD (int order, GameObject obj)  // 활성화할 HUD 넘버(order), 해당 Enemy 오브젝트 받기
    {
        Enemy enemyScript = obj.GetComponent<Enemy>();
        ObjArr[order].GetComponent<HUD>().enemyScript = enemyScript;     // HUD의 대상 설정
        ObjArr[order].transform.GetChild(2).GetComponent<StateUpdate>().enemyScript = enemyScript;   // HUD 하위의 StateUpdate 대상 설정
        ObjArr[order].GetComponent<RectTransform>().position = obj.transform.position;      // HUD 위치 조정 (적 이미지 아래)
        ObjArr[order].SetActive(true);
    }

    // HUD 비활성화
    public void DeActivateHUD (int order)  // 비활성화할 HUD 넘버(order) 받기
    {
        // 이 과정에서 HUD가 작동 시 할당된 데이터 상실로 오류가 생기는지 확인할 것
        ObjArr[order].SetActive(false);
        ObjArr[order].GetComponent<HUD>().enemyScript = null;
        ObjArr[order].transform.GetChild(2).GetComponent<StateUpdate>().enemyScript = null;
    }
}
