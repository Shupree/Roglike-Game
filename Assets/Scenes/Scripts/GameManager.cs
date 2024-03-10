using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 1마리 처치 시 즉시 전투가 종료되는 오류 발생!!!!
// 버그 픽스할 것!
public class GameManager : MonoBehaviour
{
    // static을 통해 메모리에 정보를 저장 후 타 스크립트에서 사용 가능.
    public static GameManager instance;

    public PaintManager _PaintManager = new PaintManager();
    public PlayerInfo _PlayerInfo = new PlayerInfo();

    public enum State
    {
        start, playerTurn, enemyTurn, win
    }

    public State state;
    public bool isLive;  // 적 생존 여부
    public float health;
    public float maxHealth = 100;
    public Player player;
    public GameObject target;
    public List<GameObject> EnemyArr;   // 적 정보
    //public int EnemyNum;    // 적 수
    public SkillData[] red_SkillData;
    public SkillData[] blue_SkillData;
    public SkillData[] yellow_SkillData;
    private SkillData UsingSkill;

    // 초기화
    void Awake()
    {
        instance = this;

        state = State.start;    // 전투 시작 알림

        // 적 정보 가져오는 코드

        BattleStart();
    }

    void Start()
    {
        // 시작 시 HP 설정
        health = maxHealth;
    }

    void BattleStart()
    {
        isLive = true;
        Debug.Log(isLive);
        // 전투 시작 시 캐릭터 등장 애니메이션 등 효과 넣기

            // 플레이어나 적에게 턴 넘기기
        state = State.playerTurn;
    }


    // 

    // 공격 버튼
    public void PlayerAttackBtn()
    {
        // 공격 스킬, 데미지 등 코드 작성

                // 버튼이 계속 눌리는 거 방지하기 위함
        if(state != State.playerTurn)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }

    // 

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("플레이어 공격");
        // 공격 스킬, 데미지 등 코드 작성
        switch (_PaintManager.paints[0]) {
            // Red 타입 스킬
            case 1:
                UsingSkill = red_SkillData[_PlayerInfo.skillArr[0]];
                Debug.Log(UsingSkill.skillName);
                break;
            // blue 타입 스킬
            case 2:
                UsingSkill = blue_SkillData[_PlayerInfo.skillArr[1]];
                Debug.Log(UsingSkill.skillName);
                break;
            // yellow 타입 스킬
            case 3:
                UsingSkill = yellow_SkillData[_PlayerInfo.skillArr[2]];
                Debug.Log(UsingSkill.skillName);
                break;
            default:
                Debug.Log("페인트를 선택하지 않았습니다.");
                break;
        }

        // 공격 type에 따라 분류할 것!
        target.GetComponent<Enemy>().health = target.GetComponent<Enemy>().health - UsingSkill.baseDamage;
        Debug.Log("플레이어의 공격! "+target.name+"에게"+UsingSkill.baseDamage+"의 데미지!");

        // 팔레트 초기화
        _PaintManager.ClearPaint();

        // 적 죽었으면 전투 종료
        if(EnemyArr.Count == 0)
        {
            isLive = false;
            state = State.win;
            EndBattle();
            // 적 살았으면 적에게 턴 넘기기
        }
        else
        {
            state = State.enemyTurn;
            StartCoroutine(EnemyTurn());
            Debug.Log("적의 턴입니다.");

        }
    }

    void EndBattle()
    {
        Debug.Log("전투 종료");
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        // 적 공격 코드
        for(int i = 0; i < EnemyArr.Count; i++)
        {
            EnemyArr[i].GetComponent<Enemy>().TakeActInfo();
            health = health - EnemyArr[i].GetComponent<Enemy>().damage;

            Debug.Log(EnemyArr[i].name+"의 공격! 플레이어에게"+EnemyArr[i].GetComponent<Enemy>().damage+"의 데미지!");
        }

        // 적 공격 끝났으면 플레이어에게 턴 넘기기
        if(EnemyArr.Count == 0)
        {
            isLive = false;
            state = State.win;
            EndBattle();
        }

        state = State.playerTurn;
        Debug.Log("플레이어의 턴입니다.");
    }
}

// 페인트 정보 저장
public class PaintManager
{
    // 배열보다도 큐를 써볼 것!
    int limit = 2;
    int order = 0;
    public int[] paints = new int[5];
    // white = 0, red = 1, blue = 2, yellow = 3, black = 4
    public PaintManager() {
        paints[0] = 0;
        paints[1] = 0;
        paints[2] = 0;
        paints[3] = 0;
        paints[4] = 0;
    }

    public void AddPaint(int num) {
        if (order <= limit) {
            paints[order] = num;
            order++;

            // 팔레트 UI 교체
            
        }
        else{
            Debug.Log("이미 팔레트가 꽉찼어!");
        }
    }

    public void ClearPaint() {
        paints[0] = 0;
        paints[1] = 0;
        paints[2] = 0;
        paints[3] = 0;
        paints[4] = 0;
    }
}

// 플레이어 정보
public class PlayerInfo
{
    // Red, Blue, Yellow
    public int[] skillArr = new int[3];
    public int[] itemArr = new int[6];
    public PlayerInfo()
    {
        skillArr[0] = 0;
        skillArr[1] = 0;
        skillArr[2] = 0;
    }

    public void ChangeSkill()
    {
        //switch () {

        //}
    }
}