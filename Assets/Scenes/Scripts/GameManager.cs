using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 사망 시 추가
// StateUpdate 전부 리스트로 바꿀 것!

// _PaintUI, EnemyArr를 전부 하위 컴포넌트 대상으로 하는 Arr로 바꿔야할듯..
public class GameManager : MonoBehaviour
{
    // static을 통해 메모리에 정보를 저장 후 타 스크립트에서 사용 가능.
    public static GameManager instance;

    public Camera _MainCamera;
    public GameObject _Palette;
    public GameObject[] _NextStageUI;
    public GameObject _CanvasUI;
    public GameObject[] _PaintUI;   // [0]빨강 [1]파랑 [2]노랑 [3]하양
    public SpawnManager _SpawnManager;
    public BagManager _BagManager;
    public PaintManager _PaintManager = new PaintManager();
    public StageManager _StageManager = new StageManager();

    public enum State
    {
        rest, start, playerTurn, enemyTurn, win, defeat
    }

    public State state;
    public bool isLive;  // 적 생존 여부
    public Player _player;
    public int[] enemyInfo = new int[4];    // 적 정보
    public GameObject target;
    public List<GameObject> EnemyArr;   // 적 오브젝트
    //public int EnemyNum;    // 적 수
    public MonsterStageData[] EnemySetData;
    private int randomSetNum;
    private int damage;
    //private int map;
    public SkillData usingSkill;
    public MasterPieceData MP_Data;

    // 초기화
    void Awake()
    {
        instance = this;
        _SpawnManager = gameObject.GetComponent<SpawnManager>();

        // 버튼 비활성화
        _NextStageUI[0].SetActive(false);
        _NextStageUI[1].SetActive(false);
        _NextStageUI[2].SetActive(false);

        // 테스트 맵
        //map = 0;

        enemyInfo[0] = 1;
        enemyInfo[3] = 2;

        BattleStart();
    }

    private void Update() 
    {
        if (_player.health <= 0) {
            state = State.defeat;
        }
    }

    void Start()
    {

    }

    // _MapManager
    public void Map()
    {
        //_MainCamera.transform.position = 
    }

    // 페인트 추가
    public void AddColor(int colorType)
    {
        if (_PaintManager.order == 0) {
            switch (colorType) {
                // Red 타입 스킬
                case 1:
                    usingSkill = _BagManager.use_SkillData[0];
                    break;
                // blue 타입 스킬
                case 2:
                    usingSkill = _BagManager.use_SkillData[1];
                    break;
                // yellow 타입 스킬
                case 3:
                    usingSkill = _BagManager.use_SkillData[2];
                    break;
                // white 타입 스킬
                case 4:
                    usingSkill = _BagManager.use_SkillData[3];
                    break;
            }
            damage = usingSkill.baseDamage + _player.buffArr[0];     // 문제는 걸작 등으로 증가된 데미지, 집중 수치를 얻어야함.
            _CanvasUI.GetComponent<CanvasScript>().ConvertSprite(usingSkill);
        }
        else if (_PaintManager.order > 0) {
            damage += usingSkill.damages[_PaintManager.order];
            Debug.Log("현재 데미지" + damage);
        }
        _PaintManager.AddPaint(colorType);
        _Palette.GetComponent<PaletteManager>().ConvertSprite(colorType);
    }

    // 페인트 초기화
    public void ClearColor()
    {
        _PaintManager.ClearPaint();
        _Palette.GetComponent<PaletteManager>().ClearPalette();
    }

    public void SetCurrntStage(int stageType)
    {
        // 스테이지 정보 입력
        _StageManager.stageInfo = stageType;

        // 버튼 비활성화
        _NextStageUI[0].SetActive(false);
        _NextStageUI[1].SetActive(false);
        _NextStageUI[2].SetActive(false);

        switch (stageType) {
            // 일반 몹 스테이지
            case 1:
                // 적 정보 획득 및 적용
                randomSetNum = UnityEngine.Random.Range(0, 3);
                Debug.Log(randomSetNum);
                enemyInfo[0] = EnemySetData[randomSetNum].monster01;
                enemyInfo[1] = EnemySetData[randomSetNum].monster02;
                enemyInfo[2] = EnemySetData[randomSetNum].monster03;
                enemyInfo[3] = EnemySetData[randomSetNum].monster04;
                break;
        }

        state = State.start;
        BattleStart();
    }

    public void SetNextStageUI()
    {
        _StageManager.SetNextStageInfo();

        // 버튼 활성화
        _NextStageUI[0].SetActive(true);
        _NextStageUI[1].SetActive(true);
        _NextStageUI[2].SetActive(true);

        // 버튼 정보 수정
        _NextStageUI[0].GetComponent<NextStageUI>().GetStageInfo(_StageManager.nextStageInfo[0]);
        _NextStageUI[1].GetComponent<NextStageUI>().GetStageInfo(_StageManager.nextStageInfo[1]);
        _NextStageUI[2].GetComponent<NextStageUI>().GetStageInfo(_StageManager.nextStageInfo[2]);
    }

    void BattleStart()
    {
        state = State.start;    // 전투 시작 알림

        // 물감 최대치로 보충
        for (int i = 0; i < 4; i++)
        {
            _PaintUI[i].GetComponent<Paint>().FillUpPaint();
        }

        // 스킬 리셋
        usingSkill = _BagManager.noneData;

        // 적 정보 가져오는 코드

        // 적 정보 리셋
        /*for (int i = 0; i < 4; i++)
        {
            enemyInfo[i] = 0;
        }*/

        isLive = true;
        Debug.Log(isLive);

        // 전투 시작 시 캐릭터 등장 애니메이션 등 효과 넣기

        // 적 스폰
        _SpawnManager.EnemySpawn(enemyInfo);

        for (int i = 0; i < 4; i++)
        {
            _PaintUI[i].GetComponent<Paint>().canUsePaint = true;
        }

        // 플레이어나 적에게 턴 넘기기
        state = State.playerTurn;
    }


    // 

    // 공격 버튼
    public void PlayerAttackBtn()
    {
        // 물감 선택X 시
        if (_PaintManager.order == 0) {
            return;
        }

        // 물감 버튼 Off
        for (int i = 0; i < 4; i++)
        {
            _PaintUI[i].GetComponent<Paint>().canUsePaint = false;
        }

        // 버튼이 계속 눌리는 거 방지하기 위함
        if(state != State.playerTurn)
        {
            return;
        }

        _PaintManager.stack += _PaintManager.order;     // 사용한 물감 수만큼 스택 적립
        if (MP_Data.cost < _PaintManager.stack) {
            _PaintManager.stack = MP_Data.cost;
        }

        // 공격 단계로
        StartCoroutine(PlayerAttack());
    }

    public void PlayerEraseBtn()
    {
        for (int i = 0; i < 4; i++)
        {
            _PaintUI[i].GetComponent<Paint>().ReturnPaint();
        }
        damage = 0;

        _PaintManager.ClearPaint();
        _Palette.GetComponent<PaletteManager>().ClearPalette();
        _CanvasUI.GetComponent<CanvasScript>().ClearSprite();

        // 스킬 리셋
        usingSkill = _BagManager.noneData;
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("플레이어 공격");
        // 공격 스킬, 데미지 등 코드 작성

        Debug.Log(usingSkill.skillName);
        _CanvasUI.GetComponent<CanvasScript>().ClearSprite();

        // 공격 type에 따른 분류
        switch (usingSkill.attackType) {
            // 단타 공격
            case "Single":
                target.GetComponent<Enemy>().health -= 
                    damage + target.GetComponent<Enemy>().debuffArr[0];

                // 적 디버프
                if (usingSkill.effectType > 0) {
                    if (usingSkill.effectType < 10) {
                        target.GetComponent<Enemy>().debuffArr[usingSkill.effectType - 1] += usingSkill.baseEffect;
                    }
                    else if (usingSkill.effectType < 20) {
                        target.GetComponent<Enemy>().debuffArr[usingSkill.effectType - 11] += usingSkill.baseEffect;
                    }
                }
                Debug.Log("적은 "+usingSkill.baseDamage+"의 데미지를 입었다.");
                break;
            // 다단 히트
            case "Multiple":
                for (int i = 0; i < usingSkill.baseCount; i++) {    // 타수 증가
                    target.GetComponent<Enemy>().health -= 
                        damage + target.GetComponent<Enemy>().debuffArr[0];

                    // 적 디버프
                    if (usingSkill.effectType > 0) {
                        if (usingSkill.effectType < 10) {
                            target.GetComponent<Enemy>().debuffArr[usingSkill.effectType - 1] += usingSkill.baseEffect;
                        }
                        else if (usingSkill.effectType < 20) {
                            target.GetComponent<Enemy>().debuffArr[usingSkill.effectType - 11] += usingSkill.baseEffect;
                        }
                    }
                    Debug.Log("적은 "+usingSkill.baseDamage+"의 데미지를 입었다.");
                }
                break;
            // 전체 공격
            case "Splash":
                for(int i = 0; i < EnemyArr.Count; i++) {
                    EnemyArr[i].GetComponent<Enemy>().health -=
                        damage + target.GetComponent<Enemy>().debuffArr[0];

                    // 적 디버프
                    if (usingSkill.effectType > 0) {
                        if (usingSkill.effectType < 10) {
                            EnemyArr[i].GetComponent<Enemy>().debuffArr[usingSkill.effectType - 1] += usingSkill.baseEffect;
                        }
                        else if (usingSkill.effectType < 20) {
                            EnemyArr[i].GetComponent<Enemy>().debuffArr[usingSkill.effectType - 11] += usingSkill.baseEffect;
                        }
                    }
                }
                Debug.Log("적들은 "+usingSkill.baseDamage+"의 데미지를 입었다.");
                break;
        }
        // 데미지 연산 : 기본 데미지 + 화상 데미지 + 집중 효과

        // 플레이어 보호막
        _player.shield += usingSkill.baseShield;
        Debug.Log("플레이어는 "+usingSkill.baseShield+"의 보호막을 얻었다!");

        // 플레이어 버프
        if (usingSkill.effectType > 20) {
            _player.buffArr[usingSkill.effectType - 21] += usingSkill.baseEffect;
        }

        // 색 초기화
        ClearColor();

        // 합산 데미지 초기화
        damage = 0;

        // 플레이어 중독 효과 연산
        _player.Poison();

        for(int i = 0; i < EnemyArr.Count; i++) {
            // 감전 효과 연산
            EnemyArr[i].GetComponent<Enemy>().ElectricShock();
            // 추위 효과 연산
            EnemyArr[i].GetComponent<Enemy>().Coldness();

        }

        // 스킬 리셋
        usingSkill = _BagManager.noneData;

        yield return new WaitForSeconds(1f);

        // 적 죽었으면 전투 종료
        if(EnemyArr.Count == 0)
        {
            isLive = false;

            // 적 정보 리셋
            for (int i = 0; i < 4; i++)
            {
                enemyInfo[i] = 0;
            }

            state = State.win;
            EndBattle();
        }
        else
        {
            // 적 살았으면 적에게 턴 넘기기
            state = State.enemyTurn;
            StartCoroutine(EnemyTurn());
            Debug.Log("적의 턴입니다.");

        }
    }

    void EndBattle()
    {
        Debug.Log("전투 종료");
        state = State.rest;

        SetNextStageUI();
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        // 적 공격 코드
        for(int i = 0; i < EnemyArr.Count; i++)
        {
            if (!EnemyArr[i]) {
                continue;
            }

            // 적 보호막 리셋
            EnemyArr[i].GetComponent<Enemy>().shield = 0;

            // 빙결 상태 확인
            if (EnemyArr[i].GetComponent<Enemy>().debuffArr[4] > 0) {
                EnemyArr[i].GetComponent<Enemy>().debuffArr[4] -= 1;
                continue;
            }

            // 적 행동 확정
            EnemyArr[i].GetComponent<Enemy>().TakeActInfo();

            // 공격 연산 (데미지 + 집중 + 플레이어_화상)
            damage = EnemyArr[i].GetComponent<Enemy>().damage;
            if (_player.shield != 0) {
                if (_player.shield >= damage) {
                    _player.shield -= damage;
                    damage = 0;
                }
                else {
                    damage -= _player.shield;
                    _player.shield = 0;
                }
            }
            _player.health = _player.health - damage;

            // 디버프 효과 연산
            if (EnemyArr[i].GetComponent<Enemy>().effectType > 0) {
                // 기본 디버프
                if (EnemyArr[i].GetComponent<Enemy>().effectType < 10) {
                    _player.debuffArr[EnemyArr[i].GetComponent<Enemy>().effectType - 1] += EnemyArr[i].GetComponent<Enemy>().effectNum;
                }
            }

            Debug.Log(EnemyArr[i].name+"의 공격! 플레이어에게"+damage+"의 데미지!");

            // 합산 데미지 초기화
            damage = 0;

            // 적 중독 효과
            EnemyArr[i].GetComponent<Enemy>().Poison();

            // 플레이어 빙결 효과
            _player.Coldness();
            // 플레이어 감전 효과
            _player.ElectricShock();
        }

        for(int i = 0; i < EnemyArr.Count; i++) {
            // 감전 효과 연산
            EnemyArr[i].GetComponent<Enemy>().ElectricShock();
            // 추위 효과 연산
            EnemyArr[i].GetComponent<Enemy>().Coldness();

        }

        yield return new WaitForSeconds(1f);

        // 승리 시
        if(EnemyArr.Count == 0)
        {
            isLive = false;
            state = State.win;
            EndBattle();
        }
        // 적 공격 끝났으면 플레이어에게 턴 넘기기
        else {
            // 플레이어 물감 회복
            for (int i = 0; i < 4; i++)
            {
                _PaintUI[i].GetComponent<Paint>().FillPaint();
            }

            // 턴에 따른 버프/디버프 감소
            _player.DecStatusEffect();
            for(int i = 0; i < EnemyArr.Count; i++)
            {
                if (!EnemyArr[i]) {
                    continue;
                }
                EnemyArr[i].GetComponent<Enemy>().DecStatusEffect();
            }

            // 플레이어 빙결 효과
            if(_player.debuffArr[4] > 0)
            {
                _player.debuffArr[4] = 0;
                Debug.Log("빙결!");

                state = State.enemyTurn;
                StartCoroutine(EnemyTurn());
                Debug.Log("적의 턴입니다.");
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    _PaintUI[i].GetComponent<Paint>().canUsePaint = true;
                }

                state = State.playerTurn;
                Debug.Log("플레이어의 턴입니다.");
            }
        }
    }
}

// 스테이지 정보 관리
public class StageManager
{
    // 현재 스테이지 수
    public int stageNum = 0;
    // 현재 스테이지 정보 (0비어있음, 1일반몹, 2엘리트몹, 3보스몹, 4상자, 5이벤트, 6상점)
    public int stageInfo = 0;
    public int randomNum = 0;
    public int[] nextStageInfo = new int[3];

    //초기화
    public StageManager() {
        nextStageInfo[0] = 0;
        nextStageInfo[1] = 0;
        nextStageInfo[2] = 0;
    }

    public void SetNextStageInfo() {
        // 일반몹 스테이지 강제
        if (stageNum == 1) {
            nextStageInfo[0] = 1;
            nextStageInfo[1] = 1;
            nextStageInfo[2] = 0;
        }
        // 엘리트몹 스테이지 강제
        else if (stageNum == 3) {
            nextStageInfo[0] = 2;
            nextStageInfo[1] = 2;
            nextStageInfo[2] = 0;
        }
        // 상점 스테이지 선택
        else if (stageNum == 5) {
            nextStageInfo[0] = 1;
            nextStageInfo[1] = 6;
            nextStageInfo[2] = 0;
        }
        // 보스몹 스테이지 강제
        else if (stageNum == 6) {
            nextStageInfo[0] = 3;
            nextStageInfo[1] = 0;
            nextStageInfo[2] = 0;
        }
        // 스테이지 랜덤 설정
        else {
            randomNum = UnityEngine.Random.Range(1, 7);

            switch (randomNum) {
                case 1:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 4;
                    nextStageInfo[2] = 0;
                    break;
                case 2:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 5;
                    nextStageInfo[2] = 0;
                    break;
                case 3:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 5;
                    nextStageInfo[2] = 0;
                    break;
                case 4:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 4;
                    nextStageInfo[2] = 5;
                    break;
                case 5:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 6;
                    nextStageInfo[2] = 0;
                    break;
                case 6:
                    nextStageInfo[0] = 1;
                    nextStageInfo[1] = 5;
                    nextStageInfo[2] = 6;
                    break;
            }
        }
    }
}

// 페인트 정보 저장
public class PaintManager
{
    // 최대 페인트 수
    public int limit = 2;
    public int order = 0;
    public int[] paints = new int[5];
    // none = 0, red = 1, blue = 2, yellow = 3, white = 4
    public int stack;   // 물감 사용 스택 수
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

    // 페인트 초기화
    public void ClearPaint() {
        for (int i = 0; i < 5; i++)
        {
            order = 0;
            paints[i] = 0;
        }
    }
}