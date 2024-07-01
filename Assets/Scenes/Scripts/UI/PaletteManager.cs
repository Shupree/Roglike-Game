using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteManager : MonoBehaviour
{
    public GameObject[] UIArr = new GameObject[5];
    public Image[] imageArr = new Image[5];
    private int paletteNum;

    void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            imageArr[i] = UIArr[i].GetComponent<Image>();
            imageArr[i].color = Color.white;
        }

        paletteNum = 0;
    }

    // 팔레트 이미지 교체
    public void ConvertSprite(int colorType)
    {
        switch (colorType) {
            // 빨강
            case 1:
                imageArr[paletteNum].color = Color.red;
                paletteNum++;
                break;
            // 파랑
            case 2:
                imageArr[paletteNum].color = Color.blue;
                paletteNum++;
                break;
            // 노랑
            case 3:
                imageArr[paletteNum].color = Color.yellow;
                paletteNum++;
                break;
            default:
                Debug.Log("Palette 시스템에 문제가 발생했습니다.");
                break;
        }
    }

    // 팔레트 초기화
    public void ClearPalette()
    {  
        // 팔레트 이미지 초기화
        for (int i = 0; i < 5; i++)
        {
            imageArr[i].color = Color.white;
        }

        paletteNum = 0;
    }
}
