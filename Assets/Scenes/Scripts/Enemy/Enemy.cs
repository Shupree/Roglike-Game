using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float[] enemyAct = new float[2];
    public string effectType;
    public int effectNum;
    public int turn;
    public EnemyData data;
    public float health;
    public float maxHealth;
    public float shield;
    // 화상, 중독, 감전, 추위, 빙결, 집중
    public int[] effectArr = new int[6];
    public float damage;

    public bool isLive;
    
    void Awake()
    {
        turn = 0;
        maxHealth = data.maxHealth;
        health = maxHealth;
    }

    void FixedUpdate() 
    {
        // 감전 효과 (보호막 무시 데미지)
        /*if (effectArr[2] >= 5)
        {
            health -= 7;
            Debug.Log(gameObject.name+"은(는) 감전으로 7의 데미지를 입었다!");
            effectArr[2] -= 5;
        }*/

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
        switch(data.enemyId) 
        {
            case 0:
                enemyAct = GetComponent<DummyBot>().Pattern(turn);
                break;
        }

        // 공격(Attack)일 경우
        if(enemyAct[0] == 1)
        {
            damage = enemyAct[1];
        }

        // 효과(Effect)일 경우
        else if(enemyAct[0] == 1)
        {

        }
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
