using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, ITurn
{
    [Header("Reference")]
    private SpriteRenderer targetSpriteRenderer;

    [Header("Enemy Data")]
    public EnemyData data;
    private UnitSkillData curSkillData;     // 현재 사용할 스킬 data

    [Header("HUD")]
    private HUD hud;     // HUD
    private StatusEffectUI statusEffectUI;     // 상태이상 UI

    [Header("Status")]
    public int health;     // 체력
    private int maxHealth;  // 최대 체력
    public int shield;      // 보호막

    private bool isDead = false;

    [Header("Target")]
    public List<ITurn> targets = new List<ITurn>();    // 공격 타겟

    // 상태이상 List
    public List<StatusEffect> statusEffects { get; private set; } = new List<StatusEffect>();

    [Header("Individual Turn")]
    private int currentTurn;    // 유닛별 개별 턴 수

    void Awake()
    {
        isDead = false;

        maxHealth = data.maxHealth;
        health = maxHealth;

        currentTurn = 1;
    }

    public void SetHUD(GameObject hudObject)
    {
        hud = hudObject.GetComponent<HUD>();                // HUD 가져오기
        statusEffectUI = hudObject.transform.GetChild(1).GetChild(0).GetComponent<StatusEffectUI>();    // 상태이상 UI 가져오기
        hud.UpdateHUD();                                    // HUD 정보 업데이트
        statusEffectUI.UpdateStatusEffect(statusEffects);   // 상태이상 정보 업데이트
    }

    public void TakeTurn()
    {
        Debug.Log("적이 행동합니다!");
        // 적 행동 로직

        int damage = 0;             // 최종 데미지
        bool isTrueDamage = false;  // 고정 데미지 유무

        // 공격 type에 따른 분류
        switch (curSkillData.skillType)
        {
            // 단타 공격
            case UnitSkillData.SkillType.SingleAtk:
                // 플레이어 타겟팅
                targets.Add(GameManager.instance.turnManager.allies[0]);    // '도발'효과 때문에 스크립트 수정 필요
                break;

            // 전체 공격
            case UnitSkillData.SkillType.SplashAtk:
                targets = new List<ITurn> (GameManager.instance.turnManager.allies);
                break;
            // 자신 보조
            case UnitSkillData.SkillType.SingleSup:    // 자기자신 타겟 스킬
                targets.Add(this);
                isTrueDamage = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case UnitSkillData.SkillType.SplashSup:
                targets = new List<ITurn> (GameManager.instance.turnManager.enemies);
                isTrueDamage = true;    // 아군 대상은 고정데미지
                break;
        }

        for (int c = 0; c < targets.Count; c++)
        {
            for (int i = 0; i < curSkillData.count; i++)   // 타수만큼 반복
            {
                if (curSkillData.damage > 0)    // 기본 데미지가 0일 시 스킵
                {
                    damage = curSkillData.damage;   // 기본 데미지
                    if (!isTrueDamage)
                    {
                        // '공격력 업' 관련 버프 적용
                        List<StatusEffect> attackEffects = statusEffects.FindAll(s => s.effectInfo == EffectInfo.attackUp);
                        foreach (StatusEffect effect in attackEffects)
                        {
                            damage += int.Parse(effect.efffectDetail) * effect.stackCount;

                            // 소모성 상태이상일 시 스택 수 감소
                            if (effect.isConsumable && effect.decreaseNum != -1)
                            {
                                DecStatusEffect(effect.nameEn, effect.decreaseNum);
                            }
                        }
                    }
                    targets[c].TakeDamage(damage, false);      // 공격
                    Debug.Log($"{targets[c]}은 {damage} 의 데미지를 입었다.");
                }

                if (curSkillData.heal > 0)
                {
                    targets[c].TakeHeal(curSkillData.heal);
                    Debug.Log($"{targets[c]}은 {curSkillData.heal} 만큼 체력을 회복했다.");
                }

                // 적 상태이상 부여
                if (curSkillData.effect > 0)
                {
                    targets[c].AddStatusEffect(curSkillData.effectType, curSkillData.effect);
                    Debug.Log($"{targets[c]}은 {curSkillData.effectType}을 {curSkillData.effect}만큼 받었다.");
                }
            }
        }
        // 데미지 연산 : 기본 데미지 + 화상 데미지 + 집중 효과

        targets.Clear();
    }

    // 특정 스테이터스 값 확인
    public int GetStatus(string status)
    {
        int value = 0;

        switch (status)
        {
            // HP 값 반환
            case "HP":
                value = health;
                break;
            // MaxHP 값 반환
            case "MaxHP":
                value = maxHealth;
                break;
                //case "Shield":
                //    return shield;
        }

        return value;   // 값 반환
    }

    // 상태이상 값 확인
    public int GetStatusEffect(string effectName)
    {
        if (statusEffects.Exists(e => e.nameEn == effectName))
        {      // 버프/디버프 존재 시
            return statusEffects.Find(e => e.nameEn == effectName).stackCount;
        }
        else
        {          // 아닐 시 0으로 반환
            return 0;
        }
    }

    // 피격 시 데미지 연산
    public void TakeDamage(int damage, bool isTrueDamage)
    {
        if (isTrueDamage)
        {
            health -= damage;
        }
        else
        {
            if (shield > damage)
            {
                shield -= damage;
            }
            else
            {
                damage -= shield;
                shield = 0;
                health -= damage;
            }
        }

        if (health <= 0 && isDead == false)
        {
            isDead = true;
            health = 0;     // 음수값 제외
            GameManager.instance.hudPoolManager.ReturnHUD(hud.gameObject);      // HUD를 Pool로 반환
            GameManager.instance.turnManager.lootInfoTuple.Item1 += data.gold;       // 전리품에 골드 추가
            GameManager.instance.turnManager.RemoveDeadUnit(this, "Enemy");      // 유닛 제거
            DestroyObject();
        }
        else
        {
            hud.UpdateHUD();        // HUD의 HP 변화
            Debug.Log($"{gameObject.name}이(가) {damage} 데미지를 받았습니다! 남은 체력: {health}");
        }

        // 피격 시 상태이상 업데이트
        StatusEffectProcessor.ProcessStatusEffects(this, statusEffects, StatusEffectProcessor.Situation.onHit);
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

    // 상태이상 추가 ( string 상태이상 종류, int 중첩 수 )
    public void AddStatusEffect(string effectName, int stack)
    {
        // 이미 존재하는 상태이상인지 확인
        var statusEffect = statusEffects.Find(e => e.nameEn == effectName);

        // 이미 존재 시,
        if (statusEffect != null)
        {
            // 중첩 가능 여부 확인
            if (statusEffect.maxStack > statusEffect.stackCount)
            {
                statusEffect.stackCount = Mathf.Min(
                    statusEffect.maxStack,
                    statusEffect.stackCount + stack
                );
                Debug.Log($"{gameObject.name}의 {statusEffect.name} 상태 이상이 {statusEffect.stackCount}로 중첩되었습니다.");
            }
            else
            {
                Debug.Log($"{gameObject.name}의 {statusEffect.name} 상태 이상이 최대 중첩에 도달했습니다.");
            }
        }
        // 존재하지 않을 시, 새로운 상태이상 추가
        else
        {
            statusEffect = GameManager.instance.statusEffects.Find(s => s.nameEn == effectName);
            statusEffect.stackCount = stack;        // 새로 추가되는 효과는 기본 중첩 = stack 수
            statusEffects.Add(statusEffect);        // 새 상태이상 추가
            Debug.Log($"{gameObject.name}에게 {statusEffect.name}이(가) 새로 추가되었습니다.");
        }

        statusEffectUI.UpdateStatusEffect(statusEffects);    // 상태이상UI 업데이트
    }

    // 상태이상 제거
    public void DecStatusEffect(string effectName, int stack)
    {
        var statusEffect = statusEffects.Find(e => e.nameEn == effectName);
        if (statusEffect != null)
        {
            if (statusEffect.stackCount > stack)
            {
                statusEffect.stackCount -= stack;
            }
            else
            {
                statusEffect.RemoveEffect(this);    // 빙결 디버프 제거
            }
        }

        statusEffectUI.UpdateStatusEffect(statusEffects);    // 상태이상UI 업데이트
    }

    // ------ Enemy Skill AI 로직 ------

    // 현재 턴 기반으로 스킬 정보 가져오기
    public UnitSkillData GetSkillInfo()
    {
        currentTurn++;      // 내부 턴 수++     문제는 빙결/기절과 같은 CC기로 스킬 스킵이 가능하다.
        UnitSkillData basicSkill = null;   // 기본 스킬

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
                pattern.healthPoint = -1;     // 재사용 방지 (1회성)
                curSkillData = pattern.skill;
                return curSkillData;
            }

            if (pattern.turnNumber == currentTurn)
            {
                curSkillData = pattern.skill;
                return curSkillData;
            }
            else if (pattern.turnNumber == -1 && pattern.probability == 0 && pattern.healthPoint == 0)
            {
                basicSkill = pattern.skill;     // 기본 스킬 확인
            }
        }

        // 확률 기반 선택
        float totalProbability = validPatterns.Sum(pattern => pattern.probability); // 확률 총합
        if (totalProbability > 0)       // 확률성 스킬 존재 유무 확인
        {
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
        }

        if (basicSkill != null)
        {
            curSkillData = basicSkill;
            return basicSkill;
        }

        Debug.LogError($"{gameObject.name}이(가) 사용가능한 스킬이 없습니다!");
        return null;    // 사용가능한 스킬X 시, null 반환
    }
}
