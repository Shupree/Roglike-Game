using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
public class StateUpdate : MonoBehaviour
{
    public enum TargetInfo { Player, Enemy }

    [Header ("Target Info")]
    public TargetInfo targetType;   //
    public Enemy enemyScript;  // 해당 오브젝트의 Enemy 스크립트

    [Header ("UI Scripts")]
    private Image[] UIArr = new Image[6];   // 상태이상 UI 배열 (stateUI)
    private TextMeshProUGUI[] textArr = new TextMeshProUGUI[6]; // 상태이상 수치 UI 배열
    // stateUI에 따른 이펙트 타입 정보 (ImageArr의 순서와 EffectType의 순서가 일치해야 함.)

    [Header ("Buff / Debuff")]
    // -01없음, 00화상, 01중독, 02감전, 03추위, 04빙결, 05기절, 06공포, 07위압, 08부식
    // 09철갑, 10집중, 11흡수, 12가시
    // public int[] debuffArr = new int[9];
    // public int[] buffArr = new int[4];
    public int[] effectArr = new int[13];   // player, Enemy에게서 받은 Effect 전부

    private int[] UIOrderArr = new int[13];  // UI 배치 순서 배열

    [Header ("Sprite")]
    public Sprite[] spriteArr;

    [Header ("Others")]
    private bool isConvert;
    private int order;   // stateUI 현재 순서

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

        for (int i = 0; i < UIOrderArr.Length; i++) {
            UIOrderArr[i] = -1;
        }

        order = 0;
        isConvert = false;
    }

    // 데이터 갱신
    void LateUpdate()
    {
        switch (targetType) {
            case TargetInfo.Player:
                // 모든 버프/디버프 불러오기
                for (int i = 0; i < GameManager.instance._player.debuffArr.Length; i++) 
                {
                    effectArr[i] = GameManager.instance._player.debuffArr[i];
                }
                for (int i = 9; i < GameManager.instance._player.buffArr.Length + 9; i++) 
                {
                    effectArr[i] = GameManager.instance._player.buffArr[i - 9];
                }
                break;
            case TargetInfo.Enemy:
                if (enemyScript == null) {
                    return;     // 오류 방지
                }

                // 모든 버프/디버프 불러오기
                for (int i = 0; i < enemyScript.debuffArr.Length; i++) 
                {
                    effectArr[i] = enemyScript.debuffArr[i];
                }
                for (int i = 9; i < enemyScript.buffArr.Length + 9; i++) 
                {
                    effectArr[i] = enemyScript.buffArr[i - 9];
                }
                break;
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
                        isConvert = true;
                        break;
                    }
                    // 이미 생성된 UI가 있는 경우 _ 사라진 버프/디버프 UI 제거
                    else {
                        for (int b = 0; b <= UIOrderArr.Length - a; b++)
                        {
                            // UI가 비활성화되어 있는 경우 || UI가 마지막인 경우
                            if (!UIArr[a + b].enabled || b == UIOrderArr.Length - a) {
                                UIOrderArr[a + b - 1] = -1;
                                UIArr[a + b - 1].enabled = false;
                                textArr[a + b - 1].enabled = false;
                                break;
                            }
                            UIOrderArr[a + b] = UIOrderArr[a + b + 1];
                            UIArr[a + b].sprite = UIArr[a + b + 1].sprite;
                            textArr[a + b].text = textArr[a + b + 1].text;
                        }
                        order--;
                    }
                }
            }
            // 생성된 UI가 없는 경우 _ UI 생성
            if (effectArr[i] > 0) 
            {
                if (isConvert == false) {
                    if (order >= 6) {
                    // UI 꽉참.
                    Debug.Log("UI가 전부 찼습니다.");
                    }
                    else {
                        UIOrderArr[order] = i;

                        // UI 활성화
                        UIArr[order].enabled = true;
                        textArr[order].enabled = true;

                        UIArr[order].sprite = spriteArr[i];
                        textArr[order].text = effectArr[i].ToString();

                        order++;
                    }
                }
            }
            isConvert = false;
        }
    }
}
*/