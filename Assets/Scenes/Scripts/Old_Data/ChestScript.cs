using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    /*
    public enum ChestRate { Common, Rare, Unique, Cursed, Random, Special }
    [Header ("Reference")]
    private LootUI _LootManager;

    [Header ("Sprite")]
    public Sprite openedUI;     // 열린 상자 UI

    [Header ("Chest Rate")]
    public ChestRate chestRate;

    [Header ("Loot")]
    public int gold;            // 지급 골드
    public int artifactId;      // 지급할 장신구 ID : -10.랜덤, -9.Normal랜덤, -8.Rare랜덤, -7.Unique랜덤, -6.Cursed랜덤, 정수.장신구ID

    [Header ("Others")]
    private bool isOpened; 

    void Awake()
    {
        _LootManager = GameManager.instance._LootManager;

        isOpened = false;
    }

    public void OpenChest()
    {
        if (isOpened) {
            return;
        }
        else {
            isOpened = true;
        }

        // 스프라이트 교체
        GetComponent<SpriteRenderer>().sprite = openedUI;

        // 전리품UI On
        _LootManager.gameObject.SetActive(true);

        // Reward(리워드) UI 출력 -> Reward 수여
        _LootManager.SetLootUI(1, gold);
        _LootManager.SetLootUI(3, artifactId);

        Destroy(gameObject);      // 임시로 NPC 제거 (스테이지 넘어가는 연출 + NPC 제거하면 좋을 듯)
    }
    */
}
