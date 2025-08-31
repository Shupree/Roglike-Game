using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 테스트용 수집품 추가보다 전투 시작 시점이 더 빠른 문제 발생

/// <summary>
/// 플레이어의 수집품을 관리하는 싱글톤 클래스
/// </summary>
public class CollectionManager : MonoBehaviour
{
    // 모든 수집품 Data
    public List<CollectionData> collectionDatas = new List<CollectionData>();
    // 현재 플레이어가 소유한 수집품 Data
    public List<CollectionData> ownedCollections = new List<CollectionData>();
    public Player player; // 효과를 적용할 플레이어 캐릭터

    public void Initialize()
    {
        AddCollection(collectionDatas[0]);
        Debug.Log("테스트용 수집품 추가 @@@");
        // player = GameManager.instance.player;
    }

    private void Start()
    {
        // GameEventManager에 전투 시작/종료 이벤트 구독
        // 실제 프로젝트에서는 전투를 관리하는 다른 매니저에서 이벤트를 호출해야 합니다.
        BattleEventManager.OnBattleStart += HandleBattleStart;
        BattleEventManager.OnBattleEnd += HandleBattleEnd;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제 (메모리 누수 방지)
        BattleEventManager.OnBattleStart -= HandleBattleStart;
        BattleEventManager.OnBattleEnd -= HandleBattleEnd;
    }

    // 전투 시작 시 모든 수집품의 관련 로직을 실행
    private void HandleBattleStart()
    {
        Debug.Log("전투 시작! 수집품 효과를 활성화합니다.");
        ownedCollections.ForEach(c => c.TriggerOnBattleStart(player));
    }

    // 전투 종료 시 모든 수집품의 관련 로직을 실행
    private void HandleBattleEnd()
    {
        Debug.Log("전투 종료! 수집품 효과를 비활성화합니다.");
        ownedCollections.ForEach(c => c.TriggerOnBattleEnd(player));
    }

    /// <summary>
    /// 새로운 수집품을 획득하고 효과를 적용합니다.
    /// </summary>
    /// <param name="newCollection">새로 획득한 수집품</param>
    public void AddCollection(CollectionData newCollection)
    {
        if (newCollection == null || ownedCollections.Contains(newCollection)) return;

        ownedCollections.Add(newCollection);
        newCollection.Equip(player.gameObject);
        Debug.Log($"수집품 획득: {newCollection.collectionName}");
    }

    /// <summary>
    /// 수집품을 제거하고 효과를 해제합니다.
    /// </summary>
    /// <param name="collectionToRemove">제거할 수집품</param>
    public void RemoveCollection(CollectionData collectionToRemove)
    {
        if (collectionToRemove == null || !ownedCollections.Contains(collectionToRemove)) return;

        collectionToRemove.Unequip(player.gameObject);
        ownedCollections.Remove(collectionToRemove);
        Debug.Log($"수집품 제거: {collectionToRemove.collectionName}");
    }

    // 랜덤 수집품 정보 추출
    public List<CollectionData> PickRandomCollection(CollectionRarity rarity, int num)
    {
        List<CollectionData> collectionList = collectionDatas
            .Where(collection =>
                collection != null &&
                collection.rarity == rarity &&
                !ownedCollections.Any(playerCollection => playerCollection.name == collection.name)            // 두 번째 조건 : 중복된 스킬은 제외할 것
            )
            .ToList();      // 중복 스킬을 제외한 리스트 제작

        // 추첨가능한 수집품 수가 충분한 경우
        if (collectionList.Count > num)
        {
            List<CollectionData> randomDatas = new List<CollectionData>();      // 추첨한 수집품 데이터 보관소

            for (int i = 0; i < num; i++)
            {
                int randomNum = Random.Range(0, collectionList.Count);    // 랜덤 색상 1개 추첨
                if (randomDatas.Contains(collectionList[randomNum]))
                {
                    i--;        // 이미 존재 시, 재추첨
                }
                else
                {
                    randomDatas.Add(collectionList[randomNum]);     // 추첨한 수집품 추가
                }
            }

            return randomDatas;
        }
        // 추첨가능한 수집품 수가 부족한 경우 (부족한 수만큼 null 로 대체)
        else
        {
            Debug.Log("추첨할 수집품 수가 부족함.");
            while (collectionList.Count < num)
            {
                collectionList.Add(null);
            }

            return collectionList;
        }
    }
}

