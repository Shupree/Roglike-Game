using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUpdate : MonoBehaviour
{
    public enum InfoTarget { Player, Enemy }
    public InfoTarget targetType;
    public GameObject target;
    private bool IsConvert;
    public int order;   // stateUI 현재 순서
    // stateUI 배열
    public Image[] imageArr = new Image[6];
    // stateUI에 따른 이펙트 타입 정보 (ImageArr의 순서와 EffectType의 순서가 일치해야 함.)
    // 1화상, 2중독, 3감전, 4추위, 5빙결, 6집중
    public int[] effectType = new int[6];
    public Sprite[] spriteArr;
    public Text[] textArr = new Text[6];

    void Awake()
    {
        order = 0;
        IsConvert = false;
    }

    // 데이터 갱신
    void LateUpdate()
    {
        switch (targetType) {
            case InfoTarget.Player:
                for (int i = 0; i < GameManager.instance.player.effectArr.Length; i++) 
                {
                    // 1화상, 2중독, 3감전, 4추위, 5빙결, 6집중
                    if (GameManager.instance.player.effectArr[i] > 0) {
                        for (int a = 0; a < effectType.Length; a++)
                        {
                            if (effectType[a] == i) {
                                textArr[a].text = GameManager.instance.player.effectArr[i].ToString();
                                IsConvert = true;
                                break;
                            }
                        }
                        if (IsConvert == false) {
                            effectType[order] = i;
                            imageArr[order].sprite = spriteArr[i];
                            textArr[order].text = GameManager.instance.player.effectArr[i].ToString();
                        }
                        IsConvert = false;
                    }
                    else {
                        for (int a = 0; a < effectType.Length; a++)
                        {
                            if (effectType[a] == GameManager.instance.player.effectArr[i]) {

                            }
                        }
                    }
                }
                break;
        }
    }
}
