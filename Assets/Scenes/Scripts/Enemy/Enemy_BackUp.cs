using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_BackUp : MonoBehaviour
{
    /*
    [Header ("Reference")]
    private SpriteRenderer target_SpriteRenderer;
    private SetPattern _SetPattern;

    [Header ("Enemy Data")]
    public EnemyData data;
    // private int[] enemyAct = new int[5];

    [Header ("Turn")]
    public int turn;

    [Header ("Status")]
    public int maxHealth;
    public int health;
    public int shield;

    [Header ("Buff / Debuff")]
    // 00화상, 01중독, 02감전, 03추위, 04빙결, 05기절 06공포, 07위압, 08부식
    // 00철갑, 01집중, 02흡수, 03가시
    public int[] debuffArr = new int[9];
    public int[] buffArr = new int[4];

    [Header ("Enemy Skill Figure")]
    public int skillDamage;
    public int skillCount;
    public int skillShield;
    public int effectType;
    public int effectNum;
    public int skillHeal;

    // [Header ("Skill Order")]
    // private int skillOrder;
    // private int maxSkillOrder;

    [Header ("Skill Data")]
    private MonsterSkillData skillData;

    [Header ("Placement Order")]
    public int order = 0;  // 적 순서 (배치 넘버)
    
    void Awake()
    {
        target_SpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _SetPattern = transform.gameObject.GetComponent<SetPattern>();

        turn = GameManager.instance.turn;
        maxHealth = data.maxHealth;
        health = maxHealth;

        // maxSkillOrder = data.skillNum;
    }

    void Update() 
    {
        // 만약 단일 타겟일 시 대상 타겟팅
        if (gameObject == GameManager.instance.target) {
            target_SpriteRenderer.enabled = true;
        }
        // 공격 타입이 전체 공격기 혹은 바운스 공격기일 시 전체 타겟팅
        else {
            if (GameManager.instance.usingSkill.attackType == SkillData.AttackType.Splash
                 || GameManager.instance.usingSkill.attackType == SkillData.AttackType.Bounce) {
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

            GameManager.instance.EnemyInfoList.Remove(gameObject.GetComponent<Enemy>());    // GameManager에서 Enemy 스크립트 정보 삭제
            GameManager.instance.EnemyList.Remove(gameObject);      // GameManager에서 Enemy 오브젝트 삭제
            GameManager.instance._HUDManager.DeActivateHUD(order);
            //GameManager.instance.EnemyNum--;
            //isLive = false;
            //gameObject.SetActive(false);

            // 플레이어의 승리 확인
            GameManager.instance.CheckVictory();

            Destroy(gameObject);    // 적 제거
        }
    }

    // 적 패턴 정보 가져오기
    public void TakeSkillInfo()
    {
        skillData = _SetPattern.SetSkill();

        // 최종 데미지 = 기본 데미지 + 집중 수치 + 플레이어 화상 수치
        skillDamage = skillData.baseDamage + buffArr[1] + GameManager.instance._player.debuffArr[0];
        skillCount = skillData.baseCount;
        skillShield = skillData.baseShield;
        effectType = skillData.effectType;
        effectNum = skillData.baseEffect;
        skillHeal = skillData.baseHeal;

        /* switch (skillOrder) {
            case 0:
                skillOrder++;
                enemyAct[0] = data.damage_01;
                enemyAct[1] = data.shield_01;
                enemyAct[2] = data.effectType_01;
                enemyAct[3] = data.effectNum_01;
                enemyAct[4] = data.heal_01;
                break;
            case 1:
                skillOrder++;
                enemyAct[0] = data.damage_02;
                enemyAct[1] = data.shield_02;
                enemyAct[2] = data.effectType_02;
                enemyAct[3] = data.effectNum_02;
                enemyAct[4] = data.heal_02;
                break;
            case 2:
                skillOrder++;
                enemyAct[0] = data.damage_03;
                enemyAct[1] = data.shield_03;
                enemyAct[2] = data.effectType_03;
                enemyAct[3] = data.effectNum_03;
                enemyAct[4] = data.heal_03;
                break;
            case 3:
                skillOrder++;
                enemyAct[0] = data.damage_04;
                enemyAct[1] = data.shield_04;
                enemyAct[2] = data.effectType_04;
                enemyAct[3] = data.effectNum_04;
                enemyAct[4] = data.heal_04;
                break;
            case 4:
                skillOrder++;
                enemyAct[0] = data.damage_05;
                enemyAct[1] = data.shield_05;
                enemyAct[2] = data.effectType_05;
                enemyAct[3] = data.effectNum_05;
                enemyAct[4] = data.heal_05;
                break;
        }
        if (skillOrder >= maxSkillOrder) {
            skillOrder = 0;
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
        } */

        /*
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
    */
}
