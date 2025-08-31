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
    public List<ColorType> usedPaints = new List<ColorType>();  // 사용 중인 색상별 물감 수

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

        usedPaints.Clear();

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
            // 처음 페인트 추가 시 스킬 지정
            if (player.actionType == ActionType.none)
            {
                player.actionType = ActionType.paintSkill;
                player.currentSkill = player.storageManager.GetSkillData(paintSc.colorType);   // 스킬 정보 가져오기
                Debug.Log(player.currentSkill.name);
                player.SetTarget(player.currentSkill.skillType, player.currentSkill.count);

                SetSkillImg(player.currentSkill.icon);         // 메인 스킬 이미지 변경
            }

            ColorInPalette(paintSc.colorType);          // 팔레트에 페인트 추가
            usedPaints.Add(paintSc.colorType);          // 사용 중인 페인트 정보 저장

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
        // 사용한 물감 되돌리기
        foreach (var paintType in usedPaints)
        {
            paintScArr[(int)paintType].FillNum(1);
        }

        usedPaints.Clear();     // 사용한 물감 초기화
    }

    // 특정 개수만큼 사용한 물감 반환 (먼저 사용한 순)
    public void ReturnPartialPaint(int amount)
    {
        if (amount <= 0) return;

        // 반환할 물감 수는 사용한 물감 수를 초과할 수 없음
        int totalUsed = paletteOrder;
        int amountToReturn = Mathf.Min(amount, totalUsed);

        // 지정된 수만큼 물감 반환
        for (int i = 0; i < amountToReturn; i++)
        {
            if (usedPaints.Count == 0) break;

            paintScArr[(int)usedPaints[0]].FillNum(1);      // 물감 개수 복구
            usedPaints.RemoveAt(0);                         // 사용한 물감 배열에서 제거
        }

        // 반환되어도 걸작 스택 증가됨.
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
        usedPaints.Clear();     // 사용 중인 물감 초기화

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
