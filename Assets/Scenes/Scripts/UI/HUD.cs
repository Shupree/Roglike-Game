using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Player, Enemy }

    [Header ("Object Type")]
    public InfoType type;

    [Header ("target")]
    public GameObject target;
    private Enemy enemyScript;
    
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
        HP_Slider = transform.GetChild(0).GetComponent<Slider>();
        shield_UI = transform.GetChild(1).gameObject;
        //shield_Img = shield_UI.GetComponent<Image>();
        shield_Text = shield_UI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (target != null) {
            enemyScript = target.GetComponent<Enemy>();
        }
    }

    // 데이터 갱신
    void LateUpdate() {
        switch (type) {
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

            case InfoType.Enemy:
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
