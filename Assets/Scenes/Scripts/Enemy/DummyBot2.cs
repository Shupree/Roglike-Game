using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyBot2 : MonoBehaviour
{
    public EnemyData data;
    public int[] dataArr;

    void Awake()
    {
        dataArr = new int[5];
    }

    //dataArr[0] = 행동 타입(공격, 효과 등), dataArr[1] = 수치
    public int[] SetPattern(int turn) 
    {
        // 홀수 턴 _ 휘두르기(5공격)
        if(0 != turn % 2)
        {
            dataArr[0] = data.damage_01;
            dataArr[1] = data.shield_01;
            dataArr[2] = data.effectType_01;
            dataArr[3] = data.effectNum_01;
            dataArr[4] = data.heal_01;
            return dataArr;
        }
        // 짝수 턴 _ 신체 강화(4감전)
        else
        {
            dataArr[0] = data.damage_02;
            dataArr[1] = data.shield_02;
            dataArr[2] = data.effectType_02;
            dataArr[3] = data.effectNum_02;
            dataArr[4] = data.heal_02;
            return dataArr;
        }
    }
}
