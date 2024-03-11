using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 사망 시 추가
// 이펙트 제작 중이었음.
public class GameManager : MonoBehaviour
{
    // static을 통해 메모리에 정보를 저장 후 타 스크립트에서 사용 가능.
    public static GameManager instance;

    public GameObject _Palette;
    public SpawnManager _SpawnManager;
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
    public float shield;
    // 화상, 중독, 감전, 추위, 빙결, 집중
    public int[] effectArr = new int[6];
    public Player player;
    public int[] EnemyInfo = new int[4];    // 적 정보
    public GameObject target;
    public List<GameObject> EnemyArr;   // 적 오브젝트
    //public int EnemyNum;    // 적 수
    public SkillData[] red_SkillData;
    public SkillData[] blue_SkillData;
    public SkillData[] yellow_SkillData;
    private SkillData UsingSkill;

    // 초기화
    void Awake()
    {
        instance = this;
        _SpawnManager = gameObject.GetComponent<SpawnManager>();
        Debug.Log(gameObject.GetComponent<SpawnManager>());

        for (int i = 0; i < 4; i++)
        {
            EnemyInfo[i] = 0;
        }

        state = State.start;    // 전투 시작 알림

        // 적 정보 가져오는 코드

        BattleStart();
    }

    void Start()
    {
        // 시작 시 HP 설정
        health = maxHealth;
        shield = 0;
    }

    // 페인트 추가
    public void AddColor(int colorType)
    {
        if (_PaintManager.order <= _PaintManager.limit) {
            _PaintManager.AddPaint(colorType);
            _Palette.GetComponent<PaletteManager>().ConvertImage(colorType);
        }
        else {
            Debug.Log("팔레트가 이미 꽉 찼어!");
        }
    }

    // 페인트 초기화
    public void ClearColor()
    {
        _PaintManager.ClearPaint();
        _Palette.GetComponent<PaletteManager>().ClearPalette();
    }

    void BattleStart()
    {
        isLive = true;
        Debug.Log(isLive);
        // 전투 시작 시 캐릭터 등장 애니메이션 등 효과 넣기

        // int EnemyId
        EnemyInfo[0] = 1;
        EnemyInfo[1] = 1;
        _SpawnManager.EnemySpawn(EnemyInfo);

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
                // 애초에 버튼 클릭이 안되도록 수정할 것!
                Debug.Log("페인트를 선택하지 않았습니다.");
                break;
        }

        // 공격 type에 따라 분류할 것!
        switch (UsingSkill.attackType) {
            case "Single":
                break;
            case "Multiple":
                break;
            case "Splash":
                break;
        }

        // 데미지 연산 : 기본 데미지 + 화상 데미지 + 집중 효과
        target.GetComponent<Enemy>().health -= 
            UsingSkill.baseDamage + target.GetComponent<Enemy>().effectArr[0] + effectArr[5];
        Debug.Log("플레이어의 공격! "+target.name+"에게"+UsingSkill.baseDamage + target.GetComponent<Enemy>().effectArr[0]+"의 데미지!");

        shield += UsingSkill.baseShield;
        Debug.Log("플레이어는 "+UsingSkill.baseShield+"의 보호막을 얻었다!");

        if (UsingSkill.effectType != 0 && UsingSkill.effectType <= 4 ) {
            target.GetComponent<Enemy>().effectArr[UsingSkill.effectType - 1] += UsingSkill.baseEffect;
        }
        else if (UsingSkill.effectType > 4) {
            effectArr[UsingSkill.effectType - 1] += UsingSkill.baseEffect;
        }

        ClearColor();

        // 감전 효과 연산
        target.GetComponent<Enemy>().ElectricShock();
        // 추위 효과 연산
        target.GetComponent<Enemy>().Coldness();

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
            // 빙결 상태 확인
            if (EnemyArr[i].GetComponent<Enemy>().effectArr[4] > 0) {
                EnemyArr[i].GetComponent<Enemy>().effectArr[4] -= 1;
                continue;
            }

            EnemyArr[i].GetComponent<Enemy>().TakeActInfo();
            health = health - EnemyArr[i].GetComponent<Enemy>().damage;

            Debug.Log(EnemyArr[i].name+"의 공격! 플레이어에게"+EnemyArr[i].GetComponent<Enemy>().damage+"의 데미지!");

            // 중독 효과
            EnemyArr[i].GetComponent<Enemy>().Poison();
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
    // 최대 페인트 수
    public int limit = 2;
    public int order = 0;
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
        paints[order] = num;
        order++;
    }

    // 저장된 색 초기화
    public void ClearPaint() {
        for (int i = 0; i < 5; i++)
        {
            order = 0;
            paints[i] = 0;
        }
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