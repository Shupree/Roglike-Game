using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float[] enemyAct = new float[2];
    public string effectType;
    public int effectNum;
    public int turn;
    public EnemyData data;
    public float health;
    public float maxHealth;
    public float damage;

    public bool isLive;
    
    void Awake()
    {
        turn = 0;
        maxHealth = data.maxHealth;
        health = maxHealth;
    }

    void FixedUpdate() 
    {
        if(health <= 0)
        {
            Debug.Log("적 처치");

            GameManager.instance.EnemyArr.Remove(gameObject);
            //GameManager.instance.EnemyNum--;
            //isLive = false;
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void TakeActInfo()
    {
        turn++;
        // 몬스터에 따른 공격 방식 선정
        switch(data.enemyId) 
        {
            case 0:
                enemyAct = GetComponent<DummyBot>().Pattern(turn);
                break;
        }

        // 공격(Attack)일 경우
        if(enemyAct[0] == 1)
        {
            damage = enemyAct[1];
        }

        // 효과(Effect)일 경우
        else if(enemyAct[0] == 1)
        {

        }
    }
}
