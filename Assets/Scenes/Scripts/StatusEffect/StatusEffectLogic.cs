using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데미지 정보를 담을 구조체 (확장성을 위해)
public struct DamageInfo
{
    public int amount;
    public bool isIgnoreShield;
    // public DamageType type; // 화염, 냉기 등 추가 가능
}

// 모든 상태이상 로직 클래스가 따라야 할 규격
public abstract class StatusEffectLogic
{
    // 효과가 처음 적용될 때 호출
    public virtual void OnApply(ITurn owner) { }

    // 효과가 제거될 때 호출
    public virtual void OnRemove(ITurn owner) { }

    // 소유자의 턴이 시작될 때 호출
    public virtual void OnTurnStart(ITurn owner, StatusEffect effectInstance) { }
    
    // 소유자의 턴이 끝날 때 호출
    public virtual void OnTurnEnd(ITurn owner, StatusEffect effectInstance) { }

    // 소유자가 공격받을 때 호출 (데미지 계산 전)
    public virtual void OnBeingHit(ITurn owner, StatusEffect effectInstance, ref DamageInfo damage) { }

    // 소유자가 공격할 때 호출
    public virtual void OnAttack(ITurn owner, ITurn target, StatusEffect effectInstance, ref DamageInfo damage) { }
}

// ----- 버프계열 로직 -----

// 막기 _ 로직
public class Block_Logic : StatusEffectLogic
{
    // 피격 시
    public override void OnBeingHit(ITurn owner, StatusEffect effectInstance, ref DamageInfo damage)
    {
        if (damage.amount > 0)
        {
            Debug.Log($"'막기' 효과 발동! 원래 데미지: {damage.amount} -> 1");
            damage.amount = 1; // 받는 피해를 1로 감소
            effectInstance.stackCount--; // 횟수 1 감소
        }
    }
}

// 집중 _ 로직
public class Concentration_Logic : StatusEffectLogic
{
    // 공격 시
    public override void OnAttack(ITurn owner, ITurn target, StatusEffect effectInstance, ref DamageInfo damage)
    {
        if (damage.amount >= 0)
        {
            int extraDamage = effectInstance.stackCount;
            damage.amount += extraDamage;
            Debug.Log($"'집중' 효과 발동! 데미지가 {extraDamage}만큼 증가했습니다.");

            // 집중 효과는 공격 후 소모됨
            owner.DecStatusEffect(effectInstance.data, 99);
        }
    }
}

// ----- 지속피해 디버프계열 로직 -----

// 출혈 _ 로직
public class Bleed_Logic : StatusEffectLogic
{
    // 턴 종료 시
    public override void OnTurnEnd(ITurn owner, StatusEffect effectInstance)
    {
        // (중첩 수) 만큼 피해
        var damage = new DamageInfo { amount = effectInstance.stackCount, isIgnoreShield = false };
        owner.TakeDamage(damage.amount, damage.isIgnoreShield);
        Debug.Log($"{owner}가 출혈 효과로 {damage.amount}의 피해를 입었습니다.");
    }

    // 피격 시
    public override void OnBeingHit(ITurn owner, StatusEffect effectInstance, ref DamageInfo damage)
    {
        if (damage.amount > 0) // 데미지가 0 이상일 때만 발동
        {
            int reduction = effectInstance.stackCount / 2; // 50% (내림)
            if (reduction > 0)
            {
                damage.amount += reduction;
                effectInstance.stackCount -= reduction;     // 스택 감소
                Debug.Log($"{owner}가 피격당해 출혈 스택이 {reduction}만큼 감소하고, {reduction}의 추가 피해를 입었습니다.");
            }
        }
    }
}

// ----- 행동방해 디버프계열 로직 -----

// 약화 _ 로직
public class Weak_Logic : StatusEffectLogic
{
    // 공격 시
    public override void OnAttack(ITurn owner, ITurn target, StatusEffect effectInstance, ref DamageInfo damage)
    {
        if (damage.amount > 0)
        {
            int originalDamage = damage.amount;
            // 데미지를 25% 감소시킵니다. (정수 계산을 위해 곱셈 후 나눗셈)
            damage.amount = damage.amount * 75 / 100;
            Debug.Log($"'약화' 효과 발동! {owner}의 데미지가 {originalDamage}에서 {damage.amount}로 25% 감소했습니다.");
        }
    }

    // 턴 종료 시
    public override void OnTurnEnd(ITurn owner, StatusEffect effectInstance)
    {
        // 중첩 수 1 감소
        owner.DecStatusEffect(effectInstance.data, 1);
    }
}
