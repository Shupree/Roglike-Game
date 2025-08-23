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
/// 게임의 주요 이벤트를 관리하는 정적 클래스
/// </summary>
public static class GameEventManager
{
    // 전투 관련 이벤트
    public static event Action OnCombatStart;       // 전투 시작
    public static event Action OnCombatEnd;         // 전투 종료
    public static event Action OnPlayerTurnStart;   // 플레이어 턴 시작
    public static event Action OnPlayerTurnEnd;     // 플레이어 턴 종료

    // 플레이어 행동 관련 이벤트
    public static event Action<SkillData> OnSkillPlayed;
    public static event Action<ResourceType, int> OnResourceUsed;

    // 캐릭터 상태 관련 이벤트
    public static event Action<Character, StatusEffectType> OnStatusEffectApplied;

    // --- 이벤트 호출 메서드 (게임의 다른 곳에서 호출됨) ---
    public static void TriggerCombatStart() => OnCombatStart?.Invoke();             // 전투 시작
    public static void TriggerCombatEnd() => OnCombatEnd?.Invoke();                 // 전투 종료
    public static void TriggerPlayerTurnStart() => OnPlayerTurnStart?.Invoke();     // 플레이어 턴 시작
    public static void TriggerPlayerTurnEnd() => OnPlayerTurnEnd?.Invoke();         // 플레이어 턴 종료
    public static void TriggerSKillPlayed(SkillData card) => OnSkillPlayed?.Invoke(card);       // 플레이어 스킬 사용
    public static void TriggerResourceUsed(ResourceType type, int amount) => OnResourceUsed?.Invoke(type, amount);      // 특정 자원 소모
    public static void TriggerStatusEffectApplied(Character target, StatusEffectType effect) => OnStatusEffectApplied?.Invoke(target, effect);      // 특정 상태이상 발동
}

