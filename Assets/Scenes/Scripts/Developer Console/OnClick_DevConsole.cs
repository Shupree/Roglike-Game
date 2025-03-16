using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 편의성을 위한 개발자용 콘솔
public class OnClick_DevConsole : MonoBehaviour
{
    public GameObject battleStartBtn;
    public GameObject TPStoreBtn;
    public GameObject TPChestRoomBtn;
    public GameObject resetGameBtn;

    public void Click_BattleStartBtn()
    {
        // 전투 시작 코드
        gameObject.SetActive(false);    // 패널 Off
    }

    public void Click_TPStoreBtn()
    {
        // 상점 이동 코드
        gameObject.SetActive(false);    // 패널 Off
    }

    public void Click_TPChestRoomBtn_clicked()
    {
        // 상자방 이동 코드
        gameObject.SetActive(false);    // 패널 Off
    }

    public void Click_ResetGameBtn_clicked()
    {
        // 게임 리셋 코드
        gameObject.SetActive(false);    // 패널 Off
    }
}
