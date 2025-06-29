using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteManager : MonoBehaviour
{
    [Header ("Object reference")]
    public GameObject[] UIArr = new GameObject[5];
    
    [Header ("Image Scripts")]
    public Image[] imageArr = new Image[5];

    [Header ("Figure")]
    private int paletteNum;
    public int limit = 2;       // 최대 페인트 수
    public int order = 0;       // 팔레트 순서
    public int[] paints = new int[5];
    // none = 0, red = 1, blue = 2, yellow = 3, white = 4
    public int stack;           // 물감 사용 스택 수

    void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            imageArr[i] = UIArr[i].GetComponent<Image>();
            imageArr[i].color = Color.gray;
        }
        paints[0] = 0;
        paints[1] = 0;
        paints[2] = 0;
        paints[3] = 0;
        paints[4] = 0;

        paletteNum = 0;
    }

    // 팔레트에 페인트 추가
    public void AddPaint(int num) {
        paints[order] = num;
        order++;
    }

    // 페인트 초기화
    public void ClearPaint() {
        for (int i = 0; i < 5; i++)
        {
            order = 0;
            paints[i] = 0;
        }
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
            // 하양
            case 4:
                imageArr[paletteNum].color = Color.white;
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
            imageArr[i].color = Color.gray;
        }

        paletteNum = 0;
    }
}
