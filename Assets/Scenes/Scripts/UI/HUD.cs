using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
public class HUD : MonoBehaviour
{
    public enum InfoType { Player, Enemy }

    [Header ("Object Type")]
    public InfoType type;

    [Header ("target")]
    public Enemy enemyScript;
    
    [Header ("Reference")]
    Slider HP_Slider;
    GameObject shield_UI;
    //Image shield_Img;
    TextMeshProUGUI shield_Text;

    [Header ("Status")]
    int curHealth;
    int maxHealth;
    int shield;

    // 초기화
    void Awake()
    {
        // 각 프로퍼티 할당
        HP_Slider = transform.GetChild(0).GetComponent<Slider>();
        shield_UI = transform.GetChild(1).gameObject;
        //shield_Img = shield_UI.GetComponent<Image>();
        shield_Text = shield_UI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // 데이터 갱신
    void LateUpdate() {
        switch (type) {
            // 플레이어 HUD 정보 갱신
            case InfoType.Player:
                curHealth = GameManager.instance._player.health;
                maxHealth = GameManager.instance._player.maxHealth;
                HP_Slider.value = curHealth / (float)maxHealth;

                shield = GameManager.instance._player.shield;
                if (shield == 0) {
                    shield_UI.SetActive(false);
                }
                else {
                    shield_UI.SetActive(true);
                    shield_Text.text = shield.ToString();
                }
                break;

            // 적의 HUD 정보 갱신
            case InfoType.Enemy:
                if (enemyScript == null) {
                    return;     // 오류 방지
                }

                // 적 정보 수집 후 적용
                curHealth = enemyScript.health;
                maxHealth = enemyScript.maxHealth;
                HP_Slider.value = curHealth / (float)maxHealth;

                shield = enemyScript.shield;
                if (shield == 0) {
                    shield_UI.SetActive(false);
                }
                else {
                    shield_UI.SetActive(true);
                    shield_Text.text = shield.ToString();
                }
                break;
        }
    }
}
*/
