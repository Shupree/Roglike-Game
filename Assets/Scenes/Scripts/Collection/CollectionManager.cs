using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 수집품을 관리하는 싱글톤 클래스
/// </summary>
public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance { get; private set; }

    public List<CollectionData> ownedCollections = new List<CollectionData>();
    public GameObject player; // 효과를 적용할 플레이어 캐릭터

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // GameEventManager에 전투 시작/종료 이벤트 구독
        // 실제 프로젝트에서는 전투를 관리하는 다른 매니저에서 이벤트를 호출해야 합니다.
        GameEventManager.OnCombatStart += HandleCombatStart;
        GameEventManager.OnCombatEnd += HandleCombatEnd;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제 (메모리 누수 방지)
        GameEventManager.OnCombatStart -= HandleCombatStart;
        GameEventManager.OnCombatEnd -= HandleCombatEnd;
    }

    // 전투 시작 시 모든 수집품의 관련 로직을 실행
    private void HandleCombatStart()
    {
        Debug.Log("전투 시작! 수집품 효과를 활성화합니다.");
        ownedCollections.ForEach(c => c.TriggerOnCombatStart(player));
    }

    // 전투 종료 시 모든 수집품의 관련 로직을 실행
    private void HandleCombatEnd()
    {
        Debug.Log("전투 종료! 수집품 효과를 비활성화합니다.");
        ownedCollections.ForEach(c => c.TriggerOnCombatEnd(player));
    }

    /// <summary>
    /// 새로운 수집품을 획득하고 효과를 적용합니다.
    /// </summary>
    /// <param name="newCollection">새로 획득한 수집품</param>
    public void AddCollection(CollectionData newCollection)
    {
        if (newCollection == null || ownedCollections.Contains(newCollection)) return;

        ownedCollections.Add(newCollection);
        newCollection.Equip(player);
        Debug.Log($"수집품 획득: {newCollection.collectionName}");
    }

    /// <summary>
    /// 수집품을 제거하고 효과를 해제합니다.
    /// </summary>
    /// <param name="collectionToRemove">제거할 수집품</param>
    public void RemoveCollection(CollectionData collectionToRemove)
    {
        if (collectionToRemove == null || !ownedCollections.Contains(collectionToRemove)) return;

        collectionToRemove.Unequip(player);
        ownedCollections.Remove(collectionToRemove);
        Debug.Log($"수집품 제거: {collectionToRemove.name}");
    }
}

