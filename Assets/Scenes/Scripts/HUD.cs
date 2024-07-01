using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Health, Shield, Enemy_Health, Enemy_Shield }
    public InfoType type;
    public GameObject target;
    
    Text myText;
    Slider mySlider;

    int curHealth;
    int maxHealth;

    // 초기화
    void Awake()
    {
        //myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    // 데이터 갱신
    void LateUpdate() {
        switch (type) {
            case InfoType.Health:
                curHealth = GameManager.instance.player.health;
                maxHealth = GameManager.instance.player.maxHealth;
                mySlider.value = curHealth / (float)maxHealth;
                break;
            case InfoType.Shield:

                break;
            case InfoType.Enemy_Health:
                curHealth = target.GetComponent<Enemy>().health;
                maxHealth = target.GetComponent<Enemy>().maxHealth;
                mySlider.value = curHealth / (float)maxHealth;
                break;
            case InfoType.Enemy_Shield:

                break;
        }
    }
}
