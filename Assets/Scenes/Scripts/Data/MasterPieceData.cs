using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasterPiece", menuName = "Scriptble Object/MasterPieceData")]
public class MasterPieceData : ScriptableObject
{
    // 걸작 스크립터블 오브젝트
    public enum MP_Class { White, Red, Blue, Yellow, Orange, Purple, Green, Black, Hidden }

    [Header("# Main Info")]
    public MP_Class enemyType;
    public int MP_Id;
    public string MP_Name;
    public string MP_Desc;
    public Sprite MP_Sprite;

    [Header("# Ability")]
    public int cost;
}
