using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // private StateUpdate _StateScript;
    public int health;
    public int maxHealth = 100;
    public int shield;

    // 00화상, 01중독, 02감전, 03추위, 04빙결, 05기절, 06공포, 07위압, 08부식
    // 00철갑 보호막, 01집중, 02흡수, 03가시
    public int[] debuffArr = new int[9];
    public int[] buffArr = new int[4];
    public int[] itemArr = new int[6];      // 사용 중인 장신구

    public int gold;        // 소지금

    public void Awake()
    {
        // 시작 시 HP 설정
        health = maxHealth;
        shield = 0;

        // _StateScript = transform.Find("Canvas").gameObject.transform.Find("StateUI").gameObject.GetComponent<StateUpdate>();
    }

    // 매턴마다 버프/디버프 감소
    public void DecStatusEffect()
    {
        debuffArr[0] /= 2;  // 화상
        debuffArr[6] -= 1;  // 공포
        debuffArr[7] -= 1;  // 위압
        debuffArr[8] -= 1;  // 부식
        buffArr[0] -= 1;    // 철갑
        buffArr[1] = 0;     // 집중
        buffArr[2] = 0;     // 흡수
        buffArr[3] = 0;     // 가시

        for (int i = 0; i < debuffArr.Length; i++) {
            if (debuffArr[i] < 0) {
                debuffArr[i] = 0;
            }
            if (i < buffArr.Length && buffArr[i] < 0) {
                buffArr[i] = 0;
            }
        }
    }

    // 감전 효과 확인
    public void ElectricShock()
    {
        if (debuffArr[2] >= 5)  // 조건 : 감전 5스택
        {
            // 감전 데미지 연산
            health -= 7;
            Debug.Log(gameObject.name+"은(는) 감전으로 7의 데미지를 입었다!");
            // 감전 수치 - 5
            debuffArr[2] -= 5;
        }
    }

    // 중독 효과 확인
    public void Poison()
    {
        if (debuffArr[1] > 0) {
            // 중독 데미지 연산
            health -= debuffArr[1];

            Debug.Log("플레이어는 중독으로 "+debuffArr[1]+"의 데미지를 입었다!");

            // 중독 수치 감소
            debuffArr[1] /= 2;
        }
    }

    // 추위 효과 확인
    public void Coldness()
    {
        // 추위 효과
        if (debuffArr[3] >= 6) {
            // 즉시 추위 스택을 0으로 치환
            debuffArr[3] = 0;
            // 빙결 효과 추가
            debuffArr[4] = 1;

            Debug.Log("빙결!");
        }
    }
}
