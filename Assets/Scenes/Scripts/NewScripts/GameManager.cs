using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // instance를 통해 static을 통해 메모리에 정보를 저장 후 타 스크립트에서 사용 가능.
    public static GameManager instance;

    [Header ("Reference")]
    public TurnManager turnManager;
    public PaintManager paintManager;
    public Player player;
    public Ally ally1;
    public Enemy enemy1;
    public Enemy enemy2;

    void Awake()
    {
        instance = this;    // 인스턴스화

        // TurnManager 초기화
        turnManager.Initialize(); // TurnManager 초기화 메서드 호출

        // 아군 등록
        turnManager.RegisterAlly(player);
        turnManager.RegisterAlly(ally1);

        // 적 등록
        turnManager.RegisterEnemy(enemy1);
        turnManager.RegisterEnemy(enemy2);

        // 빙결 디버프 테스트
        //player.ApplyFreezeDebuff();
        //enemy1.ApplyFreezeDebuff();

        // 턴 진행 시작
        turnManager.StartTurns();
    }
}