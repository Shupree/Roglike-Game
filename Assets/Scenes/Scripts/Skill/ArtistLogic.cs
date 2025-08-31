using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 두 개의 특성을 다루는 기본 테마 : 아티스트
public class AchromaticColor_Logic : StatusEffectLogic
{
    private int usedPaintStack = 0;         // 사용한 물감 스택 (3스택 => 1무채색)
    private int turnStackLimit = 3;             // 턴당 스택 제한 (턴당 3무채색)

    // 턴 시작 시, 턴당 스택 제한을 초기화합니다.
    public override void OnTurnStart(IUnit owner, StatusEffect effectInstance)
    {
        turnStackLimit = 3;
        usedPaintStack = 0;      // 턴이 시작될 때 누적된 물감 수도 초기화하는 것이 좋습니다.
    }

    // 플레이어가 공격할 때마다 자동으로 호출됩니다.
    public override void OnAttack(IUnit owner, IUnit target, StatusEffect effectInstance, ref DamageInfo damage)
    {
        // 턴당 최대 획득 가능 스택을 초과했거나, 패시브 최대 스택에 도달했다면 더 이상 누적하지 않습니다.
        if (turnStackLimit <= 0 || effectInstance.stackCount >= effectInstance.data.maxStack)
            return;

        // TurnManager에서 마지막에 사용한 물감 수를 가져옵니다.
        int usedPaintNum = GameManager.instance.turnManager.usedPaintNum;
        usedPaintStack += usedPaintNum;

        // 누적된 물감 수가 3 이상이면 '무채색' 스택으로 전환합니다.
        while (usedPaintStack >= 3 && turnStackLimit > 0 && effectInstance.stackCount < effectInstance.data.maxStack)
        {
            effectInstance.stackCount++; // '무채색' 스택 1 증가
            usedPaintStack -= 3;
            turnStackLimit--;
        }
    }
}

public class PrimaryColor_Logic : StatusEffectLogic
{
    public override void OnAttack(IUnit owner, IUnit target, StatusEffect effectInstance, ref DamageInfo damage)
    {
        // 이 효과는 플레이어에게만 적용되어야 합니다.
        if (!(owner is Player))
            return;

        // 적용할 중첩이 있는지 확인합니다.
        if (effectInstance.stackCount > 0)
        {
            int usedPaintNum = GameManager.instance.turnManager.usedPaintNum;   // 사용한 물감 수
            int stacksConsumed = 0;     // 사용할 중첩 수

            // 1. 소모한 물감 수와 중첩 수 비교
            if (usedPaintNum >= effectInstance.stackCount)
            {
                stacksConsumed = effectInstance.stackCount;
            }
            else
            {
                stacksConsumed = usedPaintNum;
            }

            // 2. 소모된 중첩만큼 스킬의 데미지가 증가
            damage.amount += stacksConsumed;
            Debug.Log($"'삼원색' 효과 발동! 데미지가 {stacksConsumed}만큼 증가했습니다.");

            // 3. 소모한 중첩만큼 물감 소모를 대체
            PaintManager paintManager = GameManager.instance.paintManager;
            if (paintManager != null)
            {
                paintManager.ReturnPartialPaint(stacksConsumed);
                Debug.Log($"'삼원색' 효과로 물감을 {stacksConsumed}개 돌려받았습니다.");
            }

            // 4. 해당 버프의 중첩 소모
            owner.DecStatusEffect(effectInstance.data, stacksConsumed);
        }
    }
}