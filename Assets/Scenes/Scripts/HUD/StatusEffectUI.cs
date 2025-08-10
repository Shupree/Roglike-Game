using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [Header("target")]
    public ITurn unit;  // HUD 대상
    
    [Header ("UI Scripts")]
    private Image[] UIArr = new Image[12];   // 상태이상 UI 배열 (stateUI)
    private TextMeshProUGUI[] textArr = new TextMeshProUGUI[12]; // 상태이상 수치 UI 배열

    // [Header("StatusEffect")]
    // private List<StatusEffect> statusEffects = new List<StatusEffect>();   // 상태이상 List

    void Awake()
    {
        // 각 UI 오브젝트 할당
        for (int i = 0; i < UIArr.Length; i++) {
            UIArr[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
            UIArr[i].enabled = false;
        }

        for (int i = 0; i < textArr.Length; i++) {
            textArr[i] = UIArr[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            textArr[i].enabled = false;
        }
    }

    // 데이터 갱신 (현재 상태이상 최대 개수는 12개.)
    public void UpdateStatusEffect(List<StatusEffect> curStatusEffects)
    {
        for (int i = 0; i < curStatusEffects.Count; i++)
        {
            UIArr[i].enabled = true;
            textArr[i].enabled = true;
            UIArr[i].sprite = curStatusEffects[i].data.icon;     // 상태이상 이미지 갱신
            textArr[i].text = curStatusEffects[i].stackCount.ToString();            // 상태이상 수치 갱신
        }

        for (int i = curStatusEffects.Count; i < UIArr.Length; i++)
        {
            UIArr[i].enabled = false;
            textArr[i].enabled = false;
        }
    }
}
