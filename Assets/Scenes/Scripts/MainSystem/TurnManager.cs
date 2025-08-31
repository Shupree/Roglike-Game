using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum StatusInfo
{
    health, maxHealth, shield
}

public interface IUnit
{
    List<StatusEffect> statusEffects { get; }

    void TakeTurn();
    bool IsDead();      // 사망여부 확인

    int GetStatus(StatusInfo status);   // 스테이터스 값 가져오기 (HP, maxHP, shield)
    int GetStatusEffectStack(StatusEffectData effectData); // 상태이상 데이터 가져오기 (Burn, Poison, Freezen ...)

    void TakeDamage(DamageInfo damageInfo, bool onBeingHit);     // 데미지를 받는 메서드
    void TakeHeal(int heal);                            // 회복 연산 메서드
    void TakeShield(int shield);                        // 실드 연산 메서드
    void AddStatusEffect(StatusEffectData effectData, int stack);     // 상태이상을 추가하는 메서드
    void DecStatusEffect(StatusEffectData effectData, int stack);     // 상태이상을 줄이는 메서드

    UnitSkillData GetSkillInfo();      // 아군, 적군이 사용할 스킬 정보 반환
}

public class TurnManager : MonoBehaviour
{
    public enum TurnState
    {
        rest, turnStart, playerAct, allyTurn, enemyTurn, victory, defeat
    }

    [Header("State")]
    private TurnState state;    // 현재 진행 상황

    [Header("Reference")]
    // 아군, 적 구분하여 List<ITurn> 생성
    public List<IUnit> allies = new List<IUnit>();
    public List<IUnit> enemies = new List<IUnit>();

    [HideInInspector]
    private Player player;                       // 플레이어
    private PaintManager paintManager;          // 페인트 매니저
    private StageManager stageManager;          // 스테이지 매니저
    private StorageManager storageManager;      // 스토리지 매니저

    private int canvas;     // 캔버스 수 (사용가능한 스킬 수)

    [Header("Figure")]
    private int totalTurns = 0;     // 경과한 턴 수

    public int usedPaintNum = 0;    // 지난 턴에 사용했던 물감 수

    public (int, bool) lootInfoTuple = (0, false);      // 전리품 정보 (골드 수, 스킬 ???, 수집품 ???, 걸작 ???)

    public void Initialize()
    {
        // 초기화
        state = TurnState.rest;

        paintManager = GameManager.instance.paintManager;
        stageManager = GameManager.instance.stageManager;
        storageManager = GameManager.instance.storageManager;
        player = GameManager.instance.player;

        gameObject.GetComponent<PlayerActionHandler>().Initialize();
    }

    // 아군 개체 추가
    public void RegisterAlly(IUnit ally)
    {
        allies.Add(ally);
        Debug.Log(ally + "아군 정보 등록 완료!");
    }

    // 적 개체 추가
    public void RegisterEnemy(IUnit enemy)
    {
        enemies.Add(enemy);
        Debug.Log(enemy + "적군 정보 등록 완료!");
    }

    // 전투 시작
    public void BattleStart()
    {
        state = TurnState.turnStart;

        lootInfoTuple.Item2 = true;     // 스킬 보상 활성화

        totalTurns = 0;                 // 턴 초기화
        paintManager.FillUpPaint();     // 물감 초기화

        // 전투 시작 시 애니메이션 추가

        BattleEventManager.TriggerBattleStart();

        StartTurns();   // 다음 턴 시작
    }

    // 턴 시작
    public void StartTurns()
    {
        if (state == TurnState.victory || state == TurnState.defeat) return;    // 전투 종료 시 함수 끊기
        totalTurns++;       // 턴 수 증가
        Debug.Log($"총 경과 턴: {totalTurns}");      // Log : 경과 턴

        canvas = player.canvas;     // 캔버스 수 초기화
                                    // 캔버스관련 디버프에 어떻게 대응할 것인가?

        // 보호막 초기화
        //if (_player.buffArr[0] <= 0) {
        //    _player.shield = 0;
        //}

        // 유물 : 플레이어의 턴 시작 시 효과   (유물의 Awake보다 일찍 발동되는 문제 발생)
        //_ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.StartTurn);

        // 테마 스킬 _ 턴 시작 시 효과
        //_ThemeManager.onTurnEffect = true;

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

        BattleEventManager.TriggerTurnStart();            // '턴 시작 시' 이벤트 시행

        // ----- 턴 시작 / 플레이어 턴 변경 시점 -----

        // 플레이어 턴 시작


        BattleEventManager.TriggerPlayerTurnStart();         // 플레이어 턴 시작 이벤트

        // 플레이어의 턴 시작 시 상태이상 로직 발동 (OnTurnStart)
        List<StatusEffect> effectsToProcess = new List<StatusEffect>(player.statusEffects);
        effectsToProcess.ForEach(effect => effect.logic.OnTurnStart(player, effect));

        // 플레이어 빙결 효과  (플레이어 턴 스킵)
        StatusEffectData frozenEffect = GameManager.instance.statusEffectManager.GetStatusEffectData("Frozen"); // 예시: 스토리지에서 데이터 가져오기
        if (player.GetStatusEffectStack(frozenEffect) > 0)
        {
            Debug.Log("플레이어 빙결!!");
            player.DecStatusEffect(frozenEffect, 1);

            state = TurnState.allyTurn;
            StartCoroutine(AllyTurn());
            Debug.Log("아군의 턴입니다.");
        }

        // 플레이어의 턴 진행
        else
        {
            paintManager.FillPaint();               // 플레이어 물감 보충

            // 물감 & 걸작스킬 & 테마스킬 기능 On
            paintManager.canUsePaint = true;

            state = TurnState.playerAct;        // 아군의 턴 시작 + 플레이어 행동
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
            state = TurnState.defeat;
            StopAllCoroutines();        // 모든 코루틴 종료

            paintManager.canUsePaint = false;               // 물감 & 걸작스킬 & 테마스킬 기능 Off

            Debug.Log("플레이어가 사망했습니다. 게임 오버!");
        }

        // 적군이 모두 제거되었는지 확인 (승리 조건)
        if (enemies.Count == 0)
        {
            state = TurnState.victory;      // 승리
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

    // 걸작 실행
    public IEnumerator ExecuteMasterPiece()
    {
        // 걸작 스킬 사용
        storageManager._MPManager.targets = player.targets;
        storageManager._MPManager.ExecuteMPSkill();

        player.targets.Clear();
        player.actionType = ActionType.none;

        yield return null;

        paintManager.canUsePaint = true;               // 물감 & 걸작스킬 & 테마스킬 기능 On

        // 플레이어에게 턴 제공
        state = TurnState.playerAct;
    }

    // 플레이어의 공격 진행
    public IEnumerator PlayerAttack()
    {
        // 테마 스킬 사용 시
        if (player.actionType == ActionType.themeSkill)
        {
            storageManager.themeManager.targets = player.targets;      // 타겟 넘기기
            storageManager.themeManager.CalculateSkillAbility(player.themeSkill);    // 테마 스킬 발동
        }
        // 일반 스킬 사용 시
        else
        {
            player.ExecutePaintSkill(paintManager.paletteOrder);      // 플레이어 스킬 발동
        }

        // 플레이어의 턴 종료 시 상태이상 로직 발동 (OnTurnEnd)
        // 리스트 복사본을 만들어 순회 중 리스트 변경으로 인한 오류 방지
        List<StatusEffect> playerEffects = new List<StatusEffect>(player.statusEffects);
        playerEffects.ForEach(effect => effect.logic.OnTurnEnd(player, effect));

        yield return null;

        usedPaintNum = paintManager.paletteOrder;
        storageManager._MPManager.AddStack(usedPaintNum);     // 사용한 물감 수만큼 스택 적립

        BattleEventManager.TriggerPlayerAttack();          // '플레이어 공격 시' 이벤트 시행

        paintManager.ClearPaint();      // 페인트 & 팔레트 초기화

        player.ClearActionInfo();       // 플레이어 스킬 정보 초기화

        BattleEventManager.TriggerPlayerTurnEnd();      // 플레이어 턴 종료 이벤트

        state = TurnState.allyTurn;
        StartCoroutine(AllyTurn());
    }

    // 아군의 턴 진행
    private IEnumerator AllyTurn()
    {
        yield return new WaitForSeconds(1f);

        // 아군 행동 실행
        yield return ExecuteTurn(allies);   // 아군 행동

        Debug.Log("적군의 턴입니다.");
        state = TurnState.enemyTurn;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        // 적 행동 진행
        yield return ExecuteTurn(enemies);  // 적 행동

        yield return null;  // 프레임 동기화

        state = TurnState.turnStart;
        StartTurns();       // 다음 턴 시작
    }

    // 아군 혹은 적의 턴 중 행동 진행
    private IEnumerator ExecuteTurn(List<IUnit> participants)
    {
        // 반복 중 리스트를 수정하면 문제가 발생하므로 복사본을 사용
        var currentParticipants = new List<IUnit>(participants);    // 플레이어 제외
        if (currentParticipants.Exists(n => n == allies[0]))
        {
            currentParticipants.Remove(currentParticipants.Find(n => n == allies[0]));
        }

        // 각 유닛의 행동 진행
        foreach (var participant in currentParticipants)
        {
            // 유닛의 턴 시작 시 상태이상 로직 발동 (OnTurnStart)
            // 리스트 복사본을 만들어 순회 중 리스트 변경으로 인한 오류 방지
            List<StatusEffect> effectsToProcess = new List<StatusEffect>(participant.statusEffects);
            effectsToProcess.ForEach(effect => effect.logic.OnTurnStart(participant, effect));

            // 행동 전 HP가 0인지 확인
            if (participant.IsDead())
            {
                Debug.Log($"{participant}는 이미 사망한 상태입니다.");
                continue;   // 다음 유닛으로 넘어감
            }

            // 빙결 시 턴 스킵
            StatusEffectData frozenEffect = GameManager.instance.statusEffectManager.GetStatusEffectData("Frozen");
            if (participant.GetStatusEffectStack(frozenEffect) > 0)
            {
                Debug.Log("빙결 상태로 인해 턴을 스킵합니다.");
                participant.DecStatusEffect(frozenEffect, 1);
                continue;
            }

            // 해당 유닛의 행동 수행
            participant.TakeTurn();

            // 유닛의 턴 종료 시 상태이상 로직 발동 (OnTurnEnd)
            // 리스트 복사본을 만들어 순회 중 리스트 변경으로 인한 오류 방지
            List<StatusEffect> effects = new List<StatusEffect>(participant.statusEffects);
            effects.ForEach(effect => effect.logic.OnTurnEnd(participant, effect));

            yield return null;
        }
    }

    public void RemoveDeadUnit(IUnit unit, string unitType)
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

    public TurnState GetState()
    {
        return state;
    }
}