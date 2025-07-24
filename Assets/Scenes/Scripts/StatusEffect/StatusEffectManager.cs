using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private ITurn player;

    void Awake()
    {
        player = GameManager.instance.turnManager.player;

        SubscribeEvent();   // 이벤트 구독
    }

    void SubscribeEvent()
    {
        BattleEventRouter.OnTurnStart += StatusEffectDurationHandler;
        BattleEventRouter.OnBattleEvent += PlayerStatusEffectHandler;
    }

    void UnsubscribeEvent()
    {
        BattleEventRouter.OnTurnStart -= StatusEffectDurationHandler;
    }

    // 모든 유닛의 지속시간 확인
    void StatusEffectDurationHandler()
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

    // 플레이어 상태이상 효과 발동
    void PlayerStatusEffectHandler(BattleEventRouter.BattleEventType type)
    {
        switch (type)
        {
            case BattleEventRouter.BattleEventType.playerTurnStart:
                StatusEffectProcessor.ProcessStatusEffects(player, player.statusEffects, StatusEffectProcessor.Situation.turnStart);
                break;
            case BattleEventRouter.BattleEventType.onAttack:
                StatusEffectProcessor.ProcessStatusEffects(player, player.statusEffects, StatusEffectProcessor.Situation.onAttack);
                break;
            case BattleEventRouter.BattleEventType.onHit:
                StatusEffectProcessor.ProcessStatusEffects(player, player.statusEffects, StatusEffectProcessor.Situation.onHit);
                break;
        }
    }
        /*
else
{
    string situation = type.ToString();     // Enum => 문자열(string)

    // 유닛별 상태이상 효과
    foreach (ITurn unit in allUnits)
    {
        StatusEffectProcessor.ProcessStatusEffects(unit, unit.statusEffects, situation);
    }

    // 특수 스킬의 패시브 효과 (플레이어 대상)
    StatusEffectProcessor.ProcessStatusEffects(
        GameManager.instance.turnManager.allies[0],
        GameManager.instance.storageManager.themeManager.passiveEffects,
        situation
        );
}
*/
}
