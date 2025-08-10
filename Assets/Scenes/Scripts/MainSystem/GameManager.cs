using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // instance를 통해 static을 통해 메모리에 정보를 저장 후 타 스크립트에서 사용 가능.
    public static GameManager instance;

    [Header ("Reference")]
    public PaintManager paintManager;
    public StageManager stageManager;
    public StorageManager storageManager;

    [HideInInspector] 
    public TurnManager turnManager;
    [HideInInspector] 
    public SpawnManager spawnManager;
    [HideInInspector] 
    public StatusEffectManager statusEffectManager;     // 상태이상 매니저
    [HideInInspector] 
    public HUDPoolManager hudPoolManager;       // HUDPoolManager
    [HideInInspector] 
    public EnemyPoolManager enemyPoolManager;   // EnemyPoolManager
    private GetObject getObject;

    [Header("Unit")]
    public Player player;
    public Ally ally1;
    private int[] enemyIdArr = new int[4]{1,1,0,0};     // 임시 몬스터ID 저장용

    // [Header ("Figure")]
    // public int stage = 1;       // 현재 스테이지 넘버

    void Awake()
    {
        instance = this;    // 인스턴스화

        turnManager = gameObject.GetComponent<TurnManager>();
        spawnManager = gameObject.GetComponent<SpawnManager>();
        statusEffectManager = gameObject.GetComponent<StatusEffectManager>();
        hudPoolManager = gameObject.GetComponent<HUDPoolManager>();
        enemyPoolManager = gameObject.GetComponent<EnemyPoolManager>();
        getObject = gameObject.GetComponent<GetObject>();

        // TurnManager & GetObject 초기화
        turnManager.Initialize();       // TurnManager 초기화 메서드 호출
        getObject.Initialize();    
        hudPoolManager.Initialize();
        enemyPoolManager.Initialize();     
        spawnManager.Initialize();

        // Player HUD 활성화
        GameObject hud = gameObject.GetComponent<HUDPoolManager>().GetHUD();
        if (hud != null)
        {
            hud.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);  // Screen Space Canvas 기준으로 배치
            hud.GetComponent<RectTransform>().position = player.gameObject.transform.position;
            // 플레이어 하단에 HUD 위치
            hud.GetComponent<HUD>().SetHUD(player);
            player.SetHUD(hud);
        }

        // 아군 등록 (Test)
        turnManager.RegisterAlly(player);
        turnManager.RegisterAlly(ally1);

        // 적 등록  (Test)
        spawnManager.SpawnEnemy(enemyIdArr);

        // 턴 진행 시작 (Test)
        turnManager.BattleStart();

        // 스테이지 수 초기화
        // stage = 1;
    }
}