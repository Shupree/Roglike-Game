using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Scriptable Object/ThemeData")]
public class ThemeData : ScriptableObject
{
    // 테마 스킬_스크립터블 오브젝트

    [Header("# Main Info")]
    public int themeID;         // 테마 ID
    public string themeName;    // 테마 이름
    public string desc;         // 테마 정보
    public Sprite icon;             // 테마 아이콘
    public Sprite charactorSprite;  // 캐릭터 스프라이트

    [Header("# Passive Effect")]
    public string fillName;         // 패시브 효과 파일.json

    [Header("# Skill List")]
    public List<ThemeSkillData> skillList;    // 테마 스킬 리스트
}
