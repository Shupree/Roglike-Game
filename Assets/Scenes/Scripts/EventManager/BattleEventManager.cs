using System.Collections;
using System;
using UnityEngine;

// --- 아래는 실제 프로젝트에 맞게 정의해야 할 예시 데이터 구조체입니다. ---
public class Character : MonoBehaviour
{
    public int currentBlock;
    public void AddBlock(int amount) { /* 보호막 추가 로직 */ }
    public void ApplyStatus(StatusEffectType type, int amount) { /* 상태이상 부여 로직 */ }
    public void TakeDamage(int amount) { /* 피해를 받는 로직 */ }
}

public enum ResourceType { Paint }
public enum StatusEffectType { Bleed, Focus, CC }
public enum SkillType { PaintSkill, ThemeSkill, MasterPiece }
public class SkillData { public SkillType skillType; }
// ---

/// <summary>
/// 게임의 모든 주요 이벤트를 관리하는 중앙 허브 역할을 하는 정적 클래스입니다.
/// </summary>
public static class BattleEventManager
{
    // --- 전투 흐름 이벤트 ---
    public static event Action OnBattleStart;
    public static event Action OnBattleEnd;
    public static event Action OnTurnStart;
    public static event Action OnPlayerTurnStart;
    public static event Action OnPlayerTurnEnd;

    // --- 유닛 행동 이벤트 ---
    public static event Action<SkillData> OnSkillPlayed;
    public static event Action<ResourceType, int> OnResourceUsed;
    public static event Action OnPlayerAttack;
    public static event Action<IUnit, int> OnUnitDamaged; // 피격 이벤트 (피격 대상, 피해량)

    // 유닛 상태 관련 이벤트
    public static event Action<IUnit, StatusEffectType> OnStatusEffectApplied;

    // --- 이벤트 호출 메서드 (게임의 다른 곳에서 호출됨) ---
    public static void TriggerBattleStart() => OnBattleStart?.Invoke();
    public static void TriggerBattleEnd() => OnBattleEnd?.Invoke();
    public static void TriggerTurnStart() => OnTurnStart?.Invoke();
    public static void TriggerPlayerTurnStart() => OnPlayerTurnStart?.Invoke();
    public static void TriggerPlayerTurnEnd() => OnPlayerTurnEnd?.Invoke();
    public static void TriggerSkillPlayed(SkillData skill) => OnSkillPlayed?.Invoke(skill);
    public static void TriggerResourceUsed(ResourceType type, int amount) => OnResourceUsed?.Invoke(type, amount);
    public static void TriggerPlayerAttack() => OnPlayerAttack?.Invoke();
    public static void TriggerUnitDamaged(IUnit target, int damage) => OnUnitDamaged?.Invoke(target, damage);
    public static void TriggerStatusEffectApplied(IUnit target, StatusEffectType effect) => OnStatusEffectApplied?.Invoke(target, effect);

    /// <summary>
    /// 모든 정적 이벤트를 초기화합니다. 씬 전환이나 에디터에서 플레이 모드를 나올 때 호출하면 좋습니다.
    /// </summary>
    public static void ClearAllEvents()
    {
        OnBattleStart = null;
        OnBattleEnd = null;
        OnTurnStart = null;
        OnPlayerTurnStart = null;
        OnPlayerTurnEnd = null;
        OnSkillPlayed = null;
        OnResourceUsed = null;
        OnPlayerAttack = null;
        OnUnitDamaged = null;
        OnStatusEffectApplied = null;
    }
}
