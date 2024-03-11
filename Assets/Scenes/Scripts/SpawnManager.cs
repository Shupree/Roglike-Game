using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] position;
    public GameObject[] enemyPrefebArr;
    public GameObject enemyPrefeb;
    public GameObject Enemy;
    private int positionNum;

    void Awake()
    {
        positionNum = 0;
    }

    public void EnemySpawn(int[] EnemyID)
    {
        for (int i = 0; i < 4; i++)
        {
            if (EnemyID[i] == 0) {
                // 적이 존재하지 않음.
            } 
            else {
                // 해당 적 정보 수집    (주의. EnemyID와 (enemyPrefebArr 순서 - 1) 은 같아야 함.)
                enemyPrefeb = enemyPrefebArr[EnemyID[i] - 1];

                Enemy = Instantiate(enemyPrefeb, position[positionNum].transform.position, Quaternion.Euler(0, 0, 0));
                Enemy.transform.parent = position[positionNum].transform;
                Enemy.name = Enemy.GetComponent<Enemy>().data.enemyName;
                GameManager.instance.EnemyArr.Add(Enemy);

                positionNum++;
            }
        }
    }
}
