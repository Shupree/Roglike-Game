using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ITurn
{
    void TakeTurn();
    int GetHP();        // 대상의 현재 체력 가져오기
    void TakeDamage(int damage);    // 데미지를 받는 메서드
    bool IsDead();      // 사망여부 확인

    public bool HasFreezeDebuff();      // 빙결 디버프 존재 확인
    public void RemoveFreezeDebuff();   // 빙결 디버프 제거
}

public class TurnManager : MonoBehaviour
{
    public enum State
    {
        rest, turnStart, playerAct, allyTurn, enemyTurn, win, defeat
    }

    [Header ("State")]
    private State state;    // 현재 진행 상황

    [Header ("Reference")]
    // 아군, 적 구분하여 List<ITurn> 생성
    public List<ITurn> allies = new List<ITurn>();
    public List<ITurn> enemies = new List<ITurn>();
    private Player player;       // 플레이어
    private PaintManager paintManager;  // 페인트 매니저

    private int canvas;     // 캔버스 수 (사용가능한 스킬 수)

    [Header ("Figure")]
    private int totalTurns = 0; // 경과한 턴 수
    
    // Delegate Event
    public delegate void BattleStartedEvent();
    public event BattleStartedEvent OnBattleStarted;    // 전투 시작 시 이벤트를 위한 델리게이트

    public delegate void TurnStartedEvent();
    public event TurnStartedEvent OnTurnStarted;        // 매턴 시작 시 이벤트를 위한 델리게이트

    public delegate void TurnCompletedEvent();
    public event TurnCompletedEvent OnTurnCompleted;    // 매턴 종료 시 이벤트를 위한 델리게이트

    public delegate void BattleEndEvent();
    public event BattleEndEvent OnBattleEnded;          // 전투 종료 이벤트

    public void Initialize()
    {   
        // 초기화
        state = State.rest;

        paintManager = GameManager.instance.paintManager;
        player = GameManager.instance.player;
    }

    // 아군 개체 추가
    public void RegisterAlly(ITurn ally)
    {
        allies.Add(ally);
        Debug.Log(ally+"아군 정보 등록 완료!");
    }

    // 적 개체 추가
    public void RegisterEnemy(ITurn enemy)
    {
        enemies.Add(enemy);
        Debug.Log(enemy+"적군 정보 등록 완료!");
    }

    // 전투 시작
    private void BattleStart()
    {
        state = State.turnStart;

        totalTurns = 0;     // 턴 초기화

        // 적 스폰
        // 적 정보 가져오기 (GameManager에서 추가할 것인가?)

        // 전투 시작 시 애니메이션 추가

        OnBattleStarted?.Invoke();        // '전투 시작 시' 이벤트 시행

        StartTurns();   // 다음 턴 시작
    }

    // 턴 시작
    public void StartTurns()
    {
        totalTurns++;       // 턴 수 증가
        Debug.Log($"총 경과 턴: {totalTurns}");      // Log : 경과 턴

        canvas = player.canvas;     // 캔버스 수 초기화
        // 캔버스관련 디버프에 어떻게 대응할 것인가?

        // 플레이어 물감 보충 (delegate event)

        // 보호막 초기화
        //if (_player.buffArr[0] <= 0) {
        //    _player.shield = 0;
        //}

        // 자동 타겟팅
        /*
        if (!target) {
            target = EnemyList[0];
            targetInfo = EnemyInfoList[0];
        }
        */

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

        // 플레이어 빙결/기절 효과  (플레이어 턴 스킵)
        if(player.HasFreezeDebuff())
        {
            Debug.Log("플레이어 빙결!!");
            player.RemoveFreezeDebuff();

            state = State.allyTurn;
            StartCoroutine(AllyTurn());
            Debug.Log("아군의 턴입니다.");
        }
        // 플레이어의 턴 진행
        else {
            // 물감 기능 On
            paintManager.canUsePaint = true;

            // 걸작 기능 On
            // _MasterPieceManager.canUseMP = true;

            OnTurnStarted?.Invoke();        // '새로운 턴 시작 시' 이벤트 시행

            state = State.playerAct;        // 아군의 턴 시작 + 플레이어 행동
            Debug.Log("플레이어가 행동할 차례입니다.");
        }

        // 플레이어가 행동 결정 시 (공격버튼 클릭 시) 다음 스탭으로
    }

    private bool CheckBattleEndConditions()
    {
        // 적군이 모두 제거되었는지 확인 (승리 조건)
        if (enemies.Count == 0)
        {
            EndBattle(); // 전투 종료
            Debug.Log("적군이 모두 제거되었습니다. 전투에서 승리했습니다!");
            return true;
        }

        // 플레이어가 사망했는지 확인 (패배 조건)
        if (player.IsDead())
        {
            Debug.Log("플레이어가 사망했습니다. 게임 오버!");
            return true;
        }

        return false; // 전투 종료 조건 미충족
    }

    private void EndBattle()
    {
        Debug.Log("전투가 종료되었습니다.");
        // 전투 종료 로직 추가

        OnBattleEnded?.Invoke();
    }

    // 지우기 버튼
    public void ClickEraseBtn()
    {
        for (int i = 0; i < 4; i++)
        {
            paintManager.ReturnPaint();
        }
        //damage = 0;
        //shield = 0;
        //effect = 0;

        paintManager.ClearPaint();
        //_CanvasUI.GetComponent<CanvasScript>().ClearSprite();

        // 스킬 리셋
        //usingSkill = _SkillManager.noneData;

        // 테마 스킬 리셋
        //_ThemeManager.onThemeSkill = false;
    }

    // 플레이어 공격 버튼 클릭 시
    public void ClickAttackBtn()
    {
        // 물감 선택X 시    (물감이 없어서 스킬을 사용 못할때에 예외 처리 필요)
        if (paintManager.paletteOrder == 0) {
            return;
        }

        // 버튼이 계속 눌리는 거 방지하기 위함
        if(state != State.playerAct)
        {
            return;
        }
        state = State.allyTurn;

        // 물감 버튼 Off
        paintManager.canUsePaint = false;

        for (int i = 0; i < 4; i++)
        {
            paintManager.usedPaintArr[i] = 0;       // 반환용 물감 초기화
        }
        // 걸작 기능 Off
        //_MasterPieceManager.canUseMP = false;

        paintManager.stack += paintManager.paletteOrder;     // 사용한 물감 수만큼 스택 적립

        /*
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
        */

        paintManager.ClearPaint();      // 페인트 & 팔레트 초기화

        Debug.Log("아군의 턴입니다.");
        // 공격 단계로
        StartCoroutine(AllyTurn());
    }

    // 아군의 턴 진행
    private IEnumerator AllyTurn()
    {
        yield return new WaitForSeconds(1f);

        // 아군 행동 실행
        yield return ExecuteTurn(allies);

        /*// 아군 턴 후 조건 확인
        if (CheckBattleEndConditions())
            yield break;

        // 적의 행동
        yield return ExecuteTurn(enemies);

        // 적군 턴 후 조건 확인
        if (CheckBattleEndConditions())
            yield break;*/

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

        // 턴 종료 후 작업
        OnTurnCompleted?.Invoke();        // '특정 턴 종료 시' 이벤트 시행

        state = State.turnStart;
        StartTurns();       // 다음 턴 시작
    }

    // 아군 혹은 적의 턴 중 행동 진행
    private IEnumerator ExecuteTurn(List<ITurn> participants)
    {
        // 반복 중 리스트를 수정하면 문제가 발생하므로 복사본을 사용
        var currentParticipants = new List<ITurn>(participants);

        // 각 유닛의 행동 진행
        foreach (var participant in currentParticipants)
        {
            /* 행동 전 HP가 0인지 확인
            if (participant.IsDead())
            {
                Debug.Log($"{participant}는 이미 사망한 상태입니다.");
                participants.Remove(participant);   // 리스트에서 유닛 제거
                continue;   // 다음 유닛으로 넘어감
            }*/

            // 빙결 시 턴 스킵
            if (participant.HasFreezeDebuff())
            {
                Debug.Log("빙결 상태로 인해 턴을 스킵합니다.");
                participant.RemoveFreezeDebuff();
                continue;
            }

            // 해당 유닛의 행동 수행
            participant.TakeTurn();

            /* 행동 후 HP가 0인지 확인
            if (participant.IsDead())
            {
                Debug.Log($"{participant}가 행동 중에 사망했습니다.");
                participants.Remove(participant);   // 리스트에서 유닛 제거
                continue;   // 리스트에서 제거되었으므로 다음 유닛으로 넘어감
            }*/

            /* 상대 팀 유닛의 사망 여부 확인
            var opposingTeamCopy = new List<ITurn>(opposingTeam);
            foreach (var enemy in opposingTeamCopy)
            {
                if (enemy.IsDead())
                {
                    Debug.Log($"{enemy}가 처치되었습니다.");
                    opposingTeam.Remove(enemy);     // 리스트에서 유닛 제거
                }
            }*/

            yield return null;
        }
    }

    private void RemoveDeadUnits(List<ITurn> units)
    {
        // 유닛이 처치된 경우
        units.RemoveAll(unit =>
        {
            if (unit.IsDead())
            {
                Debug.Log($"{unit}이 처치되었습니다.");
                return true;
            }
            return false;
        });
    }
}
