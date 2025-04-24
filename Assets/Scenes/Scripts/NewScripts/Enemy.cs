using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITurn
{
    [Header ("Reference")]
    private SpriteRenderer targetSpriteRenderer;

    [Header ("Enemy Data")]
    public EnemyData data;

    [Header ("Status")]
    private int health;     // 체력

    [Header ("Status Effect")] 
    public List<StatusEffect> statusEffects = new List<StatusEffect>();   // 상태이상.json 불러오기

    void Awake() {
        health = 50;    // 임의 설정값
    }

    /*
    public Enemy(int initialHealth)
    {
        health = initialHealth;
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
        
        Debug.Log("적이 행동합니다!");
        // 적 행동 로직
    }

    public bool HasFreezeDebuff()
    {
        return statusEffects.Exists(e => e.Name == "Freeze");
    }

    public void RemoveFreezeDebuff()
    {
        var freezeEffect = statusEffects.Find(e => e.Name == "Freeze");
        if (freezeEffect != null)
        {
            freezeEffect.RemoveEffect(this);    // 빙결 디버프 제거
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
        Debug.Log($"적이 {damage} 데미지를 받았습니다! 남은 체력: {health}");
    }

    // 처치 확인
    public bool IsDead()
    {
        return health <= 0;
    }
}
