using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ActionType
{
    none, paintSkill, themeSkill, masterPiece
}

public class Player : MonoBehaviour, IUnit
{
    [Header("Reference")]
    //private PaintManager paintManager;
    [HideInInspector] public StorageManager storageManager;
    private TurnManager turnManager;

    [Header("HUD")]
    private HUD hud;     // HUD
    private StatusEffectUI statusEffectUI;  // 상태이상 UI

    public int gold;   // 골드 수

    [Header("Status")]
    private int maxHealth = 50;
    public int health = 50;     // 체력
    private int shield;         // 보호막

    public int ATKPoint;        // 공격력

    public int canvas;

    [Header("Skill")]
    // private CsvSkillLoader skillLoader;
    // public Skill[] skillArr = new Skill[4];     // 기본 스킬 4종 (빨강, 노랑, 파랑, 하양)
    public PaintSkillData currentSkill;        // 현재 사용하는 스킬
    public ThemeSkillData themeSkill;   // 현재 사용하는 테마 스킬
    public ActionType actionType = ActionType.none;

    [Header("Target")]
    public List<IUnit> targets = new List<IUnit>();    // 공격 타겟
    // 스플래쉬 공격에 경우 모든 적에게 타겟팅해야함.
    // 버프 스킬의 경우 아군이 타겟팅되어야함.

    // 상태이상 List
    public List<StatusEffect> statusEffects { get; private set; } = new List<StatusEffect>();

    // 초기화
    void Awake()
    {
        //maxHealth = 50;
        health = maxHealth;    // 임의 HP 설정값
        if (hud != null)
        {
            hud.UpdateHUD();
        }
        if (statusEffectUI != null)
        {
            statusEffectUI.UpdateStatusEffect(statusEffects);
        }

        turnManager = GameManager.instance.turnManager;
        storageManager = GameManager.instance.storageManager;   // 스크립트 가져오기
        storageManager.Initialize();    // storageManager 강제 초기화

        actionType = ActionType.none;
        currentSkill = null;
    }

    public void SetHUD(GameObject hudObject)
    {
        hud = hudObject.GetComponent<HUD>();                // HUD 가져오기
        statusEffectUI = hudObject.transform.GetChild(1).GetChild(0).GetComponent<StatusEffectUI>();    // 상태이상 UI 가져오기
        hud.UpdateHUD();                                    // HUD 정보 업데이트
        statusEffectUI.UpdateStatusEffect(statusEffects);   // 상태이상 정보 업데이트
    }

    // 사용 X (Enemy.cs, Ally.cs용)
    public void TakeTurn()
    {

    }

    // 특정 스테이터스 값 확인
    public int GetStatus(StatusInfo status)
    {
        int value = 0;

        switch (status)
        {
            // HP 값 반환
            case StatusInfo.health:
                value = health;
                break;
            // MaxHP 값 반환
            case StatusInfo.maxHealth:
                value = maxHealth;
                break;
            case StatusInfo.shield:
                value = shield;
                break;
        }

        return value;   // 값 반환
    }

    // 상태이상 값 확인
    public int GetStatusEffectStack(StatusEffectData effectData)
    {
        if (statusEffects.Exists(e => e.data == effectData))
        {      // 버프/디버프 존재 시
            return statusEffects.Find(e => e.data == effectData).stackCount;
        }
        else
        {          // 아닐 시 0으로 반환
            return 0;
        }
    }

    // 타겟팅 설정
    public void SetTarget(PaintSkillData.SkillType skillType, int count)
    {
        switch (skillType)
        {
            case PaintSkillData.SkillType.SingleAtk:
                targets.Add(turnManager.enemies[0]);
                break;
            case PaintSkillData.SkillType.SplashAtk:
                targets = new List<IUnit>(turnManager.enemies);
                break;
            case PaintSkillData.SkillType.BounceAtk:
                // 랜덤 적 타겟팅
                targets.Clear();
                for (int i = 0; i < count; i++)
                {
                    int randomNum = Random.Range(0, turnManager.enemies.Count);
                    targets.Add(turnManager.enemies[randomNum]);
                }
                break;
            case PaintSkillData.SkillType.SingleSup:
                targets.Add(this);
                break;
            case PaintSkillData.SkillType.SplashSup:
                targets = new List<IUnit>(turnManager.allies);
                break;
        }
    }

    // 현재 행동 정보 초기화
    public void ClearActionInfo()
    {
        targets.Clear();
        actionType = ActionType.none;
        currentSkill = null;
        themeSkill = null;
    }

    // 스킬 발동 로직
    public void ExecutePaintSkill(int subPaint)
    {
        DamageInfo damageInfo = new DamageInfo { amount = 0, isIgnoreShield = false };  // 데미지 정보 저장
        StatusEffectInfo statusEffectInfo = new StatusEffectInfo { effectDatas = currentSkill.effectDatas, effects = new List<int>() };
        int count = 0;              // 스킬 타수

        // 공격 type에 따른 분류    (targets는 turnManager로부터 받음)
        switch (currentSkill.skillType)
        {
            // 단타 공격
            case PaintSkillData.SkillType.SingleAtk:
                count = currentSkill.count;
                break;
            // 전체 공격
            case PaintSkillData.SkillType.SplashAtk:
                count = currentSkill.count;
                break;
            // 바운스 공격
            case PaintSkillData.SkillType.BounceAtk:
                count = 1;
                break;
            // 자신 보조
            case PaintSkillData.SkillType.SingleSup:    // 자기자신 타겟 스킬
                count = currentSkill.count;
                damageInfo.isIgnoreShield = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case PaintSkillData.SkillType.SplashSup:
                count = currentSkill.count;
                damageInfo.isIgnoreShield = true;    // 아군 대상은 고정데미지
                break;
        }

        // 일반스킬의 기본 스탯 연산
        damageInfo.amount = currentSkill.damage + (currentSkill.perDamage * subPaint);  // 데미지 연산
        for (int i = 0; i < currentSkill.perEffect.Count; i++)                          // 상태이상 연산
        {
            statusEffectInfo.effects.Add(currentSkill.effects[i] + (currentSkill.perEffect[i] * subPaint));
        }

        // 스킬 스탯 집합
        ActionInfo actionInfo = new ActionInfo
        {
            damageInfo = damageInfo,
            heal = currentSkill.heal + (currentSkill.perHeal * subPaint),
            shield = currentSkill.shield + (currentSkill.perShield * subPaint),
            statusEffectInfo = statusEffectInfo
        };

        // 공격 / 상태이상 부여
        for (int i = 0; i < count; i++)   // 타수만큼 반복
        {
            foreach (var target in targets)     // 모든 타겟 공격
            {
                BattleLogic.ActionLogic(this, target, actionInfo);
            }
        }

        targets.Clear();

        actionType = ActionType.none;
        currentSkill = null;
    }

    // 피격 시 데미지 연산
    public void TakeDamage(DamageInfo damageInfo, bool onBeingHit)
    {
        if (onBeingHit)
        {
            // OnBeingHit 로직 호출 (데미지 계산 전)
            // 리스트 복사본을 만들어 순회 중 리스트 변경으로 인한 오류 방지
            List<StatusEffect> effectsToProcess = new List<StatusEffect>(statusEffects);
            effectsToProcess.ForEach(effect => effect.logic.OnBeingHit(this, effect, ref damageInfo));
        }

        // 실제 데미지 적용
        if (damageInfo.isIgnoreShield)
        {
            health -= damageInfo.amount;
            if (health < 0)
            {
                health = 0;
                GameManager.instance.turnManager.CheckBattleEndConditions();
            }
        }
        else
        {
            if (shield > damageInfo.amount)
            {
                shield -= damageInfo.amount;
            }
            else
            {
                damageInfo.amount -= shield;
                shield = 0;
                health -= damageInfo.amount;
                if (health < 0) health = 0;
            }
        }

        hud.UpdateHUD();        // HUD의 HP 변화
        Debug.Log($"플레이어가 {damageInfo.amount} 데미지를 받았습니다! 남은 체력: {health}");

        BattleEventManager.TriggerUnitDamaged(this, damageInfo.amount);      // '플레이어 피격 시' 이벤트 시행
    }

    // 회복 시 연산
    public void TakeHeal(int num)
    {
        if (maxHealth <= health + num)
        {
            health = maxHealth;
        }
        else
        {
            health += num;
        }
        hud.UpdateHUD();        // HUD의 HP 변화
    }

    // 보호막 획득 시 연산
    public void TakeShield(int num)
    {
        shield += num;
    }

    // 처치 확인
    public bool IsDead()
    {
        return health <= 0;
    }

    // Unit별 AI 코드. 사용 X
    public UnitSkillData GetSkillInfo()
    {
        Debug.LogError("사용하지 않는 코드입니다.");
        return null;
    }

    // ------ 버프 / 디버프 로직 ------

    // 특정 상태이상 추가 ( StatusEffect 상태이상 종류, int 중첩 수 )
    public void AddStatusEffect(StatusEffectData effectData, int stack)
    {
        // 이미 존재하는 상태이상인지 확인
        var statusEffect = statusEffects.Find(e => e.data == effectData);

        // 이미 존재 시,
        if (statusEffect != null)
        {
            statusEffect.stackCount = Mathf.Min(
                statusEffect.data.maxStack,
                statusEffect.stackCount + stack
                );
            Debug.Log($"플레이어의 {statusEffect.data.effectName} 상태 이상이 {statusEffect.stackCount}로 중첩되었습니다.");
        }
        // 존재하지 않을 시, 새로운 상태이상 추가
        else
        {
            StatusEffect newEffectInstance = new StatusEffect(effectData);
            newEffectInstance.stackCount = stack;
            statusEffects.Add(newEffectInstance);

            Debug.Log($"{statusEffect.data.effectName}이(가) 새로 추가되었습니다.");
        }

        statusEffectUI.UpdateStatusEffect(statusEffects);    // 상태이상UI 업데이트
    }

    // 버프/디버프 감소
    public void DecStatusEffect(StatusEffectData effectData, int stack)
    {
        var statusEffect = statusEffects.Find(e => e.data == effectData);
        if (statusEffect != null)
        {
            if (statusEffect.stackCount > stack)
            {
                statusEffect.stackCount -= stack;
            }
            else
            {
                statusEffects.Remove(statusEffect);     // 상태이상 제거
            }
        }

        statusEffectUI.UpdateStatusEffect(statusEffects);    // 상태이상UI 업데이트
    }
}
