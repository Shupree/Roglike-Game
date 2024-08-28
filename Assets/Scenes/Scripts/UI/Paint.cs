using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Paint : MonoBehaviour
{
    public int maxNum;
    public int currentNum;
    public int usedColorNum;
    public bool canUsePaint = false;
    public GameObject colorUI;
    private Image icon;

    void Awake()
    {
        icon = transform.Find("Color").gameObject.GetComponent<Image>();

        // 최대 페인트 수
        maxNum = 5;
        currentNum = maxNum;
        icon.fillAmount = 1;
        usedColorNum = 0;
    }

    private void FixedUpdate() 
    {
        if (currentNum > maxNum) {
            currentNum = maxNum;
        }
        else if (currentNum < 0) {
            currentNum = 0;
        }
        
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
                    GameManager.instance.AddColor(1, false);
                    break;
                case "BluePaint":
                    GameManager.instance.AddColor(2, false);
                    break;
                case "YellowPaint":
                    GameManager.instance.AddColor(3, false);
                    break;
                case "WhitePaint":
                    GameManager.instance.AddColor(4, false);
                    break;
            }

            usedColorNum += 1;
            currentNum--;
        }
    }

    public void UseThemeSkill(int paintType, int paintNum)
    {
        if (canUsePaint == false) {
            return;
        }
        if (currentNum < paintNum) {
            Debug.Log("해당 페인트가 없어요!");
        }
        else {
            switch (paintType) {
                case 1:
                    GameManager.instance.AddColor(1, true);
                    break;
                case 2:
                    GameManager.instance.AddColor(2, true);
                    break;
                case 3:
                    GameManager.instance.AddColor(3, true);
                    break;
                case 4:
                    GameManager.instance.AddColor(4, true);
                    break;
            }

            usedColorNum += paintNum;
            currentNum -= paintNum;
        }
    }

    // 취소 버튼 클릭 시 페인트 반환
    public void ReturnPaint()
    {
        currentNum += usedColorNum;
        usedColorNum = 0;

        if (maxNum < currentNum) {
            currentNum = maxNum;
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
