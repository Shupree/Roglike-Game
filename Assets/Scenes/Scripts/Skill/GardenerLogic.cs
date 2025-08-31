using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 방해효과 디버프를 부여하며 체력을 서서히 갉아먹는 플레이의 테마 : 정원사

// 패시브 : 원예
public class Gardening_Logic : StatusEffectLogic
{
    // giver: 효과를 부여하는 주체 (플레이어)
    // receiver: 효과를 받는 대상 (적)
    // givenEffectData: 부여되는 효과의 데이터
    public override void OnGiveStatusEffect(IUnit giver, IUnit receiver, StatusEffectData givenEffectData, StatusEffect effectInstance)
    {
        // 플레이어가 적에게 CC기를 부여하는 경우에만 발동
        if (giver is Player && receiver is Enemy && givenEffectData.effectType == StatusEffectData.EffectType.CC)
        {
            StatusEffectData effectData = GameManager.instance.statusEffectManager.statusEffectDatas.Find(e => e.effectName == "parasiticSeed");

            if (effectData != null)
            {
                receiver.AddStatusEffect(effectData, 1);
            }
            else
            {
                Debug.LogError("'기생화' StatusEffectData를 찾을 수 없습니다!");
            }
        }
    }
}

// 패시브 : 기생 씨앗
public class ParasiticSeed_Logic : StatusEffectLogic
{
    public override void OnTurnStart(IUnit owner, StatusEffect effectInstance)
    {
        if (effectInstance.stackCount > 0)
        {
            DamageInfo damageInfo = new DamageInfo { amount = effectInstance.stackCount, isIgnoreShield = true };
            owner.TakeDamage(damageInfo, false); // 'false'는 플레이어의 공격이 아님을 의미
        }
    }
}

// 패시브 : 피안화 만개
public class Lycoris_Logic : StatusEffectLogic
{
    private void TriggerEffect(IUnit owner, StatusEffect effectInstance)
    {
        if (effectInstance.stackCount > 0)
        {
            // 5의 피해
            DamageInfo damageInfo = new DamageInfo { amount = 5, isIgnoreShield = true };
            owner.TakeDamage(damageInfo, false);

            // 플레이어 2 회복
            GameManager.instance.player.TakeHeal(2);

            // 중첩 1 감소
            owner.DecStatusEffect(effectInstance.data, 1);
        }
    }

    // 이 효과를 가진 유닛이 공격할 때
    public override void OnAttack(IUnit owner, IUnit target, StatusEffect effectInstance, ref DamageInfo damage)
    {
        TriggerEffect(owner, effectInstance);
    }

    // 이 효과를 가진 유닛이 공격받을 때
    public override void OnBeingHit(IUnit owner, StatusEffect effectInstance, ref DamageInfo damage)
    {
        TriggerEffect(owner, effectInstance);
    }
}