using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public StateUpdate _StateScript;
    public int health;
    public int maxHealth = 100;
    public int shield;
    // 0화상, 1중독, 2감전, 3추위, 4빙결, 5집중
    public int[] effectArr = new int[6];

    public void Awake()
    {
        // 시작 시 HP 설정
        health = maxHealth;
        shield = 0;
    }

    public void Update()
    {
        
    }

    // 감전 효과 확인
    public void ElectricShock()
    {
        if (effectArr[2] >= 5)
        {
            // 감전 데미지 연산
            health -= 7;
            Debug.Log(gameObject.name+"은(는) 감전으로 7의 데미지를 입었다!");
            // 감전 효과 - 5
            effectArr[2] -= 5;
        }
    }

    // 중독 효과 확인
    public void Poison()
    {
        // 중독 데미지 연산
        health -= effectArr[1];
        // 중독 효과를 절반으로 상실
        effectArr[1] -= effectArr[1] / 2;
    }

    // 추위 효과 확인
    public void Coldness()
    {
        // 추위 효과
        if (effectArr[3] >= 6) {
            // 즉시 추위 스택을 0으로 치환
            effectArr[3] = 0;
            // 빙결 효과 추가
            effectArr[4] = 1;
        }
    }
}
