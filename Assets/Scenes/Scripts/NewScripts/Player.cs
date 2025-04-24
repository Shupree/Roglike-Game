using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ITurn
{
    [Header ("Status")]
    public int maxHealth;
    private int health;   // 체력

    public int canvas;

    [Header ("Status Effect")] 
    public List<StatusEffect> statusEffects = new List<StatusEffect>();   // 상태이상.json

    // 초기화
    void Awake() 
    {
        health = 50;    // 임의 설정값
    }

    /*
    public Player()
    {
        maxHealth = 50;
        health = maxHealth;
    }
    */

    public void TakeTurn()
    {
        foreach (var effect in statusEffects)
        {
            effect.ApplyEffect(this);   // 특정 상태이상 효과 적용

            effect.Duration--;      // 지속 턴 수 감소
            if (effect.Duration <= 0 || effect.StackCount <= 0)     // (지속 시간 = 0 or 중첩 수 = 0)
            {
                effect.RemoveEffect(this);      // 특정 상태이상 제거
            }
        }

        Debug.Log("플레이어가 행동합니다!");
        // 플레이어 행동 로직
    }

    // 특정 상태이상 추가 ( StatusEffect 상태이상 종류, int 중첩 수 )
    public void AddStatusEffect(StatusEffect effect, int stack)
    {
        var existingEffect = statusEffects.Find(e => e.Name == effect.Name);

    if (existingEffect != null)
    {
        existingEffect.StackCount += stack;
        Debug.Log($"{effect.Name} 중첩: {existingEffect.StackCount}");
    }
    else
    {
        effect.StackCount = stack;      // 새로 추가되는 효과는 기본 중첩 = stack 수
        statusEffects.Add(effect);
        Debug.Log($"{effect.Name}이(가) 새로 추가되었습니다.");
    }
    }

    // 체력(HP) 값 확인
    public int GetHP()
    {
        return health;
    }

    // 피격 시 데미지 연산
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        Debug.Log($"플레이어가 {damage} 데미지를 받았습니다! 남은 체력: {health}");
    }

    // 처치 확인
    public bool IsDead()
    {
        return health <= 0;
    }

    // ------ 버프 / 디버프 로직 ------

    // 빙결 디버프 존재 유무 확인
    public bool HasFreezeDebuff()
    {
        return statusEffects.Exists(e => e.Name == "Freeze");
    }

    // 빙결 디버프 제거
    public void RemoveFreezeDebuff()
    {
        var freezeEffect = statusEffects.Find(e => e.Name == "Freeze");
        if (freezeEffect != null)
        {
            freezeEffect.RemoveEffect(this);    // 빙결 디버프 제거
        }
    }
}
