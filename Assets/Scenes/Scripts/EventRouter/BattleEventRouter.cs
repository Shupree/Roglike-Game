using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BattleEventRouter : MonoBehaviour
{
    public enum BattleEventType     // OnBattleEvent 델리게이트 이벤트용
    {
        turnStart, playerTurnStart, onAttack, onHit
    }

    // 명시적 개별 이벤트
    public static event Action OnTurnStarted;
    public static event Action OnPlayerTurnStarted;
    public static event Action OnAttack;
    public static event Action OnHit;

    // 통합 이벤트 (Delegate Event)
    public delegate void BattleEvent(BattleEventType battleEventType);
    public static event BattleEvent OnBattleEvent;          // 전투 시 이벤트를 위한 델리게이트

    // 이벤트 호출 : 전투 턴 시작 시
    public static void RaiseTurnStarted()
    {
        OnTurnStarted?.Invoke();
        OnBattleEvent?.Invoke(BattleEventType.turnStart);
    }

    // 이벤트 호출 : 플레이어 턴 시작 시
    public static void RaisePlayerTurnStarted()
    {
        OnPlayerTurnStarted?.Invoke();
        OnBattleEvent?.Invoke(BattleEventType.playerTurnStart);
    }

    // 이벤트 호출 : 플레이어 공격 시
    public static void RaiseOnAttack()
    {
        OnAttack?.Invoke();
        OnBattleEvent?.Invoke(BattleEventType.onAttack);
    }

    // 이벤트 호출 : 플레이어 피격 시
    public static void RaiseOnHit()
    {
        OnHit?.Invoke();
        OnBattleEvent?.Invoke(BattleEventType.onHit);
    }

    // 필요에 따른 공통 Raise 함수
    public static void Raise(BattleEventType type)
    {
        OnBattleEvent?.Invoke(type);
    }

    // 구독 초기화 (씬 전환이나 재시작용)
    public static void Clear()
    {
        OnTurnStarted = null;
        OnPlayerTurnStarted = null;
        OnAttack = null;
        OnHit = null;
        OnBattleEvent = null;
    }
}
