using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private StateUpdate _StateScript;
    public int health;
    public int maxHealth = 100;
    public int shield;
    // 00화상, 01중독, 02감전, 03추위, 04빙결, 05기절, 06공포, 07위압, 08부식
    // 00철갑 보호막, 01집중, 02흡수, 03가시
    public int[] debuffArr = new int[9];
    public int[] buffArr = new int[4];

    public void Awake()
    {
        // 시작 시 HP 설정
        health = maxHealth;
        shield = 0;

        _StateScript = transform.Find("Canvas").gameObject.transform.Find("StateUI").gameObject.GetComponent<StateUpdate>();
    }

    public void Update()
    {
        
    }

    // 감전 효과 확인
    public void ElectricShock()
    {
        if (debuffArr[2] >= 5)
        {
            // 감전 데미지 연산
            health -= 7;
            Debug.Log(gameObject.name+"은(는) 감전으로 7의 데미지를 입었다!");
            // 감전 효과 - 5
            debuffArr[2] -= 5;
        }
    }

    // 중독 효과 확인
    public void Poison()
    {
        // 중독 데미지 연산
        health -= debuffArr[1];
        // 중독 효과를 절반으로 상실
        debuffArr[1] -= debuffArr[1] / 2;
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
        }
    }
}
