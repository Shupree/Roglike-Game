using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Paint : MonoBehaviour
{
    public int maxNum;
    public int currentNum;
    public bool canUsePaint = false;
    public GameObject colorUI;
    Image icon;

    void Awake()
    {
        icon = transform.Find("Color").gameObject.GetComponent<Image>();

        // 최대 페인트 수
        maxNum = 5;
        currentNum = maxNum;
        icon.fillAmount = 1;
    }

    private void FixedUpdate() 
    {
        icon.fillAmount = currentNum / (float)maxNum;
    }

    // 페인트 선택
    public void ClickPaintBtn()
    {
        if (canUsePaint == false) {
            return;
        }
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        if (currentNum == 0) {
            Debug.Log("해당 페인트가 없어요!");
        }
        else if (GameManager.instance._PaintManager.order > GameManager.instance._PaintManager.limit) {
            Debug.Log("이미 팔레트가 꽉 찼어!");
        }
        else {
            switch (clickObject.name) {
                case "RedPaint":
                    GameManager.instance.AddColor(1);
                    break;
                case "BluePaint":
                    GameManager.instance.AddColor(2);
                    break;
                case "YellowPaint":
                    GameManager.instance.AddColor(3);
                    break;
                case "BlackPaint":
                    GameManager.instance.AddColor(4);
                    break;
            }
            currentNum--;
        }
    }

    // 페인트 보충
    public void FillPaint()
    {
        // 최소 2씩 보충
        if (currentNum < maxNum - 2) {
            currentNum += maxNum / 2;
        }
        else if (currentNum == maxNum - 2) {
            currentNum = maxNum;
        }
    }

    // 페인트 최대로 보충
    public void FillUpPaint()
    {
        currentNum = maxNum;
    }
}
