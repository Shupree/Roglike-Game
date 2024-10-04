using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] position;
    public GameObject position_Outside;
    public GameObject[] enemyPrefebArr;
    private GameObject enemyPrefeb;
    public GameObject[] NPCPrefebArr;
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
                enemyPrefeb = enemyPrefebArr[EnemyID[i] - 1];

                Enemy = Instantiate(enemyPrefeb, position[i].transform.position, Quaternion.Euler(0, 0, 0));
                Enemy.transform.parent = position[i].transform;
                Enemy.name = Enemy.GetComponent<Enemy>().data.enemyName;
                GameManager.instance.EnemyList.Add(Enemy);
            }
        }
    }

    public void StoreNPCSpawn() 
    {
        GameObject StoreNPC;
        StoreNPC = Instantiate(NPCPrefebArr[0], position[2].transform.position, Quaternion.Euler(0, 0, 0));
        StoreNPC.transform.parent = position[2].transform;
        // StoreNPC.name = StoreNPC.GetComponent<Enemy>().data.enemyName;
    }
}
