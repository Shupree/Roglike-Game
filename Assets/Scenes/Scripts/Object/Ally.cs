using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ally : MonoBehaviour, IUnit
{
    [Header("Ally Data")]
    public AllyData data;
    private UnitSkillData curSkillData;     // 현재 사용할 스킬 data

    [Header("HUD")]
    private HUD hud;     // HUD
    private StatusEffectUI statusEffectUI;  // 상태이상 UI

    [Header("Status")]
    private int health;     // 체력
    private int maxHealth;  // 최대 체력
    private int shield;     // 보호막

    private bool isDead = false;

    [Header("Target")]
    public List<IUnit> targets = new List<IUnit>();    // 공격 타겟

    // 상태이상 List
    public List<StatusEffect> statusEffects { get; private set; } = new List<StatusEffect>();

    [Header("Individual Turn")]
    int currentTurn;

    void Awake()
    {
        maxHealth = data.maxHealth;
        health = maxHealth;    // 임의 설정값
    }

    /*
    public Ally(int initialHealth)
    {
        health = initialHealth;
    }
    */

    public void SetHUD(GameObject hudObject)
    {
        hud = hudObject.GetComponent<HUD>();                // HUD 가져오기
        statusEffectUI = hudObject.transform.GetChild(1).GetChild(0).GetComponent<StatusEffectUI>();    // 상태이상 UI 가져오기
        hud.UpdateHUD();                                    // HUD 정보 업데이트
        statusEffectUI.UpdateStatusEffect(statusEffects);   // 상태이상 정보 업데이트
    }

    public void TakeTurn()
    {
        Debug.Log($"아군 {name}이(가) 행동합니다!");
        // 아군 행동 로직

        // 데미지 정보 생성 (상태이상 로직용)
        DamageInfo damageInfo = new DamageInfo { amount = curSkillData.damage, isIgnoreShield = false };
        StatusEffectInfo statusEffectInfo = new StatusEffectInfo { effectDatas = curSkillData.effectDatas, effects = curSkillData.effects };


        // 공격 type에 따른 분류
        switch (curSkillData.skillType)
        {
            // 단타 공격
            case UnitSkillData.SkillType.SingleAtk:
                // 랜덤 적 타겟팅
                int randomValue = Random.Range(0, GameManager.instance.turnManager.enemies.Count);      // 랜덤 값 산출
                targets.Add(GameManager.instance.turnManager.enemies[randomValue]);
                break;

            // 전체 공격
            case UnitSkillData.SkillType.SplashAtk:
                targets = new List<IUnit> (GameManager.instance.turnManager.enemies);
                break;
            // 자신 보조
            case UnitSkillData.SkillType.SingleSup:    // 자기자신 타겟 스킬
                targets.Add(this);
                damageInfo.isIgnoreShield = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case UnitSkillData.SkillType.SplashSup:
                targets = new List<IUnit> (GameManager.instance.turnManager.allies);
                damageInfo.isIgnoreShield = true;    // 아군 대상은 고정데미지
                break;
        }

        // 스킬 스탯 집합
        ActionInfo actionInfo = new ActionInfo {
            damageInfo = damageInfo,
            heal = curSkillData.heal,
            shield = curSkillData.shield,
            statusEffectInfo = statusEffectInfo
        };

        for (int i = 0; i < curSkillData.count; i++)   // 타수만큼 반복
        {
            // 공격 / 상태이상 부여
            foreach (var target in targets)     // 모든 타겟 공격
            {
                BattleLogic.ActionLogic(this, target, actionInfo);
            }
        }

        targets.Clear();
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

        if (damageInfo.isIgnoreShield)
        {
            health -= damageInfo.amount;
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
            }
        }

        if (health <= 0 && isDead == false)
        {
            isDead = true;
            health = 0;     // 음수값 제외
            //GameManager.instance.hudPoolManager.ReturnHUD(hud.gameObject);      // HUD를 Pool로 반환
            GameManager.instance.turnManager.RemoveDeadUnit(this, "Ally");      // 유닛 제거
            DestroyObject();
        }
        else
        {
            hud.UpdateHUD();        // HUD의 HP 변화
            Debug.Log($"{gameObject.name}이(가) {damageInfo.amount} 데미지를 받았습니다! 남은 체력: {health}");
        }
    }

    // 회복 시 연산
    public void TakeHeal(int heal)
    {
        if (maxHealth <= health + heal)
        {
            health = maxHealth;
        }
        else
        {
            health += heal;
        }
        hud.UpdateHUD();        // HUD의 HP 변화
    }

    // 보호막 획득 시 연산
    public void TakeShield(int shield)
    {

    }

    // 해당 오브젝트 사망 시
    private void DestroyObject()
    {
        Destroy(gameObject);      // 해당 유닛 제거
    }

    // 처치 확인
    public bool IsDead()
    {
        return health <= 0;
    }

    // ------ 버프 / 디버프 로직 ------

    // 특정 상태이상 추가 ( string 상태이상 종류, int 중첩 수 )
    public void AddStatusEffect(StatusEffectData effectData, int stack)
    {
        // 이미 존재하는 상태이상인지 확인
        var statusEffect = statusEffects.Find(e => e.data == effectData);

        // 이미 존재 시,
        if (statusEffect != null)
        {
            // 상태이상 중첩 수 ++
            statusEffect.stackCount = Mathf.Min(
                statusEffect.data.maxStack,
                statusEffect.stackCount + stack
                );
            Debug.Log($"{gameObject.name}의 {statusEffect.data.effectName} 상태 이상이 {statusEffect.stackCount}로 중첩되었습니다.");
        }
        // 존재하지 않을 시, 새로운 상태이상 추가
        else
        {
            StatusEffect newEffectInstance = new StatusEffect(effectData);
            newEffectInstance.stackCount = stack;
            statusEffects.Add(newEffectInstance);

            Debug.Log($"{gameObject.name}에게 {statusEffect.data.effectName}이(가) 새로 추가되었습니다.");
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
                statusEffects.Remove(statusEffect);    // 상태이상 제거
            }
        }
        
        statusEffectUI.UpdateStatusEffect(statusEffects);    // 상태이상UI 업데이트
    }
    
    // ------ Ally Skill AI 로직 ------

    // 현재 턴 기반으로 스킬 정보 가져오기
    public UnitSkillData GetSkillInfo()
    {
        currentTurn++;      // 내부 턴 수++     문제는 빙결/기절과 같은 CC기로 스킬 스킵이 가능하다.

        // 사용가능한 패턴 불러오기
        List<SkillPattern> validPatterns = data.skillPatterns
            // 현재 턴 = 패턴의 턴 일치 여부 확인 or 모든 턴에 실행가능한 '기본 동작'인지 확인( == -1 )
            .Where(pattern => pattern.turnNumber == currentTurn || pattern.turnNumber == -1)
            .ToList();  // Where()의 IEnumerable<T> => List 변환
                        // .Where(LINQ제공 메서드 : 특정 조건에 해당하는 요소만 선택)

        if (validPatterns.Count == 0)
        {
            Debug.LogError($"{gameObject.name}이(가) 사용가능한 스킬이 없습니다!");
            return null;    // 사용가능한 스킬X 시, null 반환
        }

        // HP기반 & 턴제기반 선택
        foreach (var pattern in validPatterns)
        {
            if (pattern.healthPoint > 0 && pattern.healthPoint >= health / maxHealth)
            {
                pattern.healthPoint = -1;     // 재사용 방지
                curSkillData = pattern.skill;
                return curSkillData;
            }

            if (pattern.turnNumber == currentTurn)
            {
                curSkillData = pattern.skill;
                return curSkillData;
            }
        }

        // 확률 기반 선택
        float totalProbability = validPatterns.Sum(pattern => pattern.probability); // 확률 총합
        float randomValue = Random.Range(0, totalProbability);      // 랜덤 값 산출
        float cumulativeProbability = 0;                            // 계산 값 초기화

        foreach (var pattern in validPatterns)
        {
            cumulativeProbability += pattern.probability;       // 확률기반 랜덤 스킬 지정
            if (randomValue <= cumulativeProbability)
            {
                curSkillData = pattern.skill;
                return curSkillData;
            }
        }

        Debug.LogError($"{gameObject.name}이(가) 사용가능한 스킬이 없습니다!");
        return null;    // 사용가능한 스킬X 시, null 반환
    }
}
