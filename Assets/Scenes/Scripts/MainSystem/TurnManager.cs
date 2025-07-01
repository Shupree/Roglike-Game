using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface ITurn
{
    void TakeTurn();
    bool IsDead();      // 사망여부 확인

    int GetStatus(string status);   // 스테이터스 값 가져오기 (HP, MaxHP, Shield)
    int GetStatusEffect(string effectName); // 상태이상 값 가져오기 (Burn, Poison, Freezen ...)
    void TakeDamage(int damage, bool isTrueDamage);     // 데미지를 받는 메서드
    void TakeHeal(int heal);                            // 회복 연산 메서드
    void TakeShield(int shield);                        // 실드 연산 메서드
    void AddStatusEffect(string effectName, int stack);     // 상태이상을 추가하는 메서드
    void DecStatusEffect(string effectName, int stack);     // 상태이상을 줄이는 메서드
    void CheckStatusEffectDuration();                       // 턴제 지속형 상태이상 확인

    //public bool HasFreezeDebuff();      // 빙결 디버프 존재 확인
    //public void RemoveFreezeDebuff();   // 빙결 디버프 제거

    //public int HasBurnDebuff();         // 화상 디버프 수치 확인

    UnitSkillData GetSkillInfo();      // 아군, 적군이 사용할 스킬 정보 반환
}

public class TurnManager : MonoBehaviour
{
    public enum State
    {
        rest, turnStart, playerAct, allyTurn, enemyTurn, useMP, victory, defeat
    }

    [Header("State")]
    private State state;    // 현재 진행 상황

    [Header("Reference")]
    // 아군, 적 구분하여 List<ITurn> 생성
    public List<ITurn> allies = new List<ITurn>();
    public List<ITurn> enemies = new List<ITurn>();
    [HideInInspector]
    public Player player;       // 플레이어
    private PaintManager paintManager;  // 페인트 매니저
    private StageManager stageManager;  // 스테이지 매니저
    private StorageManager storageManager;      // 스토리지 매니저

    private int canvas;     // 캔버스 수 (사용가능한 스킬 수)

    [Header("Target")]
    public List<ITurn> targets = new List<ITurn>();

    [Header("Figure")]
    private int totalTurns = 0;     // 경과한 턴 수

    public int usedPaintNum = 0;    // 지난 턴에 사용했던 물감 수

    public (int, bool) lootInfoTuple = (0, false);      // 전리품 정보 (골드 수, 스킬 ???, 수집품 ???, 걸작 ???)

    // Delegate Event
    public delegate void BattleEvent(string situation);
    public static event BattleEvent OnBattleEvent;          // 전투 시 이벤트를 위한 델리게이트

    /*
    public delegate void StartTurnEvent();
    public static event StartTurnEvent OnStartTurn;        // 매턴 시작 시 이벤트를 위한 델리게이트

    public delegate void AttackEvent();
    public static event AttackEvent OnAttack;    // 플레이어 공격 시 이벤트를 위한 델리게이트

    public delegate void HitEvent();
    public static event HitEvent OnHit;          // 플레이어 피격 시 이벤트를 위한 델리게이트
    */

    public void Initialize()
    {
        // 초기화
        state = State.rest;

        paintManager = GameManager.instance.paintManager;
        stageManager = GameManager.instance.stageManager;
        storageManager = GameManager.instance.storageManager;
        player = GameManager.instance.player;
    }

    // 아군 개체 추가
    public void RegisterAlly(ITurn ally)
    {
        allies.Add(ally);
        Debug.Log(ally + "아군 정보 등록 완료!");
    }

    // 적 개체 추가
    public void RegisterEnemy(ITurn enemy)
    {
        enemies.Add(enemy);
        Debug.Log(enemy + "적군 정보 등록 완료!");
    }

    // 델리게이트 이벤트 실행 함수
    public void OperateEvent(string eventName)
    {
        OnBattleEvent(eventName);
    }

    // 전투 시작
    public void BattleStart()
    {
        state = State.turnStart;

        lootInfoTuple.Item2 = true;     // 스킬 보상 활성화

        totalTurns = 0;                 // 턴 초기화
        paintManager.FillUpPaint();     // 물감 초기화

        // 전투 시작 시 애니메이션 추가

        StartTurns();   // 다음 턴 시작
    }

    // 턴 시작
    public void StartTurns()
    {
        if (state == State.victory || state == State.defeat) return;    // 전투 종료 시 함수 끊기
        totalTurns++;       // 턴 수 증가
        Debug.Log($"총 경과 턴: {totalTurns}");      // Log : 경과 턴

        canvas = player.canvas;     // 캔버스 수 초기화
                                    // 캔버스관련 디버프에 어떻게 대응할 것인가?

        // 플레이어 물감 보충 (delegate event)

        // 보호막 초기화
        //if (_player.buffArr[0] <= 0) {
        //    _player.shield = 0;
        //}

        // 유물 : 플레이어의 턴 시작 시 효과   (유물의 Awake보다 일찍 발동되는 문제 발생)
        //_ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.StartTurn);

        // 테마 스킬 _ 턴 시작 시 효과
        //_ThemeManager.onTurnEffect = true;

        // 플레이어 중독 효과 연산
        //_player.Poison();

        // 적 행동 확정
        //for(int i = 0; i < EnemyList.Count; i++) {
        //    EnemyInfoList[i].TakeSkillInfo();
        //}

        // 승리 시
        //CheckVictory();

        foreach (var enemy in enemies)
        {
            enemy.GetSkillInfo();
        }
        foreach (var ally in allies)
        {
            if (ally == allies[0])
            {
                continue;
            }
            ally.GetSkillInfo();
        }

        // 플레이어 턴 잡기
        Debug.Log("아군의 턴입니다.");
        player.TakeTurn();


        // 플레이어 빙결/기절 효과  (플레이어 턴 스킵)
        if (player.GetStatusEffect("Freezen") > 0)
        {
            Debug.Log("플레이어 빙결!!");
            player.DecStatusEffect("Freezen", 1);

            state = State.allyTurn;
            StartCoroutine(AllyTurn());
            Debug.Log("아군의 턴입니다.");
        }

        // 플레이어의 턴 진행
        else
        {
            paintManager.FillPaint();               // 플레이어 물감 보충

            // 물감 & 걸작스킬 & 테마스킬 기능 On
            paintManager.canUsePaint = true;

            OperateEvent("OnStartTurn");            // '턴 시작 시' 이벤트 시행

            state = State.playerAct;        // 아군의 턴 시작 + 플레이어 행동
            Debug.Log("플레이어가 행동할 차례입니다.");
        }

        // 플레이어가 행동 결정 시 (공격버튼 클릭 시) 다음 스탭으로
    }

    // 승리 혹은 패배 조건 확인
    public void CheckBattleEndConditions()
    {
        // 플레이어가 사망했는지 확인 (패배 조건)
        if (player.IsDead())
        {
            state = State.defeat;
            StopAllCoroutines();        // 모든 코루틴 종료

            paintManager.canUsePaint = false;               // 물감 & 걸작스킬 & 테마스킬 기능 Off
            
            Debug.Log("플레이어가 사망했습니다. 게임 오버!");
        }

        // 적군이 모두 제거되었는지 확인 (승리 조건)
        if (enemies.Count == 0)
        {
            state = State.victory;      // 승리
            StopAllCoroutines();        // 모든 코루틴 종료

            paintManager.canUsePaint = false;               // // 물감 & 걸작스킬 & 테마스킬 기능 Off

            paintManager.ClearPaint();          // 팔레트 초기화
            paintManager.FillUpPaint();         // 물감 초기화
            storageManager._MPManager.ClearStack();     // 걸작 스택 초기화

            EndBattle(); // 전투 종료
            Debug.Log("적군이 모두 제거되었습니다.");
        }
    }

    // 전투 종료
    private void EndBattle()
    {
        Debug.Log("전투에서 승리했습니다.");

        // 전리품(loot) 설정
        stageManager.SetLoot(lootInfoTuple);
        lootInfoTuple = (0, false);     // 전리품 정보 초기화
    }

    // 지우기 버튼
    public void ClickEraseBtn()
    {
        targets.Clear();

        for (int i = 0; i < 4; i++)
        {
            paintManager.ReturnPaint();
        }

        paintManager.ClearPaint();

        // 테마 스킬 리셋
        //_ThemeManager.onThemeSkill = false;
    }

    // 플레이어 공격 버튼 클릭 시
    public void ClickAttackBtn()
    {
        if (state == State.playerAct)
        {
            // 물감 선택X 시    (물감이 없어서 스킬을 사용 못할때에 예외 처리 필요)
            if (paintManager.paletteOrder == 0)
            {
                return;
            }

            state = State.allyTurn;

            // 물감 & 걸작스킬 & 테마스킬 기능 Off
            paintManager.canUsePaint = false;

            // 공격 단계로
            StartCoroutine(PlayerTurn());
        }
        else if (state == State.useMP)
        {
            state = State.allyTurn;

            // 걸작 스킬 실행
            StartCoroutine(ExecuteMasterPiece());
        }
    }

    // 걸작 사용 버튼
    public void ClickMasterPieceBtn()
    {
        // 버튼이 계속 눌리는 거 방지하기 위함
        if (state != State.playerAct || !storageManager._MPManager.CheckCondition())
        {
            return;
        }
        state = State.useMP;

        ClickEraseBtn();        // 사용 중이던 스킬 해제

        paintManager.canUsePaint = false;               // 물감 & 걸작스킬 & 테마스킬 기능 On

        paintManager.SetSkillImg(storageManager._MPManager.GetMPData().icon);
        SetTarget(storageManager._MPManager.GetMPData().skillType);  // 타겟팅 설정
    }

    // 걸작 실행
    private IEnumerator ExecuteMasterPiece()
    {
        // 걸작 스킬 사용
        storageManager._MPManager.targets = targets;
        storageManager._MPManager.ExecuteMPSkill();

        targets.Clear();

        yield return null;

        paintManager.canUsePaint = true;               // 물감 & 걸작스킬 & 테마스킬 기능 On

        // 플레이어에게 턴 제공
        state = State.playerAct;
    }

    // 플레이어의 공격 진행
    private IEnumerator PlayerTurn()
    {
        // 플레이어가 바운스 공격 시 타겟팅 설정
        if (player.mainSkill.skillType == Skill.SkillType.BounceAtk)
        {
            // 랜덤 적 타겟팅
            targets.Clear();
            for (int i = 0; i < player.mainSkill.count; i++)
            {
                int randomNum = Random.Range(0, enemies.Count);
                targets.Add(enemies[randomNum]);
            }
        }
        player.targets = targets;     // 플레이어에게 타겟 넘기기

        player.ExecuteSkill();      // 플레이어 스킬 발동

        yield return null;

        for (int i = 0; i < 4; i++)
        {
            paintManager.usedPaintArr[i] = 0;       // 반환용 물감 초기화
        }
        
        usedPaintNum = paintManager.paletteOrder;
        storageManager._MPManager.AddStack(usedPaintNum);     // 사용한 물감 수만큼 스택 적립

        OperateEvent("OnAttack");          // '플레이어 공격 시' 이벤트 시행

        paintManager.ClearPaint();      // 페인트 & 팔레트 초기화

        targets.Clear();                // 타겟팅 초기화

        StartCoroutine(AllyTurn());
    }

    // 아군의 턴 진행
    private IEnumerator AllyTurn()
    {
        yield return new WaitForSeconds(1f);

        // 아군 행동 실행
        yield return ExecuteTurn(allies);   // 플레이어 및 아군

        // 적의 행동
        yield return ExecuteTurn(enemies);

        Debug.Log("적군의 턴입니다.");
        state = State.enemyTurn;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        // 적 행동 진행
        ExecuteTurn(enemies);

        yield return null;  // 프레임 동기화

        state = State.turnStart;
        StartTurns();       // 다음 턴 시작
    }

    // 아군 혹은 적의 턴 중 행동 진행
    private IEnumerator ExecuteTurn(List<ITurn> participants)
    {
        // 반복 중 리스트를 수정하면 문제가 발생하므로 복사본을 사용
        var currentParticipants = new List<ITurn>(participants);    // 플레이어 제외
        if (currentParticipants.Exists(n => n == allies[0]))
        {
            currentParticipants.Remove(currentParticipants.Find(n => n == allies[0]));
        }

        // 각 유닛의 행동 진행
        foreach (var participant in currentParticipants)
        {
            // 행동 전 HP가 0인지 확인
            if (participant.IsDead())
            {
                Debug.Log($"{participant}는 이미 사망한 상태입니다.");
                continue;   // 다음 유닛으로 넘어감
            }

            // 빙결 시 턴 스킵
            if (participant.GetStatusEffect("Freezen") > 0)
            {
                Debug.Log("빙결 상태로 인해 턴을 스킵합니다.");
                participant.DecStatusEffect("Freezen", 1);
                continue;
            }

            // 해당 유닛의 행동 수행
            participant.TakeTurn();

            yield return null;
        }
    }

    public void RemoveDeadUnit(ITurn unit, string unitType)
    {
        // 적 유닛이 처치된 경우
        if (unitType == "Enemy")
        {
            enemies.Remove(unit);
            Debug.Log($"{unit}이 처치되었습니다.");
            CheckBattleEndConditions();     // 플레이어 승리 확인
        }
        // 아군 유닛이 처치된 경우
        else if (unitType == "Ally")
        {
            allies.Remove(unit);
            Debug.Log($"{unit}이 처치되었습니다.");
        }
        else
        {
            Debug.LogError("알 수 없는 유닛입니다!");
        }
    }

    // 기본적인 타겟팅 설정
    public void SetTarget(Skill.SkillType skillType)
    {
        switch (skillType)
        {
            case Skill.SkillType.SingleAtk:
                targets.Add(enemies[0]);
                break;
            case Skill.SkillType.SplashAtk:
                targets = new List<ITurn>(enemies);
                break;
            case Skill.SkillType.BounceAtk:
                targets = new List<ITurn>(enemies);
                break;
            case Skill.SkillType.SingleSup:
                targets.Add(player);
                break;
            case Skill.SkillType.SplashSup:
                targets = new List<ITurn>(allies);
                break;
        }
    }

    public State GetState()
    {
        return state;
    }
}