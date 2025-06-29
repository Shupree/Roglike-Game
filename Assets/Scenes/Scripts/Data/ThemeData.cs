using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Scriptble Object/ThemeData")]
public class ThemeData : ScriptableObject
{
    // 테마 스킬_스크립터블 오브젝트

    [Header("# Main Info")]
    public int themeID;         // 테마 ID
    public string themeName;    // 테마 이름
    public string desc;         // 테마 정보
    public Sprite sprite;       // 테마 스프라이트

    [Header("# Passive Effect")]
    public string effectName;   // 패시브 이름
    public string effectDesc;   // 패시브 정보
    public Sprite[] effectIcons;    // 패시브 아이콘 Array
    public int maxStack;        // 패시브 최대 스택 수
    public int stack;           // 현재 스택 수


    [Header("# Skill Pattern")]
    public List<ThemeSkillData> skillPatterns;    // 테마 스킬 리스트
}
