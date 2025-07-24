using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class PaintManager : MonoBehaviour
{
    public enum ColorType
    {
        red, blue, yellow, white
    }

    [Header("Reference")]
    private TurnManager turnManager;
    private Player player;

    public GameObject[] paletteObjArr = new GameObject[5];      // 팔레트별 PaletteUI

    [Header("Figure")]
    public int[] usedPaintArr = new int[4];     // 사용 중인 색상별 물감 수

    public int maxPalette;      // 사용가능한 최대 팔레트 수
    public int paletteOrder = 0;       // 현재 팔레트 순서

    [Header("Image Script")]
    private Image[] paletteImgArr = new Image[5];       // 팔레트별 PaletteImage 스크립트

    public Image skillIconUI;                         // 메인 스킬 Image 스크립트

    [Header("Paint Script")]
    public Paint[] paintScArr = new Paint[4];      // 색상별 Paint 스크립트

    [Header("Others")]
    public bool canUsePaint = false;            // 공격 가능할때만 버튼 On용 장치

    void Awake()
    {
        // 초기화
        turnManager = GameManager.instance.turnManager;
        player = GameManager.instance.player;

        for (int i = 0; i < 4; i++)
        {
            usedPaintArr[i] = 0;
            // paintScArr[i] = paintObjArr[i].GetComponent<Paint>();
        }

        for (int i = 0; i < 5; i++)
        {
            paletteImgArr[i] = paletteObjArr[i].GetComponent<Image>();
        }

        paletteOrder = 0;
        maxPalette = 3;     // 임의 설정값 : 사용가능한 팔레트 수

        //canUsePaint = false;
    }

    // ---- Paint 관련 함수 ----

    // 페인트 선택
    public void ClickPaintBtn(Paint paintSc)
    {
        // 공격 가능할때만 버튼 On
        if (canUsePaint == false)
        {
            return;
        }

        Debug.Log("물감 클릭");

        if (paintSc.GetNum() == 0)
        {
            Debug.Log("해당 페인트가 없어요!");
        }
        else if (paletteOrder > maxPalette)
        {
            Debug.Log("이미 팔레트가 꽉 찼어!");
        }
        else
        {
            ColorType colorType = ColorType.red;
            switch (paintSc.gameObject.name)
            {
                // 빨강 페인트 추가
                case "RedPaint":
                    colorType = ColorType.red;
                    break;
                // 파랑 페인트 추가
                case "BluePaint":
                    colorType = ColorType.blue;
                    break;
                // 노랑 페인트 추가
                case "YellowPaint":
                    colorType = ColorType.yellow;
                    break;
                // 하양 페인트 추가
                case "WhitePaint":
                    colorType = ColorType.white;
                    break;
                default:
                    Debug.LogError("페인트를 추가하는 과정에서 문제 발생!");
                    break;
            }

            if (paletteOrder == 0 && player.themeSkill == null)
            {            // 처음 페인트 추가 시 스킬 지정
                player.mainSkill = player.storageManager.GetSkillData(colorType);   // 스킬 정보 가져오기
                Debug.Log(player.mainSkill.name);
                turnManager.SetTarget(player.mainSkill.skillType);

                SetSkillImg(Resources.Load<Sprite>("Sprite/Skill_Sprite/"+player.mainSkill.icon));         // 메인 스킬 이미지 변경
            }

            ColorInPalette(colorType);          // 팔레트에 페인트 추가
            usedPaintArr[(int)colorType]++;      // 사용 중인 페인트 수 증가

            paintSc.paint--;    // 페인트 수 감소
        }
    }

    // 특정 페인트 수를 가져오기
    public int GetPaintInfo(int colorType)              // colorNum : 1.빨강, 2.파랑, 3.노랑, 4.하양
    {
        return paintScArr[colorType].paint;
    }

    // 특정 페인트를 강제 소모
    public void ReducePaint(int colorType, int num)       // colorNum : 1.빨강, 2.파랑, 3.노랑, 4.하양 / num : 소모 수
    {
        if (paintScArr[colorType].paint >= num)
        {
            paintScArr[colorType].paint -= num;
        }
        // 예외 처리(음수)
        else
        {
            paintScArr[colorType].paint = 0;
        }
    }

    // 취소 버튼 클릭 시 페인트 반환
    public void ReturnPaint()
    {
        for (int i = 0; i < paintScArr.Length; i++)
        {
            paintScArr[i].FillNum(usedPaintArr[i]);     // 사용한 물감 다시 보충
            usedPaintArr[i] = 0;                // 초기화
        }
    }

    // 페인트 보충
    public void FillPaint()
    {
        foreach (var paint in paintScArr)
        {
            // 최대 페인트 수, 현재 페인트 수 가져오기
            int maxNum = paint.GetMaxNum();
            int currentNum = paint.GetNum();

            // 최대치의 절반만큼 보충 (최소 2)
            if (currentNum < maxNum - 2)
            {
                paint.FillNum(maxNum / 2);
            }
            else
            {
                paint.FillUp();
            }
        }
    }

    // 페인트 최대로 보충
    public void FillUpPaint()
    {
        // 각 색상별 물감 최대로 보충
        foreach (var paint in paintScArr)
        {
            paint.FillUp();
        }
    }

    // ---- Palette 관련 함수 ----

    public void ColorInPalette(ColorType colorType)
    {
        switch (colorType)
        {
            // 빨강
            case ColorType.red:
                paletteImgArr[paletteOrder].color = Color.red;
                break;
            // 파랑
            case ColorType.blue:
                paletteImgArr[paletteOrder].color = Color.blue;
                break;
            // 노랑
            case ColorType.yellow:
                paletteImgArr[paletteOrder].color = Color.yellow;
                break;
            // 하양
            case ColorType.white:
                paletteImgArr[paletteOrder].color = Color.white;
                break;
            default:
                Debug.Log("Palette 시스템에 문제가 발생했습니다.");
                break;
        }
        paletteOrder++;     // 팔레트 순서++
    }

    // 페인트 & 팔레트 초기화
    public void ClearPaint()
    {
        paletteOrder = 0;       // 팔레트 순서 초기화
        Array.Clear(usedPaintArr, 0, 4);    // 사용 중인 물감 초기화

        // 팔레트 이미지 초기화
        for (int i = 0; i < 5; i++)
        {
            paletteImgArr[i].color = Color.gray;
        }

        skillIconUI.sprite = Resources.Load<Sprite>($"Icons/Canvas");   // 메인 스킬 이미지 기본으로 변경
    }

    // ---- CanvasUI 관련 함수 ----

    // 메인 스킬 이미지 변경
    public void SetSkillImg(Sprite sprite)
    {
        skillIconUI.sprite = sprite;
    }
}
