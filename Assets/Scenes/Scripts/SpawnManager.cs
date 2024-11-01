using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header ("Position")]
    public GameObject[] position;
    public GameObject position_Outside;

    [Header ("Prefabs")]
    public GameObject[] enemyPrefabArr;     // enemy Prefab Array
    private GameObject enemyPrefab;

    [Space (10f)]
    public GameObject[] NPCPrefabArr;       // NPC Prefab Array

    [Space (10f)]
    public GameObject[] chestPrefabArr;    // chest Prefab Array
    
    private GameObject enemy;

    void Awake()
    {
        
    }

    public void EnemySpawn(int[] EnemyID)
    {
        for (int i = 0; i < 4; i++)
        {
            if (EnemyID[i] == 0) {
                // 적이 존재하지 않음.
            } 
            else {
                // 해당 적 정보 수집    (주의. 'enemyID - 1와 'enemyPrefebArr'의 순서는 같아야 함.)
                enemyPrefab = enemyPrefabArr[EnemyID[i] - 1];

                enemy = Instantiate(enemyPrefab, position[i].transform.position, Quaternion.Euler(0, 0, 0));
                enemy.transform.parent = position[i].transform;
                enemy.name = enemy.GetComponent<Enemy>().data.enemyName;
                GameManager.instance.EnemyList.Add(enemy);
            }
        }
    }

    public void StoreNPCSpawn() 
    {
        GameObject NPC;
        NPC = Instantiate(NPCPrefabArr[0], position[2].transform.position, Quaternion.Euler(0, 0, 0));
        NPC.transform.parent = position[2].transform;
        NPC.name = "StoreNPC";
        GameManager.instance._StoreManager.storeNPC = NPC;
    }

    // 상자 등급 : 1.Common 2.Rare 3.Unique 4.Cursed 5.Random
    //          6 ~.Special
    public void TreasureChestSpawn(int rate)
    {
        GameObject chest;
        chest = Instantiate(chestPrefabArr[rate - 1], position[2].transform.position, Quaternion.Euler(0, 0, 0));
        chest.transform.parent = position[2].transform;
        chest.name = "TreasureChest";
    }
}
