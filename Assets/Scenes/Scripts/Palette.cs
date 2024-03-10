using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Palette : MonoBehaviour
{
    public GameObject[] UIArr = new GameObject[5];
    public Image[] imageComponentArr = new Image[5];

    void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            imageComponentArr[i] = UIArr[i].GetComponent<Image>();
        }
    }

    // 팔레트 이미지 교체
    void ConvertImage()
    {
        
    }
}
