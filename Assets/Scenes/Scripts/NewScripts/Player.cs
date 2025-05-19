using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, ITurn
{
    [Header("Reference")]
    private PaintManager paintManager;

    [Header("Status")]
    private int maxHealth;
    public int health;     // 체력
    private int shield;      // 보호막

    public int canvas;

    [Header("Skill")]
    private CsvSkillLoader skillLoader;
    public Skill[] skillArr = new Skill[4];     // 기본 스킬 4종 (빨강, 노랑, 파랑, 하양)
    public Skill mainSkill;        // 현재 사용하는 스킬

    [Header("Target")]
    public List<ITurn> targets = new List<ITurn>();    // 공격 타겟
    // 스플래쉬 공격에 경우 모든 적에게 타겟팅해야함.
    // 버프 스킬의 경우 아군이 타겟팅되어야함.

    [Header("Status Effect")]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();   // 상태이상.json

    // 초기화
    void Awake()
    {
        paintManager = GameManager.instance.paintManager;

        skillLoader = GameManager.instance.skillLoader;

        skillArr[0] = skillLoader.skillList.Find(s => s.name == "FlameShot");
        skillArr[1] = skillLoader.skillList.Find(s => s.name == "Thunder");
        skillArr[2] = skillLoader.skillList.Find(s => s.name == "Brinicle");
        skillArr[3] = skillLoader.skillList.Find(s => s.name == "AcrylShield");

        mainSkill = null;

        health = 50;    // 임의 설정값
    }

    /*
    public Player()
    {
        maxHealth = 50;
        health = maxHealth;
    }
    */

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
            case "Shield":
                return shield;
        }

        return value;   // 값 반환
    }

    // 스킬 발동 로직
    public void ExecuteSkill()
    {
        int damage = 0;             // 최종 데미지
        bool isTrueDamage = false;  // 고정 데미지 유무
        Debug.Log(targets[0]);

        // 공격 type에 따른 분류    (targets는 turnManager로부터 받음)
        switch (mainSkill.skillType)
        {
            // 단타 공격
            case Skill.SkillType.SingleAtk:

                break;

            // 전체 공격
            case Skill.SkillType.SplashAtk:

                break;
            // 자신 보조
            case Skill.SkillType.SingleSup:    // 자기자신 타겟 스킬
                isTrueDamage = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case Skill.SkillType.SplashSup:
                isTrueDamage = true;    // 아군 대상은 고정데미지
                break;
        }

        for (int c = 0; c < targets.Count; c++)
        {
            for (int i = 0; i < mainSkill.count; i++)   // 타수만큼 반복
            {
                if (mainSkill.damage > 0)    // 기본 데미지가 0일 시 스킵
                {
                    damage = mainSkill.damage;   // 기본 데미지
                    if (!isTrueDamage)
                    {
                        damage += targets[c].HasBurnDebuff();   // 화상 데미지
                    }
                    targets[c].TakeDamage(damage);      // 공격
                    Debug.Log($"{targets[c]}은 {damage} 의 데미지를 입었다.");
                }

                // 적 상태이상 부여
                if (mainSkill.effect > 0)
                {
                    targets[c].AddStatusEffect(GameManager.instance.statusEffects.Find(s => s.name == mainSkill.effectType), mainSkill.effect);
                    Debug.Log($"{targets[c]}은 {mainSkill.effectType}을 {mainSkill.effect}만큼 받었다.");
                }
            }
        }
        // 데미지 연산 : 기본 데미지 + 화상 데미지 + 집중 효과

        targets.Clear();
        mainSkill = null;
    }

    // 피격 시 데미지 연산
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        Debug.Log($"플레이어가 {damage} 데미지를 받았습니다! 남은 체력: {health}");
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
                Debug.Log($"플레이어의 {effect.name} 상태 이상이 {existingEffect.stackCount}로 중첩되었습니다.");
            }
            else
            {
                Debug.Log($"플레이어의 {effect.name} 상태 이상이 최대 중첩에 도달했습니다.");
            }
        }
        // 존재하지 않을 시, 새로운 상태이상 추가
        else
        {
            effect.stackCount = stack;      // 새로 추가되는 효과는 기본 중첩 = stack 수
            statusEffects.Add(effect);
            Debug.Log($"{effect.name}이(가) 새로 추가되었습니다.");
        }
    }

    // 처치 확인
    public bool IsDead()
    {
        return health <= 0;
    }

    // ------ 버프 / 디버프 로직 ------

    // 빙결 디버프 존재 유무 확인
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

    // Unit별 AI 코드 사용 X
    public UnitSkillData GetSkillInfo()
    {
        Debug.LogError("사용하지 않는 코드입니다.");
        return null;
    }
}
