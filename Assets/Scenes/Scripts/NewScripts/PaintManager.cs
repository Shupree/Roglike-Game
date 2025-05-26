using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class PaintManager : MonoBehaviour
{
    [Header ("Reference")]
    private TurnManager turnManager;
    private Player player;

    // public GameObject[] paintObjArr = new GameObject[4];    // 색상별 PaintUI
    public GameObject[] paletteObjArr = new GameObject[5];      // 팔레트별 PaletteUI

    [Header ("Figure")]
    public int[] usedPaintArr = new int[4];     // 사용 중인 색상별 물감 수
    
    public int maxPalette;      // 사용가능한 최대 팔레트 수
    public int paletteOrder = 0;       // 현재 팔레트 순서

    public int stack = 0;       // 걸작 사용을 위한 사용한 물감 수

    [Header ("Image Script")]
    private Image[] paletteImgArr = new Image[5];       // 팔레트별 PaletteImage 스크립트

    public Image skillIconUI;                         // 메인 스킬 Image 스크립트

    [Header("Paint Script")]
    public Paint[] paintScArr = new Paint[4];      // 색상별 Paint 스크립트

    [Header ("Others")]
    public bool canUsePaint = false;            // 공격 가능할때만 버튼 On용 장치

    void Awake()
    {
        // 초기화
        turnManager = GameManager.instance.turnManager;
        player = GameManager.instance.player;

        for(int i = 0; i < 4; i++) {
            usedPaintArr[i] = 0;
            // paintScArr[i] = paintObjArr[i].GetComponent<Paint>();
        }
        
        for(int i = 0; i < 5; i++) {
            paletteImgArr[i] = paletteObjArr[i].GetComponent<Image>();
        }

        paletteOrder = 0; 
        maxPalette = 3;     // 임의 설정값 : 사용가능한 팔레트 수

        //canUsePaint = false;

        // 델리게이트 이벤트 구독
        turnManager.OnBattleStarted += FillUpPaint;
        turnManager.OnTurnStarted += FillPaint;
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

        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

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
            int colorType = 0;
            switch (clickObject.name)
            {
                // 빨강 페인트 추가
                case "RedPaint":
                    colorType = 1;
                    break;
                // 파랑 페인트 추가
                case "BluePaint":
                    colorType = 2;
                    break;
                // 노랑 페인트 추가
                case "YellowPaint":
                    colorType = 3;
                    break;
                // 하양 페인트 추가
                case "WhitePaint":
                    colorType = 4;
                    break;
                default:
                    Debug.LogError("페인트를 추가하는 과정에서 문제 발생!");
                    break;
            }

            if (paletteOrder == 0)
            {            // 처음 페인트 추가 시 스킬 지정
                player.mainSkill = player.storageManager.GetSkillData(colorType - 1);   // 스킬 정보 가져오기
                Debug.Log(player.mainSkill.name);
                switch (player.mainSkill.skillType)
                {
                    case Skill.SkillType.SingleAtk:
                        turnManager.targets.Add(turnManager.enemies[0]);
                        break;
                    case Skill.SkillType.SplashAtk:
                        turnManager.targets = new List<ITurn> (turnManager.enemies);
                        break;
                    case Skill.SkillType.SingleSup:
                        turnManager.targets.Add(player);
                        break;
                    case Skill.SkillType.SplashSup:
                        turnManager.targets = new List<ITurn> (turnManager.allies);
                        break;
                }
                
                skillIconUI.sprite = Resources.Load<Sprite>(player.mainSkill.icon);   // 메인 스킬 이미지 변경
            }
            
            ColorInPalette(colorType);          // 팔레트에 페인트 추가
            usedPaintArr[colorType - 1]++;      // 사용 중인 페인트 수 증가

            paintSc.paint--;    // 페인트 수 감소
        }
    }

    /*
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
    */

    // 취소 버튼 클릭 시 페인트 반환
    public void ReturnPaint()
    {
        for (int i = 0; i < paintScArr.Length; i++) {
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
            if (currentNum < maxNum - 2) {
                paint.FillNum(maxNum / 2);
            }
            else {
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
    public void ColorInPalette(int colorType)
    {
        switch (colorType) {
            // 빨강
            case 1:
                paletteImgArr[paletteOrder].color = Color.red;
                break;
            // 파랑
            case 2:
                paletteImgArr[paletteOrder].color = Color.blue;
                break;
            // 노랑
            case 3:
                paletteImgArr[paletteOrder].color = Color.yellow;
                break;
            // 하양
            case 4:
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
}
