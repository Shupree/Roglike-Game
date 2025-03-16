using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 개발자용 콘솔 패널 열기
public class On_DevConsole : MonoBehaviour
{
    public GameObject devConsolePanel;

    // 개발자용 콘솔 패널 On/Off
    public void DevConsoleBtn_clicked()
    {
        devConsolePanel.SetActive(!devConsolePanel.activeSelf);
    }
}
