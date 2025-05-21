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

    [Header("Status")]
    public int health;     // 체력
    private int maxHealth;  // 최대 체력
    public int shield;      // 보호막

    private int damage;

    [Header("Target")]
    public List<ITurn> targets = new List<ITurn>();    // 공격 타겟

    [Header("Status Effect")]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();   // 상태이상 저장공간

    [Header("Individual Turn")]
    private int currentTurn;    // 유닛별 개별 턴 수

    void Awake()
    {
        maxHealth = data.maxHealth;
        health = maxHealth;

        currentTurn = 1;
    }

    public void TakeTurn()
    {
        foreach (var effect in statusEffects)
        {
            effect.ApplyEffect(this);   // 특정 상태이상 효과 적용

            effect.duration--;      // 지속 턴 수 감소
            if (effect.duration <= 0 || effect.stackCount <= 0)     // (지속 시간 = 0 or 중첩 수 = 0)
            {
                effect.RemoveEffect(this);      // 특정 상태이상 제거
            }
        }

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
                targets = GameManager.instance.turnManager.allies;
                break;
            // 자신 보조
            case UnitSkillData.SkillType.SingleSup:    // 자기자신 타겟 스킬
                targets.Add(this);
                isTrueDamage = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case UnitSkillData.SkillType.SplashSup:
                targets = GameManager.instance.turnManager.enemies;
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
                        damage += targets[c].HasBurnDebuff();   // 화상 데미지
                    }
                    targets[c].TakeDamage(damage);      // 공격
                    Debug.Log($"{targets[c]}은 {damage} 의 데미지를 입었다.");
                }

                // 적 상태이상 부여
                if (curSkillData.effect > 0)
                {
                    targets[c].AddStatusEffect(GameManager.instance.statusEffects.Find(s => s.name == curSkillData.effectType), curSkillData.effect);
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

    // 피격 시 데미지 연산
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;     // 음수값 제외     
            GameManager.instance.turnManager.RemoveDeadUnit(this,"Enemy");    // 유닛 제거
            DestroyObject();
        }
        Debug.Log($"{gameObject.name}이 {damage} 데미지를 받았습니다! 남은 체력: {health}");
    }

    // 특정 상태이상 추가 ( StatusEffect 상태이상 종류, int 중첩 수 )
    public void AddStatusEffect(StatusEffect effect, int stack)
    {
        // 이미 존재하는 상태이상인지 확인
        var existingEffect = statusEffects.Find(e => e.name == effect.name);

        // 이미 존재 시,
        if (existingEffect != null)
        {
            // 중첩 가능 여부 확인
            if (existingEffect.stackable > existingEffect.stackCount)
            {
                existingEffect.stackCount = Mathf.Min(
                    existingEffect.stackable,
                    existingEffect.stackCount + stack
                );
                Debug.Log($"{gameObject.name}의 {effect.name} 상태 이상이 {existingEffect.stackCount}로 중첩되었습니다.");
            }
            else
            {
                Debug.Log($"{gameObject.name}의 {effect.name} 상태 이상이 최대 중첩에 도달했습니다.");
            }
        }
        // 존재하지 않을 시, 새로운 상태이상 추가
        else
        {
            effect.stackCount = stack;      // 새로 추가되는 효과는 기본 중첩 = stack 수
            statusEffects.Add(effect);
            Debug.Log($"{gameObject.name}에게 {effect.name}이(가) 새로 추가되었습니다.");
        }
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

    // 빙결 디버프 유무 확인
    public bool HasFreezeDebuff()
    {
        return statusEffects.Exists(e => e.name == "Freeze");
    }

    // 빙결 디버프 제거
    public void RemoveFreezeDebuff()
    {
        var freezeEffect = statusEffects.Find(e => e.name == "Freeze");
        if (freezeEffect != null)
        {
            freezeEffect.RemoveEffect(this);    // 빙결 디버프 제거
        }
    }

    // 화상 디버프 수치 확인
    public int HasBurnDebuff()
    {
        if (statusEffects.Exists(e => e.name == "Burn"))
        {      // 화상 디버프 존재 시
            return statusEffects.Find(e => e.name == "Burn").stackCount;
        }
        else
        {          // 아닐 시 0으로 반환
            return 0;
        }
    }

    // ------ Enemy Skill AI 로직 ------

    // 공격 패턴 존재 여부 확인
    /*public void PerformTurn()
    {
        // 현재 턴에 맞는 스킬 정보 가져오기
        SkillPattern selectedPattern = GetSkillInfo();
        if (selectedPattern != null)
        {
            // 스킬 발동
            ExecuteSkill(selectedPattern.skill);
        }
    }*/

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
