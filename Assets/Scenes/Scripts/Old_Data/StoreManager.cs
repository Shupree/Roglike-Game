using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using System.Security.Cryptography;

// 상점 시스템 총괄 매니저
public class StoreManager : MonoBehaviour
{
    [Header("Managers")]
    private Player player;
    private CollectionManager collectionManager;
    private StorageManager storageManager;
    private StageManager stageManager;

    [Header("UI Elements")]
    [Tooltip("상점 UI의 최상위 게임 오브젝트")]
    [SerializeField] private GameObject storeUI;
    [Tooltip("판매 완료 시 표시될 스프라이트")]
    [SerializeField] private Sprite soldOutSprite;

    [Header("Skill Products")]
    [Tooltip("스킬 슬롯 UI 게임 오브젝트 목록 (4개)")]
    [SerializeField] private List<GameObject> skillSlots = new List<GameObject>(4);
    private List<PaintSkillData> skillProducts = new List<PaintSkillData>();
    private List<int> skillPrices = new List<int>();

    [Header("Collection Products")]
    [Tooltip("수집품 슬롯 UI 게임 오브젝트 목록 (3개)")]
    [SerializeField] private List<GameObject> collectionSlots = new List<GameObject>(3);
    private List<CollectionData> collectionProducts = new List<CollectionData>();
    private List<int> collectionPrices = new List<int>();

    [Header("Masterpiece Product")]
    [Tooltip("걸작 슬롯 UI 게임 오브젝트")]
    [SerializeField] private GameObject masterpieceSlot;
    private MasterPieceData masterpieceProduct;
    private int masterpiecePrice = 150;

    // 판매 상태 추적
    private bool[] isSkillSold;
    private bool[] isCollectionSold;
    private bool isMasterpieceSold;

    void Start()
    {
        // GameManager를 통해 참조 가져오기
        if (player == null) player = GameManager.instance.player;
        if (collectionManager == null) collectionManager = GameManager.instance.storageManager.collectionManager;
        if (storageManager == null) storageManager = GameManager.instance.storageManager;
        if (stageManager == null) stageManager = GameManager.instance.stageManager;

        storeUI.SetActive(false);
    }

    /// <summary>
    /// 상점을 열고 상품을 진열합니다. StageManager에서 호출합니다.
    /// </summary>
    public void OpenStore()
    {
        SetUpProducts();
        storeUI.SetActive(true);
    }

    /// <summary>
    /// 상점을 닫고 다음 스테이지로 진행합니다.
    /// </summary>
    public void CloseStore()
    {
        storeUI.SetActive(false);
        stageManager.SetNextStageInfo();        // 다음 스테이지 진행
    }

    private void SetUpProducts()
    {
        // 판매 상태 초기화
        isSkillSold = new bool[skillSlots.Count];
        isCollectionSold = new bool[collectionSlots.Count];
        isMasterpieceSold = false;

        skillProducts.Clear();
        collectionProducts.Clear();

        // 1. 물감 스킬 설정
        for (int i = 0; i < skillSlots.Count; i++)
        {
            // TODO: PaintManager에 색상별로 스킬을 뽑는 기능이 필요하다면 추가해야 합니다.
            // 예: _paintManager.GetRandomPaintSkillByColor(i);
            // 현재는 완전히 무작위로 스킬을 가져옵니다.
            var skill = storageManager.PickRandomSkill((PaintManager.ColorType)i);
            skillProducts.Add(skill);
            UpdateSlotUI(skillSlots[i], skill.icon, skillPrices[i]);
        }

        // 2. 수집품 설정
        // 등급 0 (Common) 수집품을 무작위로 지정 수만큼 가져옵니다.
        collectionProducts = collectionManager.PickRandomCollection(CollectionRarity.Common, collectionSlots.Count);
        for (int i = 0; i < collectionSlots.Count; i++)
        {
            collectionPrices.Add(GetCollectionPrice(collectionProducts[i].rarity));
            UpdateSlotUI(collectionSlots[i], collectionProducts[i].icon, collectionPrices[i]);
        }

        // 3. 걸작 설정
        masterpieceProduct = storageManager.PickRandomMasterPiece();
        UpdateSlotUI(masterpieceSlot, masterpieceProduct.icon, masterpiecePrice);
    }

    private int GetCollectionPrice(CollectionRarity rate)
    {
        switch (rate)
        {
            case CollectionRarity.Common: return 80;
            case CollectionRarity.Rare: return 100;
            case CollectionRarity.Unique: return 130;
            case CollectionRarity.Hidden: return 999;
            default: return 80;
        }
    }

    private void UpdateSlotUI(GameObject slot, Sprite icon, int price)
    {
        if (slot == null) return;
        slot.GetComponent<Image>().sprite = icon;
        slot.transform.GetChild(0).GetComponent<TMP_Text>().text = price.ToString();
        slot.GetComponent<Button>().interactable = true;
    }

    private void MarkAsSold(GameObject slot)
    {
        if (slot == null) return;
        slot.GetComponent<Image>().sprite = soldOutSprite;
        slot.transform.GetChild(0).GetComponent<TMP_Text>().text = "Thanks!";
        slot.GetComponent<Button>().interactable = false;
    }

    // UI Button의 OnClick() 이벤트에 연결할 함수들입니다.
    // 인스펙터에서 각 버튼에 맞는 인덱스를 전달해야 합니다.
    public void BuyPaintSkill(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= skillProducts.Count || isSkillSold[slotIndex]) return;

        if (player.gold >= skillPrices[slotIndex])
        {
            player.gold -= skillPrices[slotIndex];
            storageManager.ConvertSkill(skillProducts[slotIndex]);       // 스킬 교체

            MarkAsSold(skillSlots[slotIndex]);
            isSkillSold[slotIndex] = true;
        }
        else
        {
            Debug.Log("골드가 부족하여 스킬을 구매할 수 없습니다.");
            // TODO: 골드 부족 알림 UI 표시
        }
    }

    public void BuyCollection(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= collectionProducts.Count || isCollectionSold[slotIndex]) return;

        if (player.gold >= collectionPrices[slotIndex])
        {
            player.gold -= collectionPrices[slotIndex];
            collectionManager.AddCollection(collectionProducts[slotIndex]);

            MarkAsSold(collectionSlots[slotIndex]);
            isCollectionSold[slotIndex] = true;
        }
        else
        {
            Debug.Log("골드가 부족하여 수집품을 구매할 수 없습니다.");
            // TODO: 골드 부족 알림 UI 표시
        }
    }

    public void BuyMasterpiece()
    {
        if (isMasterpieceSold) return;

        if (player.gold >= masterpiecePrice)
        {
            player.gold -= masterpiecePrice;
            storageManager.ConvertMasterPiece(masterpieceProduct);

            MarkAsSold(masterpieceSlot);
            isMasterpieceSold = true;
        }
        else
        {
            Debug.Log("골드가 부족하여 걸작을 구매할 수 없습니다.");
            // TODO: 골드 부족 알림 UI 표시
        }
    }

    // 상점 초기화
    public void FormatShop()
    {
        skillProducts.Clear();
        skillPrices.Clear();

        collectionProducts.Clear();
        collectionPrices.Clear();

        masterpieceProduct = null;
        masterpiecePrice = 150;
    }
}
