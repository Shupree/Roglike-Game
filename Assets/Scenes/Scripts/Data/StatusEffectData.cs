using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Scriptable Object/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    // 상태이상_스크립터블 오브젝트
    public enum EffectLogicType     // 상태이상 로직
    {
        bleed, block, concentration, frozen, weak
    }

    public enum effectType          // 상태이상 종류
    { 
        DoT, CC, buff, themePassive
    }

    [Header("기본 정보")]
    public EffectLogicType logicType;
    public string effectName;
    [TextArea] public string description;
    public Sprite icon;


    [Header("스택 및 지속시간")]
    public int maxStack;
    public bool haveDuration;
    public int duration;
}
