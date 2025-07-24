using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatusEffectProcessor
{
    public enum Situation
    {
        turnStart, onAttack, onHit, always
    }

    // 상태이상 효과 발동
    public static void ProcessStatusEffects(ITurn unit, List<StatusEffect> statusEffects, Situation situation)
    {
        // 해당 상황에서 발동가능한 상태이상 확인
        List<StatusEffect> triggeredEffects = statusEffects.FindAll(s => s.whenIsTrigger == situation);

        foreach (StatusEffect effect in triggeredEffects)
        {
            // 상태이상 발동 조건 확인
            if (effect.stackCount >= effect.needStack)
            {
                switch (effect.effectInfo)
                {
                    // 공격력 업 (각 유닛의 메인 스크립트-공격 매커니즘 참조 )
                    case EffectInfo.attackUp:
                        break;
                    // 대상 보호막+
                    case EffectInfo.shield:
                        unit.TakeShield(int.Parse(effect.efffectDetail));
                        break;
                    // 대상 회복
                    case EffectInfo.heal:
                        unit.TakeHeal(int.Parse(effect.efffectDetail));
                        break;
                    // 타 상태이상으로 변환
                    case EffectInfo.convert:
                        unit.AddStatusEffect(effect.efffectDetail, 1);
                        unit.DecStatusEffect(effect.nameEn, effect.needStack);    // 필요 중첩 수만큼 제거
                        break;
                    // 타 상태이상 추가
                    case EffectInfo.addEffect:
                        unit.AddStatusEffect(effect.efffectDetail, 1);
                        break;
                    // 대상 턴 스킵 (TurnManager 참조)
                    case EffectInfo.turnSkip:
                        break;
                    // 특정 요소의 대용으로 사용 (미완)
                    case EffectInfo.substitute:
                        break;
                }

                // 소모성 스킬의 경우 사용 시 스택 수 감소
                if (effect.isConsumable && effect.decreaseNum != -1)
                {
                    unit.DecStatusEffect(effect.nameEn, effect.decreaseNum);
                }

                // 지속성 스킬의 경우 각 개체의 턴 시작 시 효과 참조
            }
        }
    }

    // 상태이상 지속시간 감소
    public static void CheckStatusEffectDuration(ITurn unit, List<StatusEffect> statusEffects)
    {
        foreach (StatusEffect effect in statusEffects)
        {
            if (!effect.isConsumable && effect.decreaseNum != -1)
            {
                unit.DecStatusEffect(effect.nameEn, effect.decreaseNum);
            }
        }
    }
}