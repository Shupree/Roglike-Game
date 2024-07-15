using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int[] enemyAct = new int[3];
    public int effectType;
    public int effectNum;
    public int turn;
    public EnemyData data;
    public int health;
    public int maxHealth;
    public int shield;
    // 00화상, 01중독, 02감전, 03추위, 04빙결, 05기절 06공포, 07위압, 08부식
    // 00철갑 보호막, 01집중, 02흡수, 03가시
    public int[] debuffArr = new int[9];
    public int[] buffArr = new int[4];
    public int damage;

    public bool isLive;
    
    void Awake()
    {
        turn = 0;
        maxHealth = data.maxHealth;
        health = maxHealth;
    }

    void FixedUpdate() 
    {
        if (health <= 0)
        {
            Debug.Log("적 처치");

            GameManager.instance.EnemyArr.Remove(gameObject);
            //GameManager.instance.EnemyNum--;
            //isLive = false;
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void TakeActInfo()
    {
        turn++;
        // 몬스터에 따른 공격 방식 선정
        switch (data.enemyId) {
            // DummyBot
            case 1:
                enemyAct = GetComponent<DummyBot>().SetPattern(turn);
                break;

            // DummyBot2
            case 2:
                enemyAct = GetComponent<DummyBot2>().SetPattern(turn);
                break;
        }

        // 스킬 데미지 확정
        if (enemyAct[0] != 0) {
            damage = enemyAct[0];
        }
        
        // 효과 없음
        if (enemyAct[1] == 0) {
            effectType = 0;
            effectNum = 0;
        }
        // 디버프 효과
        else if (enemyAct[1] <= 10) {
            effectType = enemyAct[1];
            effectNum = enemyAct[2];
        }
        // 버프 효과
        else if (enemyAct[1] > 10) {
            switch (enemyAct[1]) {
                // 회복
                case 11:
                    health += enemyAct[2];
                    if (maxHealth < health) {
                        health = maxHealth;
                    }
                    break;
                
                // 보호막
                case 12:
                    shield += enemyAct[2];
                    break;
                // 집중
                case 13:
                    buffArr[1] += enemyAct[2];
                    break;
            }
        }
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
