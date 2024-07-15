using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateUpdate : MonoBehaviour
{
    public enum TargetInfo { Player, Enemy }
    public TargetInfo targetType;
    public GameObject target;
    private bool IsConvert;
    public int order;   // stateUI 현재 순서
    private Image[] UIArr = new Image[6];   // 상태이상 UI 배열 (stateUI)
    private TextMeshProUGUI[] textArr = new TextMeshProUGUI[6]; // 상태이상 수치 UI 배열
    // stateUI에 따른 이펙트 타입 정보 (ImageArr의 순서와 EffectType의 순서가 일치해야 함.)

    // -01없음, 00화상, 01중독, 02감전, 03추위, 04빙결, 05기절, 06공포, 07위압, 08부식
    // 00철갑 보호막, 01집중, 02흡수, 03가시
    /*public int[] debuffArr = new int[9];
    public int[] buffArr = new int[4];*/
    public int[] effectArr = new int[13];   // player, Enemy에게서 받은 Effect 전부
    private int[] UIOrderArr = new int[13];  // UI 배치 순서 배열
    public Sprite[] spriteArr;

    void Awake()
    {
        for (int i = 0; i < UIArr.Length; i++) {
            UIArr[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
            UIArr[i].enabled = false;
        }

        for (int i = 0; i < textArr.Length; i++) {
            textArr[i] = UIArr[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            textArr[i].enabled = false;
        }

        for (int i = 0; i < UIOrderArr.Length; i++) {
            UIOrderArr[i] = -1;
            Debug.Log(UIOrderArr[i]);
        }

        order = 0;
        IsConvert = false;
    }

    // 데이터 갱신
    void LateUpdate()
    {
        switch (targetType) {
            case TargetInfo.Player:
                // 모든 버프/디버프 불러오기
                for (int i = 0; i < GameManager.instance.player.debuffArr.Length; i++) 
                {
                    effectArr[i] = GameManager.instance.player.debuffArr[i];
                }
                for (int i = 9; i < GameManager.instance.player.buffArr.Length + 9; i++) 
                {
                    effectArr[i] = GameManager.instance.player.buffArr[i - 9];
                }

                // 현재 모든 Effect값에서 존재하는 값 확인
                for (int i = 0; i < effectArr.Length; i++) 
                {
                    // 이미 생성된 UI가 있는지 확인
                    for (int a = 0; a < UIOrderArr.Length; a++)
                    {
                        if (UIOrderArr[a] == i) {
                            // 이미 생성된 UI가 있는 경우 _ UI 업데이트
                            if (effectArr[i] > 0) {
                                textArr[a].text = effectArr[i].ToString();
                                IsConvert = true;
                                break;
                            }
                            // 이미 생성된 UI가 있는 경우 _ 사라진 버프/디버프 UI 제거
                            else {
                                for (int b = 0; b <= UIOrderArr.Length - a; b++)
                                {
                                    if (!UIArr[a + b].enabled || b == UIOrderArr.Length - a) {
                                        UIOrderArr[a + b - 1] = -1;
                                        UIArr[a + b - 1].enabled = false;
                                        textArr[a + b - 1].enabled = false;
                                        break;
                                    }
                                    UIOrderArr[a + b] = UIOrderArr[a + b + 1];
                                    UIArr[a + b] = UIArr[a + b + 1];
                                    textArr[a + b] = textArr[a + b + 1];
                                }
                                order--;
                            }
                        }
                    }
                    // 생성된 UI가 없는 경우 _ UI 생성
                    if (effectArr[i] > 0) 
                    {
                        if (IsConvert == false) {
                            if (order >= 6) {
                            // UI 꽉참.
                            Debug.Log("UI가 전부 찼습니다.");
                            }
                            else {
                                UIOrderArr[order] = i;
                                Debug.Log(UIOrderArr);

                                // UI 활성화
                                UIArr[order].enabled = true;
                                textArr[order].enabled = true;

                                UIArr[order].sprite = spriteArr[i];
                                textArr[order].text = GameManager.instance.player.debuffArr[i].ToString();

                                order++;
                            }
                        }
                    }
                    IsConvert = false;
                }
                break;
            }
    }
}
