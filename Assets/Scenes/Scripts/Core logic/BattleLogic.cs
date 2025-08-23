using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 행동 정보를 담을 구조체 (DamageInfo, StatusEffectInfo 포함)
public struct ActionInfo
{
    public DamageInfo damageInfo;
    public StatusEffectInfo statusEffectInfo;
    public int shield;
    public int heal;
}

// 데미지 정보를 담을 구조체
public struct DamageInfo
{
    public int amount;
    public bool isIgnoreShield;
}

// 상태이상 정보를 담을 구조체
public struct StatusEffectInfo
{
    public List<StatusEffectData> effectDatas;
    public List<int> effects;
}

public static class BattleLogic
{
    public static void ActionLogic(ITurn owner, ITurn target, ActionInfo actionInfo)
    {
        if (target == null) { return; } // 적 소실 시 종료

        if (actionInfo.damageInfo.amount > 0)      // 기본 데미지가 0일 시 스킵
        {
            // OnAttack 로직 호출 (데미지 계산 전)
            // 리스트 복사본을 만들어 순회 중 리스트 변경으로 인한 오류 방지
            List<StatusEffect> effectsToProcess = new List<StatusEffect>(owner.statusEffects);
            effectsToProcess.ForEach(effect => effect.logic.OnAttack(owner, target, effect, ref actionInfo.damageInfo));

            target.TakeDamage(actionInfo.damageInfo, true);       // 공격
        }

        // 적 상태이상 부여
        for (int e = 0; e < actionInfo.statusEffectInfo.effects.Count; e++)
        {
            target.AddStatusEffect(actionInfo.statusEffectInfo.effectDatas[e], actionInfo.statusEffectInfo.effects[e]);

            // OnGiveStatusEffect 로직 호출 (데미지 계산 후)
            // 리스트 복사본을 만들어 순회 중 리스트 변경으로 인한 오류 방지
            List<StatusEffect> effectsToProcess = new List<StatusEffect>(owner.statusEffects);
            effectsToProcess.ForEach(effect => effect.logic.OnGiveStatusEffect(owner, target, actionInfo.statusEffectInfo.effectDatas[e], effect));
        }

        if (actionInfo.heal > 0)
        {
            target.TakeHeal(actionInfo.heal);
        }

        if (actionInfo.shield > 0)
        {
            target.TakeShield(actionInfo.shield);
        }
    }
}
