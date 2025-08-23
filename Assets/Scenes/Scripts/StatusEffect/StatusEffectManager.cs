using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    public List<StatusEffectData> statusEffectDatas;    // 상태이상 Data 리스트

    // 이벤트 구독
    void OnEnable()
    {
        BattleEventRouter.OnTurnStarted += DecDurationOfUnits;
    }

    // 이벤트 해지
    void OnDisable()
    {
        BattleEventRouter.OnTurnStarted -= DecDurationOfUnits;
    }

    // 특정 상태이상 정보 반환
    public StatusEffectData GetStatusEffectData(string effectName)
    {
        return statusEffectDatas.Find(e => e.effectName == effectName);
    }

    // 모든 유닛들의 상태이상 지속시간 감소
    private void DecDurationOfUnits()
    {
        // 모든 유닛 정보 취합
        List<ITurn> allUnits = new List<ITurn>();
        allUnits.AddRange(GameManager.instance.turnManager.allies);
        allUnits.AddRange(GameManager.instance.turnManager.enemies);

        // 유닛별 상태이상 지속시간 확인
        foreach (ITurn unit in allUnits)
        {
            DecDuration(unit, unit.statusEffects);
        }
    }

    // 상태이상 지속시간 감소
    public static void DecDuration(ITurn unit, List<StatusEffect> statusEffects)
    {
        foreach (StatusEffect effect in statusEffects)
        {
            if (effect.data.haveDuration)
            {
                effect.duration--;
                if (effect.duration <= 0)
                {
                    unit.DecStatusEffect(effect.data, 99);  // 지속시간이 0일 시, 상태이상 삭제
                }
            }
        }
    }

    // 대상이 지닌 특정 종류의 모든 상태이상의 중첩 수의 합 반환
    public static int GetEffectTypeStack(ITurn unit, StatusEffectData.EffectType effectType)
    {
        // 대상의 상태이상 정보 복사
        List<StatusEffect> statusEffects = unit.statusEffects.FindAll(e => e.data.effectType == effectType);
        int result = 0;

        // 상태이상 수치 저장
        foreach (var statusEffect in statusEffects)
        { 
            result += statusEffect.stackCount;
        }
        return result;
    }

    /* 모든 유닛의 지속시간 확인
    void DecAllDuration()
    {
        // 모든 유닛 정보 취합
        List<ITurn> allUnits = new List<ITurn>();
        allUnits.AddRange(GameManager.instance.turnManager.allies);
        allUnits.AddRange(GameManager.instance.turnManager.enemies);

        // 유닛별 상태이상 지속시간 확인
        foreach (ITurn unit in allUnits)
        {
            StatusEffectProcessor.CheckStatusEffectDuration(unit, unit.statusEffects);
        }

        // 특수 스킬의 패시브 지속시간 확인 (플레이어 대상)
        StatusEffectProcessor.CheckStatusEffectDuration(
            GameManager.instance.turnManager.allies[0],
            GameManager.instance.storageManager.themeManager.passiveEffects
            );
    }
    */
}
