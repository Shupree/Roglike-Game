using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyBot : MonoBehaviour
{
    public EnemyData data;
    public int[] dataArr;

    void Awake()
    {
        dataArr = new int[5];
    }

    //dataArr[] 0:공격력, 1:효과타입 2:효과수치
    // 00없음, 01화상, 02중독, 03감전, 04추위, 05빙결, 06기절, 07공포, 08위압, 09부식
    // 11회복 12보호막 13집중
    public int[] SetPattern(int turn) 
    {
        // 홀수 턴 _ 뼈다귀치기(6공격, 4중독)
        if(0 != turn % 2)
        {
            dataArr[0] = data.damage_01;
            dataArr[1] = data.shield_01;
            dataArr[2] = data.effectType_01;
            dataArr[3] = data.effectNum_01;
            dataArr[4] = data.heal_01;
            return dataArr;
        }
        // 짝수 턴 _ 재구성(4회복)
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
