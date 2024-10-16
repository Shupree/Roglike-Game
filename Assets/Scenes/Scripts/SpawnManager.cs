using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] position;
    public GameObject position_Outside;
    public GameObject[] enemyPrefabArr;     // Enemy Prefab Array
    private GameObject enemyPrefab;
    public GameObject[] NPCPrefabArr;       // NPC Prefab Array
    public GameObject[] ObjectPrefabArr;    // Object Prefab Array
    private GameObject Enemy;

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

                Enemy = Instantiate(enemyPrefab, position[i].transform.position, Quaternion.Euler(0, 0, 0));
                Enemy.transform.parent = position[i].transform;
                Enemy.name = Enemy.GetComponent<Enemy>().data.enemyName;
                GameManager.instance.EnemyList.Add(Enemy);
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

    public void TreasureChestSpawn()
    {
        GameObject Object;
        Object = Instantiate(ObjectPrefabArr[0], position[2].transform.position, Quaternion.Euler(0, 0, 0));
        Object.transform.parent = position[2].transform;
        Object.name = "TreasureChest";
    }
}
