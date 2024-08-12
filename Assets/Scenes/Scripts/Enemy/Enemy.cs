using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer target_SpriteRenderer;

    public EnemyData data;
    public int[] enemyAct = new int[5];

    public int turn;

    public int maxHealth;
    public int health;
    public int shield;
    // 00화상, 01중독, 02감전, 03추위, 04빙결, 05기절 06공포, 07위압, 08부식
    // 00철갑, 01집중, 02흡수, 03가시
    public int[] debuffArr = new int[9];
    public int[] buffArr = new int[4];

    public int skillDamage;
    public int skillShield;
    public int effectType;
    public int effectNum;
    public int skillHeal;

    public bool isLive;
    
    void Awake()
    {
        target_SpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        turn = GameManager.instance.turn;
        maxHealth = data.maxHealth;
        health = maxHealth;
    }

    void Update() 
    {
        if (gameObject == GameManager.instance.target) {
            target_SpriteRenderer.enabled = true;
        }
        else {
            if (GameManager.instance.usingSkill.attackType == SkillData.AttackType.Splash) {
                target_SpriteRenderer.enabled = true;
            }
            else {
                target_SpriteRenderer.enabled = false;
            }
        }

        // 적 처치
        if (health <= 0)
        {
            Debug.Log("적 처치");

            GameManager.instance.EnemyInfoList.Remove(gameObject.GetComponent<Enemy>());
            GameManager.instance.EnemyList.Remove(gameObject);
            //GameManager.instance.EnemyNum--;
            //isLive = false;
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void TakeActInfo()
    {
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
            skillDamage = enemyAct[0] + buffArr[1] + GameManager.instance._player.debuffArr[0];
        }

        // 보호막 수치 확정
        if (enemyAct[1] != 0) {
            skillShield = enemyAct[1];
        }
        
        // 효과 없음
        if (enemyAct[2] == 0) {
            effectType = 0;
            effectNum = 0;
        }
        // 버프/디버프 효과
        else {
            effectType = enemyAct[2];
            effectNum = enemyAct[3];
        }

        // 회복 수치 확정
        if (enemyAct[4] != 0) {
            skillHeal = enemyAct[4];
        }
    }

    // 매턴마다 버프/디버프 감소
    public void DecStatusEffect()
    {
        debuffArr[0] /= 2;
        debuffArr[6] -= 1;
        debuffArr[7] -= 1;
        debuffArr[8] -= 1;
        buffArr[0] -= 1;
        buffArr[1] = 0;
        buffArr[2] = 0;
        buffArr[3] = 0;

        for (int i = 0; i < debuffArr.Length; i++) {
            // 혹시 모를 음수 차단
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
        if (debuffArr[1] > 0) {
            // 중독 데미지 연산
            health -= debuffArr[1];

            Debug.Log(gameObject.name+"은(는) 중독으로 "+debuffArr[1]+"의 데미지를 입었다!");

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
