using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyBot : MonoBehaviour
{
    public EnemyData data;
    public float shield;
    public float damage;
    public float[] dataArr;

    void Awake()
    {
        dataArr = new float[2];
    }

    //dataArr[0] = 행동 타입(공격, 효과 등), dataArr[1] = 수치
    public float[] Pattern(int turn) 
    {
        if(0 == turn % 2)
        {
            damage = data.attackDamage_01;
            dataArr[0] = 1f;
            dataArr[1] = damage;
            return dataArr;
        }
        else
        {
            damage = data.attackDamage_01;
            dataArr[0] = 1f;
            dataArr[1] = damage;
            return dataArr;
        }
    }
}
