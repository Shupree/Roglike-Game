using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    // 전투 종료 시, 혹은 테마 교체 시 하단의 요소들을 초기화시킬 것.

    public GameObject[] themePrefebArr;
    private GameObject theme;

    public bool onTurnEffect;
    public bool onThemeSkill;
    public bool useThemeSkill;
    public int usedPaintNum;
    public int colorType_FirstSub;

    void Awake()
    {
        onTurnEffect = false;
        onThemeSkill = false;
        useThemeSkill = false;
        usedPaintNum = 0;
        colorType_FirstSub = 0;

        theme = Instantiate(themePrefebArr[0], gameObject.transform.position, Quaternion.identity, gameObject.transform);
        //Theme.transform.parent = gameObject.transform;
    }
    
    void AddThemePrefab()
    {
        //switch 
    }

    void RemoveThemePrefab()
    {
        //switch 
    }
}
