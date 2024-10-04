using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 사망 시 추가
// StateUpdate 전부 리스트로 바꿀 것!

public class GameManager : MonoBehaviour
{
    // static을 통해 메모리에 정보를 저장 후 타 스크립트에서 사용 가능.
    public static GameManager instance;

    public Camera _MainCamera;
    public GameObject[] _NextStageUI;
    public GameObject _CanvasUI;
    public Paint[] _PaintScripts;   // [0]빨강 [1]파랑 [2]노랑 [3]하양
    public SpawnManager _SpawnManager;
    public SkillManager _SkillManager;
    public MPManager _MasterPieceManager;
    public ArtifactManager _ArtifactManager;
    public ThemeManager _ThemeManager;
    public PaletteManager _PaletteManager;
    public LootUI _LootManager;
    public StageManager _StageManager = new StageManager();
    public Player _player;

    public enum State
    {
        rest, start, playerTurn, playerAttack, enemyTurn, win, defeat
    }

    public State state;

    public bool isLive;  // 적 생존 여부
    public int[] enemyID = new int[4];    // 적 정보
    public GameObject target;
    public Enemy targetInfo;   // 타겟의 Enemy 스크립트
    public List<GameObject> EnemyList;   // 적 오브젝트
    public List<Enemy> EnemyInfoList;    // 적 Enemy 스크립트

    public MonsterStageData[] EnemySetData;
    private int randomSetNum;

    //private int map;
    public SkillData usingSkill;
    public MasterPieceData MP_Data;

    public int maxCanvasNum;       // 최대 캔버스 수 (최대 행동 수)
    private int canvasNum;       // 현재 캔버스 수 (현재 행동 수)

    public int turn;            // 현재 턴 수

    private int finalDamage;
    private int damage;
    private int shield;
    private int effect;
    private int heal;

    private int[] loot = new int[4];     // 1.골드 수, 2.스킬 선택지 수, 3.장신구ID, 4.걸작ID

    // 초기화
    void Awake()
    {
        instance = this;
        _SpawnManager = gameObject.GetComponent<SpawnManager>();

        // UI 비활성화
        _NextStageUI[0].SetActive(false);
        _NextStageUI[1].SetActive(false);
        _NextStageUI[2].SetActive(false);

        _LootManager.DeactivateAllUI();

        // 테스트 맵
        //map = 0;

        // 임시 : 캔버스 수 2개
        maxCanvasNum = 2;

        // 임시 : 첫 스테이지 적 스폰 ID
        enemyID[0] = 1;
        enemyID[3] = 2;

        damage = 0;
        shield = 0;
        effect = 0;
        heal = 0;

        _player.gold += 100;        // 초기 소지금

        SetNextStageUI();
    }

    private void Update() 
    {
        if (_player.health <= 0) {
            // 유물 : 패배 시 효과
            _ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.Defeat);

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
    public void AddColor(int paintType, bool isThemeSkill)
    {
        if (_PaletteManager.order == 0) {
            // 테마스킬이 아닌 기본 스킬의 경우 : usingSkill 설정
            if (isThemeSkill == false) {
                switch (paintType) {
                    // Red 타입 스킬
                    case 1:
                        usingSkill = _SkillManager.use_SkillData[0];
                        break;
                    // blue 타입 스킬
                    case 2:
                        usingSkill = _SkillManager.use_SkillData[1];
                        break;
                    // yellow 타입 스킬
                    case 3:
                        usingSkill = _SkillManager.use_SkillData[2];
                        break;
                    // white 타입 스킬
                    case 4:
                        usingSkill = _SkillManager.use_SkillData[3];
                        break;
                }
            }
            // 기본 데미지, 보호막, 효과, 회복 연산
            if (usingSkill.baseDamage > 0) {
                damage = usingSkill.baseDamage + _player.buffArr[0];     // 기본 데미지 + 집중 수치
            }
            shield = usingSkill.baseShield;
            effect = usingSkill.baseEffect;
            heal = usingSkill.baseHeal; 

            // 스킬 아이콘 변경
            _CanvasUI.GetComponent<CanvasScript>().ConvertSprite(usingSkill);
        }
        // 계수에 따른 추가 연산
        else if (_PaletteManager.order > 0) {
            damage += usingSkill.incDamage * _PaletteManager.order;
            shield += usingSkill.incShield * _PaletteManager.order;
            effect += usingSkill.incEffect * _PaletteManager.order;
            heal += usingSkill.incHeal * _PaletteManager.order;

            Debug.Log("현재 데미지" + damage + "\n현재 보호막" + shield + "\n현재 버프/디버프" + effect);
        }

        _PaletteManager.AddPaint(paintType);
        _PaletteManager.ConvertSprite(paintType);
    }

    // 페인트 초기화
    public void ClearColor()
    {
        _PaletteManager.ClearPaint();
        _PaletteManager.ClearPalette();
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
                enemyID[0] = EnemySetData[randomSetNum].monster01;
                enemyID[1] = EnemySetData[randomSetNum].monster02;
                enemyID[2] = EnemySetData[randomSetNum].monster03;
                enemyID[3] = EnemySetData[randomSetNum].monster04;
                
                BattleStart();
                break;
            // 상점
            case 6:
                // 상점NPC 스폰
                _SpawnManager.StoreNPCSpawn();
                break;
        }
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

    public void CheckVictory() {
        // 적 죽었으면 전투 종료
        if(EnemyList.Count == 0)
        {
            isLive = false;

            // 적 정보 리셋
            for (int i = 0; i < 4; i++)
            {
                enemyID[i] = 0;
            }

            state = State.win;
            EndBattle();
        }
    }

    void BattleStart()
    {
        state = State.start;    // 전투 시작 알림

        turn = 0;   // 턴 초기화

        // 캔버스 수 초기화
        canvasNum = maxCanvasNum;

        // 물감 최대치로 보충
        //for (int i = 0; i < 4; i++)
        //{
        //    _PaintScripts[i].FillUpPaint();
        //}

        // 스킬 리셋
        usingSkill = _SkillManager.noneData;

        isLive = true;
        Debug.Log(isLive);

        // 전투 시작 시 캐릭터 등장 애니메이션 등 효과 넣기

        // 적 스폰
        _SpawnManager.EnemySpawn(enemyID);

        // 적 정보 가져오는 코드
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemyInfoList.Add(EnemyList[i].GetComponent<Enemy>());
            loot[0] += EnemyInfoList[i].data.gold;      // 전리품_골드 세팅
            //loot[2]
        }

        // 전리품_스킬 세팅
        loot[1] = 3;

        // 유물 : 적 조우 시 효과
        _ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.Encounter);

        // 플레이어 턴 시작
        NextTurnStart();
    }

    // 턴 넘기기
    void NextTurnStart()
    {
        turn++;     // 턴 수 증가

        // 캔버스 수 초기화
        canvasNum = maxCanvasNum;

        // 플레이어 물감 회복
        for (int i = 0; i < 4; i++)
        {
            _PaintScripts[i].FillPaint();
        }

        // 보호막 초기화
        if (_player.buffArr[0] <= 0) {
            _player.shield = 0;
        }

        // 자동 타겟팅
        if (!target) {
            target = EnemyList[0];
            targetInfo = EnemyInfoList[0];
        }

        // 유물 : 플레이어의 턴 시작 시 효과   (유물의 Awake보다 일찍 발동되는 문제 발생)
        _ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.StartTurn);

        // 테마 스킬 _ 턴 시작 시 효과
        _ThemeManager.onTurnEffect = true;

        // 플레이어 중독 효과 연산
        _player.Poison();

        // 적 행동 확정
        for(int i = 0; i < EnemyList.Count; i++) {
            EnemyInfoList[i].TakeActInfo();
        }

        // 승리 시
        CheckVictory();

        // 플레이어 빙결/기절 효과  (플레이어 턴 스킵)
        if(_player.debuffArr[4] > 0 || _player.debuffArr[5] > 0)
        {
            if (_player.debuffArr[4] > 0) {
                _player.debuffArr[4] = 0;
                Debug.Log("빙결!");
            }

            if (_player.debuffArr[5] > 0) {
                _player.debuffArr[5] = 0;
                Debug.Log("기절!");
            }

            state = State.enemyTurn;
            StartCoroutine(EnemyTurn());
            Debug.Log("적의 턴입니다.");
        }
        // 플레이어의 턴 진행
        else {
            // 물감 기능 On
            for (int i = 0; i < 4; i++)
            {
                _PaintScripts[i].canUsePaint = true;
            }
            // 걸작 기능 On
            _MasterPieceManager.canUseMP = true;

            state = State.playerTurn;
            Debug.Log("플레이어의 턴입니다.");
        }
    }

    // 공격 버튼
    public void PlayerAttackBtn()
    {
        // 물감 선택X 시
        if (_PaletteManager.order == 0) {
            return;
        }

        // 버튼이 계속 눌리는 거 방지하기 위함
        if(state != State.playerTurn)
        {
            return;
        }
        state = State.playerAttack;

        // 물감 버튼 Off
        for (int i = 0; i < 4; i++)
        {
            _PaintScripts[i].usedColorNum = 0;      // 반환용 물감 초기화
            _PaintScripts[i].canUsePaint = false;
        }
        // 걸작 기능 Off
        _MasterPieceManager.canUseMP = false;

        _PaletteManager.stack += _PaletteManager.order;     // 사용한 물감 수만큼 스택 적립

        if (MP_Data.conditionType == MasterPieceData.ConditionType.Cost) {
            if (MP_Data.maximumCondition < _PaletteManager.stack) {
                _PaletteManager.stack = MP_Data.maximumCondition;
            }
        }
        else {
            if (MP_Data.cost < _PaletteManager.stack) {
                _PaletteManager.stack = MP_Data.cost;
            }
        }

        // 공격 단계로
        StartCoroutine(PlayerAttack());
    }

    public void PlayerEraseBtn()
    {
        for (int i = 0; i < 4; i++)
        {
            _PaintScripts[i].ReturnPaint();
        }
        damage = 0;
        shield = 0;
        effect = 0;

        _PaletteManager.ClearPaint();
        _PaletteManager.ClearPalette();
        _CanvasUI.GetComponent<CanvasScript>().ClearSprite();

        // 스킬 리셋
        usingSkill = _SkillManager.noneData;

        // 테마 스킬 리셋
        _ThemeManager.onThemeSkill = false;
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
            case SkillData.AttackType.Single:
                for (int i = 0; i < usingSkill.baseCount; i++) {    // 타수만큼 반복
                    if (usingSkill.baseDamage > 0) {                        // 기본 데미지가 0일 시 스킵
                        finalDamage = damage + targetInfo.debuffArr[0];     // 최종 데미지 = 기본 데미지 + 적 화상 수치
                        if (targetInfo.shield > 0) {    // 실드 존재 시
                            if (targetInfo.shield > finalDamage) {
                                targetInfo.shield -= finalDamage;
                                finalDamage = 0;
                            }
                            else {
                                finalDamage -= targetInfo.shield;
                            }
                        }
                        targetInfo.health -= finalDamage;   // 데미지 누적
                    }

                    // 적 디버프
                    if (usingSkill.effectType > 0) {
                        if (usingSkill.effectType < 20) {
                            targetInfo.debuffArr[usingSkill.effectType - 1] += effect;
                        }
                    }
                    Debug.Log(targetInfo.data.enemyName+"은(는) "+finalDamage+"의 데미지를 입었다.");
                }
                break;
            // 바운스
            case SkillData.AttackType.Bounce:
                List<Enemy> attackedEnemyList = EnemyInfoList.ToList();          // 적 생존 확인용 간이 리스트
                for (int i = 0; i < usingSkill.baseCount; i++) {    // 타수만큼 반복
                    int randomNum = UnityEngine.Random.Range(0, attackedEnemyList.Count);

                    // Hp없는 적에게 공격이 튀는 것을 방지
                    if (attackedEnemyList[randomNum].health < 0) {
                        attackedEnemyList.RemoveAt(randomNum);
                        i--;
                        if (attackedEnemyList.Count <= 0) {
                            break;
                        }
                        else {
                            continue;
                        }
                    }
                    
                    if (usingSkill.baseDamage > 0) {                                    // 기본 데미지가 0일 시 스킵
                        finalDamage = damage + EnemyInfoList[randomNum].debuffArr[0];   // 최종 데미지 = 기본 데미지 + 적 화상 수치
                        if (EnemyInfoList[randomNum].shield > 0) {      // 실드 존재 시
                            if (EnemyInfoList[randomNum].shield > finalDamage) {
                                EnemyInfoList[randomNum].shield -= finalDamage;
                                finalDamage = 0;
                            }
                            else {
                                finalDamage -= EnemyInfoList[randomNum].shield;
                            }
                        }
                        EnemyInfoList[randomNum].health -= finalDamage;     // 데미지 누적 
                    }

                    // 적 디버프
                    if (usingSkill.effectType > 0) {
                        if (usingSkill.effectType < 20) {
                            EnemyInfoList[randomNum].debuffArr[usingSkill.effectType - 1] += effect;
                        }
                    }
                    Debug.Log(EnemyInfoList[randomNum].data.enemyName+"은(는) "+finalDamage+"의 데미지를 입었다.");
                }
                break;
            // 전체 공격
            case SkillData.AttackType.Splash:
                for (int a = 0; a < usingSkill.baseCount; a++) {    // 타수만큼 반복
                    for(int i = 0; i < EnemyList.Count; i++) {
                        if (usingSkill.baseDamage > 0) {                                        // 기본 데미지가 0일 시 스킵
                            finalDamage = damage + EnemyInfoList[i].debuffArr[0];               // 최종 데미지 = 기본 데미지 + 적 화상 수치
                            if (EnemyInfoList[i].shield > 0) {
                                if (EnemyInfoList[i].shield > finalDamage) {
                                    EnemyInfoList[i].shield -= finalDamage;
                                    finalDamage = 0;
                                }
                                else {
                                    finalDamage -= EnemyInfoList[i].shield;
                                }
                            }
                            EnemyInfoList[i].health -= finalDamage;
                        }

                        // 적 디버프
                        if (usingSkill.effectType > 0) {
                            if (usingSkill.effectType < 20) {
                                EnemyInfoList[i].debuffArr[usingSkill.effectType - 1] += effect;
                            }
                        }
                        
                        Debug.Log(EnemyInfoList[i].data.enemyName+"은(는) "+finalDamage+"의 데미지를 입었다.");
                    }
                }
                break;
        }
        // 데미지 연산 : 기본 데미지 + 화상 데미지 + 집중 효과

        // 플레이어 버프
        if (usingSkill.effectType > 20) {
            _player.buffArr[usingSkill.effectType - 21] += effect;
        }

        // 플레이어 보호막
        _player.shield += shield;
        Debug.Log("플레이어는 "+shield+"의 보호막을 얻었다!");

        // 플레이어 회복
        _player.health += heal;
        if (_player.health > _player.maxHealth) {
            _player.health = _player.maxHealth;
        }
        Debug.Log("플레이어는 "+heal+"만큼 회복했다!");

        // 공격 확인
        if (usingSkill.baseDamage > 0) {
            // 유물 : 적중 시 효과
            _ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.OnHit);
        }

        // 테마 스킬_테마 스킬 사용 시
        if (_ThemeManager.onThemeSkill == true) {
            _ThemeManager.usedPaintNum = _PaletteManager.order;
            _ThemeManager.colorType_FirstSub = _PaletteManager.paints[1];
            _ThemeManager.useThemeSkill = true;
        }

        // 타겟팅 초기화 (마지막 캔버스일 시)
        if (canvasNum <= 0) {
            target = null;
        }

        // 합산 수치 초기화
        damage = 0;
        shield = 0;
        effect = 0;
        heal = 0;

        for(int i = 0; i < EnemyList.Count; i++) {
            // 감전 효과 연산
            EnemyInfoList[i].ElectricShock();
            // 추위 효과 연산
            EnemyInfoList[i].Coldness();
        }

        // 색 초기화
        ClearColor();

        // 스킬 리셋
        usingSkill = _SkillManager.noneData;

        yield return new WaitForSeconds(1f);

        // 캔버스 수 감소
        canvasNum--;

        // 적 죽었으면 전투 종료
        if(EnemyList.Count == 0)
        {
            isLive = false;

            // 적 정보 리셋
            for (int i = 0; i < 4; i++)
            {
                enemyID[i] = 0;
            }

            state = State.win;
            EndBattle();
        }
        else
        {
            // 캔버스 보유 시 재공격 기회 제공
            if (canvasNum > 0) {
                state = State.playerTurn;

                // 물감 기능 On
                for (int i = 0; i < 4; i++)
                {
                    _PaintScripts[i].canUsePaint = true;
                }
                // 걸작 기능 Off
                _MasterPieceManager.canUseMP = true;
            }
            // 적 살았으면 적에게 턴 넘기기
            else {
                // 플레이어 턴 종료 시 버프/디버프 감소
                _player.DecStatusEffect();

                state = State.enemyTurn;
                StartCoroutine(EnemyTurn());

                Debug.Log("적의 턴입니다.");
            }

        }
    }

    void EndBattle()
    {
        // 플레이어 상태 이상 초기화
        _player.ClearStatusEffect();

        // 물감 최대치로 보충
        for (int i = 0; i < 4; i++)
        {
            _PaintScripts[i].FillUpPaint();
        }

        // 걸작 스택 리셋
        _PaletteManager.stack = 0;

        // 유물 : 승리 시 효과
        _ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.Victory);

        /* 전리품_걸작 획득
        if (loot[3] > 0) {
            _LootManager.SetLootUI(4,loot[3]);
        }
        loot[3] = 0;*/

        Debug.Log("전투 종료");
        state = State.rest;

        // 전리품UI On
        _LootManager.gameObject.SetActive(true);

        // 전리품_골드 획득
        if (loot[0] > 0) {
            _LootManager.SetLootUI(1,loot[0]);
        }
        loot[0] = 0;

        // 전리품_스킬 획득
        if (loot[1] > 0) {
            _LootManager.SetLootUI(2,loot[1]);
        }
        loot[1] = 0;

        // 전리품_장신구 획득
        if (loot[2] > 0) {
            _LootManager.SetLootUI(3,loot[2]);
        }
        loot[2] = 0;
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        // 적 공격 코드
        for(int i = 0; i < EnemyList.Count; i++)
        {
            if (!EnemyList[i]) {
                continue;
            }

            // 적 보호막 초기화
            if (EnemyInfoList[i].buffArr[0] <= 0) {
                EnemyInfoList[i].shield = 0;
            }

            // 적 중독 효과
            EnemyInfoList[i].Poison();

            // 적 빙결/기절 상태 확인   (적 턴 스킵)
            if (EnemyInfoList[i].debuffArr[4] > 0 || EnemyInfoList[i].debuffArr[5] > 0) {
                if (EnemyInfoList[i].debuffArr[4] > 0) {
                    EnemyInfoList[i].debuffArr[4] = 0;
                    Debug.Log(EnemyInfoList[i].data.enemyName+" 빙결!");
                }
                if (EnemyInfoList[i].debuffArr[5] > 0) {
                    EnemyInfoList[i].debuffArr[5] = 0;
                    Debug.Log(EnemyInfoList[i].data.enemyName+" 기절!");
                }
                continue;
            }

            // 공격 연산 (데미지 + 집중 + 플레이어_화상)
            if (EnemyInfoList[i].skillDamage > 0) {
                damage = EnemyInfoList[i].skillDamage + EnemyInfoList[i].buffArr[1] + _player.debuffArr[0];

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
                _player.health -= damage;
            }

            // 보호막 연산
            EnemyInfoList[i].shield += EnemyInfoList[i].skillShield;

            // 회복 연산
            EnemyInfoList[i].health += EnemyInfoList[i].skillHeal;
            if (EnemyInfoList[i].health > EnemyInfoList[i].maxHealth) {
                EnemyInfoList[i].health = EnemyInfoList[i].maxHealth;
            }

            // 버프/디버프 효과 연산
            if (EnemyInfoList[i].effectType > 0) {
                // 디버프
                if (EnemyInfoList[i].effectType < 20) {
                    _player.debuffArr[EnemyInfoList[i].effectType - 1] += EnemyInfoList[i].effectNum;
                }
                // 버프
                else if (EnemyInfoList[i].effectType < 30) {
                    EnemyInfoList[i].buffArr[EnemyInfoList[i].effectType - 21] += EnemyInfoList[i].effectNum;
                }
            }

            
            Debug.Log(EnemyList[i].name+"의 공격! 플레이어에게"+damage+"의 데미지!");

            // 피격 확인
            if (EnemyInfoList[i].skillDamage > 0) {
                // 유물 : 피격 시 효과
                _ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.GetHit);
            }

            // 합산 수치 초기화
            damage = 0;
            shield = 0;
            effect = 0;
            heal = 0;

            // 플레이어 빙결 효과
            _player.Coldness();
            // 플레이어 감전 효과
            _player.ElectricShock();
        }

        for(int i = 0; i < EnemyList.Count; i++) {
            // 감전 효과 연산
            EnemyInfoList[i].ElectricShock();
            // 추위 효과 연산
            EnemyInfoList[i].Coldness();

        }

        yield return new WaitForSeconds(1f);

        // 승리 시
        if(EnemyList.Count == 0)
        {
            CheckVictory();
        }
        // 적 공격 끝났으면 플레이어에게 턴 넘기기
        else {
            // 적 턴 종료 시 버프/디버프 감소
            for(int i = 0; i < EnemyList.Count; i++)
            {
                if (!EnemyList[i]) {
                    continue;
                }
                EnemyInfoList[i].DecStatusEffect();
            }

            // 다음 턴 시작
            NextTurnStart();
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
        // 일반몹 스테이지 강제 (첫 스테이지)
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
        // 상점 스테이지 선택 (보스 전 상점)
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
    /* 최대 페인트 수
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
    }*/

    /*public void AddPaint(int num) {
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
    }*/
}