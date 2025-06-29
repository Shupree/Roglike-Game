using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header ("target")]
    public ITurn unit;  // HUD 대상
    
    [Header ("Reference")]
    Slider HP_Slider;

    [Header ("Status")]
    int curHealth = 100;
    int maxHealth = 100;
    int shield;

    // 초기화
    void Awake()
    {
        // 각 프로퍼티 할당
        HP_Slider = transform.GetChild(0).GetComponent<Slider>();
        //shield_UI = transform.GetChild(1).gameObject;
        //shield_Img = shield_UI.GetComponent<Image>();
        //shield_Text = shield_UI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetHUD(ITurn target)
    {
        unit = target;      // HUD 주인 설정
    }

    // HUD 데이터 갱신
    public void UpdateHUD()
    {
        curHealth = unit.GetStatus("HP");
        maxHealth = unit.GetStatus("MaxHP");
        HP_Slider.value = curHealth / (float)maxHealth;

        /*shield = unit.shield;
        if (shield == 0)
        {
            shield_UI.SetActive(false);
        }
        else
        {
            shield_UI.SetActive(true);
            shield_Text.text = shield.ToString();
        }
        */
    }
}
