using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITheme
{
    ThemeData GetThemeData();
}

// 두 개의 특성을 다루는 기본 테마 : 아티스트
public class Artist : ITheme
{
    public ThemeData themeData;     // 고유 테마 데이터

    public ThemeData GetThemeData()
    {
        return themeData;
    }
}

// 매 상황에 맞는 카드로 지속적 전투에 유리한 테마 : 점성술사 
public class FortuneTeller : ITheme
{
    public ThemeData themeData;     // 고유 테마 데이터

    public ThemeData GetThemeData()
    {
        return themeData;
    }
}