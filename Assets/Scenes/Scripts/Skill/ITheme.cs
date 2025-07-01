using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ITheme
{
    // ThemeData GetThemeData();
    ThemeData ApplyTheme(ThemeManager themeManager);
    void ApplyBattleEvent(string situation);
}

// 두 개의 특성을 다루는 기본 테마 : 아티스트
public class Artist : ITheme
{
    public ThemeData themeData;     // 고유 테마 데이터
    private TurnManager turnManager;
    private ThemeManager themeManager;

    private int usedPaintStack;         // 사용한 물감 스택 (3스택 => 1무채색)
    private int stackLimit;             // 턴당 스택 제한 (턴당 3무채색)

    public ThemeData ApplyTheme(ThemeManager managerSc)
    {
        turnManager = GameManager.instance.turnManager;

        themeManager = managerSc;           // ThemeManager 스크립트 받기
        usedPaintStack = 0;

        return themeData;
    }

    // 배틀 이벤트 적용
    public void ApplyBattleEvent(string situation)
    {
        switch (situation)
        {
            case "OnStartTurn":
                ClearStackLimit();
                break;
            case "OnAttack":
                ConvertStack();
                break;
        }
    }

    // 턴 시작 시, 얻을 수 있는 '무채색' 수 초기화 (턴 시작 시)
    public void ClearStackLimit()
    {
        stackLimit = 3;
    }

    // 사용한 물감 수 증감 함수 (플레이어가 공격 시)
    public void ConvertStack()
    {
        int usedPaintNum = turnManager.usedPaintNum;        // 사용한 물감 수 가져오기
        StatusEffect passiveEffect = themeManager.passiveEffects.Find(e => e.nameEn == "AchromaticColor");

        if (stackLimit > 0 && passiveEffect.maxStack > passiveEffect.stackCount)
        {
            usedPaintStack += usedPaintNum;         // 스택에 추가
        }

        while (usedPaintStack >= 3 && stackLimit > 0 && passiveEffect.maxStack > passiveEffect.stackCount)
        {
            themeManager.CalculatePassiveEffect("AchromaticColor", 1);
            usedPaintStack -= 3;
            stackLimit--;
        }

        if (stackLimit <= 0 && passiveEffect.maxStack <= passiveEffect.stackCount)
        {
            usedPaintStack = 0;         // 스택 제한 시, 스택 초기화
        }
    }
}

/*
// 매 상황에 맞는 카드로 지속적 전투에 유리한 테마 : 점성술사 
public class FortuneTeller : ITheme
{
    public ThemeData themeData;     // 고유 테마 데이터

    public ThemeData GetThemeData()
    {
        return themeData;
    }
}
*/