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
        dataArr = new int[3];
    }

    //dataArr[] 0:공격력, 1:효과타입 2:효과수치
    // 00없음, 01화상, 02중독, 03감전, 04추위, 05빙결
    // 11회복 12보호막 13집중
    public int[] SetPattern(int turn) 
    {
        // 홀수 턴 _ 뼈다귀치기(10공격)
        if(0 != turn % 2)
        {
            dataArr[0] = data.attackDamage_01;
            dataArr[1] = data.attackEffect_01;
            dataArr[2] = data.effectNum_01;
            return dataArr;
        }
        // 짝수 턴 _ 재구성(2회복)
        else
        {
            dataArr[0] = data.attackDamage_02;
            dataArr[1] = data.attackEffect_02;
            dataArr[2] = data.effectNum_02;
            return dataArr;
        }
    }
}
