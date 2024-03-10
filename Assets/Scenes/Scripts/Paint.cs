using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Paint : MonoBehaviour
{
    public
    //Image icon;

    void Awake()
    {
        // 컴포넌트의 모든 구성요소 가져오기 (배열 중 2번째 가져오기[첫 번째는 자기 자신임.])
        //icon = GetComponentsInChildren<Image>()[1]
        //icon.sprite = data.itemIcon;
    }

    // 페인트 선택
    public void SelectPaintBtn()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        switch (clickObject.name) {
            case "RedPaint":
                GameManager.instance._PaintManager.AddPaint(1);
                Debug.Log("OOK");
                break;
            case "BluePaint":
                GameManager.instance._PaintManager.AddPaint(2);
                break;
            case "YellowPaint":
                GameManager.instance._PaintManager.AddPaint(3);
                break;
            case "BlackPaint":
                GameManager.instance._PaintManager.AddPaint(4);
                break;
        }
    }
}
