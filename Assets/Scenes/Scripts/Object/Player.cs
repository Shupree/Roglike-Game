using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, ITurn
{
    [Header("Reference")]
    //private PaintManager paintManager;
    [HideInInspector] public StorageManager storageManager;

    [Header("HUD")]
    private HUD hud;     // HUD
    private StatusEffectUI statusEffectUI;  // 상태이상 UI

    public int gold;   // 골드 수

    [Header("Status")]
    private int maxHealth = 50;
    public int health = 50;     // 체력
    private int shield;      // 보호막

    public int canvas;

    [Header("Skill")]
    // private CsvSkillLoader skillLoader;
    // public Skill[] skillArr = new Skill[4];     // 기본 스킬 4종 (빨강, 노랑, 파랑, 하양)
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

        // paintManager = GameManager.instance.paintManager;
            storageManager = GameManager.instance.storageManager;   // 스크립트 가져오기
        storageManager.Initialize();    // storageManager 강제 초기화

        mainSkill = null;
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
                value = shield;
                break;
        }

        return value;   // 값 반환
    }

    // 상태이상 값 확인
    public int GetStatusEffect(string effectName)
    {
        if (statusEffects.Exists(e => e.name == effectName))
        {      // 버프/디버프 존재 시
            return statusEffects.Find(e => e.name == effectName).stackCount;
        }
        else
        {          // 아닐 시 0으로 반환
            return 0;
        }
    }

    // 스킬 발동 로직
    public void ExecuteSkill()
    {
        int damage = 0;             // 최종 데미지
        bool isTrueDamage = false;  // 고정 데미지 유무

        int count = 0;              // 스킬 타수

        // 공격 type에 따른 분류    (targets는 turnManager로부터 받음)
        switch (mainSkill.skillType)
        {
            // 단타 공격
            case Skill.SkillType.SingleAtk:
                count = mainSkill.count;
                break;
            // 전체 공격
            case Skill.SkillType.SplashAtk:
                count = mainSkill.count;
                break;
            // 바운스 공격
            case Skill.SkillType.BounceAtk:
                count = 1;
                break;
            // 자신 보조
            case Skill.SkillType.SingleSup:    // 자기자신 타겟 스킬
                count = mainSkill.count;
                isTrueDamage = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case Skill.SkillType.SplashSup:
                count = mainSkill.count;
                isTrueDamage = true;    // 아군 대상은 고정데미지
                break;
        }

        for (int c = 0; c < targets.Count; c++)
        {
            for (int i = 0; i < count; i++)   // 타수만큼 반복
            {
                if (targets[c] == null)     // 적 제거 시 스킬 이펙트만 보여줄 것!
                {
                    continue;
                }
                if (mainSkill.damage > 0)    // 기본 데미지가 0일 시 스킵
                {
                    damage = mainSkill.damage;   // 기본 데미지
                    if (!isTrueDamage)
                    {
                        damage += targets[c].GetStatusEffect("Burn");   // 화상 데미지
                    }
                    targets[c].TakeDamage(damage, false);      // 공격
                    Debug.Log($"{targets[c]}은 {damage} 의 데미지를 입었다.");
                }

                if (mainSkill.heal > 0)
                {
                    targets[c].TakeHeal(mainSkill.heal);
                    Debug.Log($"{targets[c]}은 {mainSkill.heal} 만큼 체력을 회복했다.");
                }

                // 적 상태이상 부여
                if (mainSkill.effect > 0)
                {
                    targets[c].AddStatusEffect(mainSkill.effectType, mainSkill.effect);
                    Debug.Log($"{targets[c]}은 {mainSkill.effectType}을 {mainSkill.effect}만큼 받었다.");
                }
            }
        }
        // 데미지 연산 : 기본 데미지 + 화상 데미지 + 집중 효과

        targets.Clear();
        mainSkill = null;
    }

    // 피격 시 데미지 연산
    public void TakeDamage(int damage, bool isTrueDamage)
    {
        if (isTrueDamage)
        {
            health -= damage;
            if (health < 0)
            {
                health = 0;
                GameManager.instance.turnManager.CheckBattleEndConditions();
            }
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
                if (health < 0) health = 0;
            }
        }

        hud.UpdateHUD();        // HUD의 HP 변화
        Debug.Log($"플레이어가 {damage} 데미지를 받았습니다! 남은 체력: {health}");
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
    public void AddStatusEffect(string effectName, int stack)
    {
        // 이미 존재하는 상태이상인지 확인
        var statusEffect = statusEffects.Find(e => e.name == effectName);

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
                Debug.Log($"플레이어의 {effectName} 상태 이상이 {statusEffect.stackCount}로 중첩되었습니다.");
            }
            else
            {
                Debug.Log($"플레이어의 {effectName} 상태 이상이 최대 중첩에 도달했습니다.");
            }
        }
        // 존재하지 않을 시, 새로운 상태이상 추가
        else
        {
            statusEffect = GameManager.instance.statusEffects.Find(s => s.name == effectName);
            statusEffect.stackCount = stack;        // 새로 추가되는 효과는 기본 중첩 = stack 수
            statusEffects.Add(statusEffect);        // 새 상태이상 추가
            Debug.Log($"{effectName}이(가) 새로 추가되었습니다.");
        }

        // '변환'타입의 상태이상 적용
        if (statusEffect.effectInfo == "convert" && statusEffect.stackCount <= statusEffect.needStack)
        {
            AddStatusEffect(statusEffect.efffectDetail, 1);
            DecStatusEffect(effectName, statusEffect.needStack);    // 필요 중첩 수만큼 제거
        }

        statusEffectUI.UpdateStatusEffect(statusEffects);    // 상태이상UI 업데이트
    }

    // 버프/디버프 제거
    public void DecStatusEffect(string effectName, int stack)
    {
        var statusEffect = statusEffects.Find(e => e.name == effectName);
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

    // 버프/디버프 지속시간 확인
    public void CheckStatusEffectDuration()
    {
        foreach (var statusEffect in statusEffects)
        {
            if (statusEffect.isConsumable == false && statusEffect.duration != -1)
            {
                // 턴제 지속형일 시, 상태이상 지속시간 줄이기.
                DecStatusEffect(statusEffect.name, statusEffect.duration);
            }
            else
            {
                continue;
            }
        }
    }
}
