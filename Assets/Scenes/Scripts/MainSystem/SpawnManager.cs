using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
    private HUDPoolManager hudPoolManager;       // HUDPoolManager
    private EnemyPoolManager enemyPoolManager;   // EnemyPoolManager

    [Header("Position")]
    public GameObject[] position;
    public GameObject position_Outside;

    [Header ("Prefabs")]
    public GameObject[] enemyPrefabArr;     // enemy Prefab Array
    private GameObject enemyPrefab;

    public GameObject[] allyPrefabArr;      // ally Prefab Array
    private GameObject allyPrefab;

    [Space(10f)]
    public GameObject[] NPCPrefabArr;       // NPC Prefab Array

    [Space (10f)]
    public GameObject[] chestPrefabArr;    // chest Prefab Array

    // private GameObject enemy;
    // private GameObject ally;

    public void Initialize()
    {
        hudPoolManager = gameObject.GetComponent<HUDPoolManager>();
        enemyPoolManager = gameObject.GetComponent<EnemyPoolManager>();
    }

    public void SpawnEnemy(int[] EnemyID)
    {
        for (int i = 0; i < 4; i++)
        {
            if (EnemyID[i] == 0)
            {
                // 적이 존재하지 않음.
            }
            else
            {
                // 해당 적 정보 수집    (주의. 'enemyID - 1와 'enemyPrefebArr'의 순서는 같아야 함.)
                enemyPrefab = enemyPrefabArr[EnemyID[i] - 1];   // 몬스터Prefab 확인

                /*
                GameObject enemy = Instantiate(enemyPrefab, position[i].transform.position, Quaternion.Euler(0, 0, 0));    // 몬스터 스폰
                enemy.transform.parent = position[i].transform;             // Parent 설정
                enemy.name = enemy.GetComponent<Enemy>().data.unitName;    // 몬스터 이름 명명
                GameManager.instance.turnManager.RegisterEnemy(enemy.GetComponent<Enemy>());
                */
                GameObject enemy = enemyPoolManager.GetEnemy(enemyPrefab);
                enemy.transform.parent = position[i].transform;             // Parent 설정
                enemy.transform.position = position[i].transform.position;
                enemy.name = enemy.GetComponent<Enemy>().data.unitName;    // 몬스터 이름 명명
                GameManager.instance.turnManager.RegisterEnemy(enemy.GetComponent<Enemy>());

                // Enemy HUD 활성화
                Debug.Log(hudPoolManager);
                GameObject hud = hudPoolManager.Get();
                if (hud != null)
                {
                    hud.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);  // Screen Space Canvas 기준으로 배치
                    hud.GetComponent<RectTransform>().position = enemy.gameObject.transform.position;
                    // 유닛 하단에 HUD 위치
                    hud.GetComponent<HUD>().SetHUD(enemy.GetComponent<Enemy>());
                    enemy.GetComponent<Enemy>().SetHUD(hud);
                }
            }
        }
    }
    
    public void SpawnAlly(int[] AllyID)
    {
        for (int i = 0; i < 4; i++)
        {
            if (AllyID[i] == 0)
            {
                // 아군 데이터가 존재하지 않음.
            }
            else
            {
                // 해당 동료 정보 수집    (주의. 'allyID - 1와 'allyPrefebArr'의 순서는 같아야 함.)
                allyPrefab = allyPrefabArr[AllyID[i] - 1];   // allyPrefab 확인

                GameObject ally = Instantiate(allyPrefab, position[i].transform.position, Quaternion.Euler(0, 0, 0));    // 동료 스폰
                ally.transform.parent = position[i].transform;             // Parent 설정
                ally.name = ally.GetComponent<Ally>().data.unitName;    // 동료 이름 명명
                GameManager.instance.turnManager.RegisterAlly(ally.GetComponent<Ally>());

                // Ally HUD 활성화
                GameObject hud = hudPoolManager.Get();
                if (hud != null)
                {
                    hud.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);  // Screen Space Canvas 기준으로 배치
                    hud.GetComponent<RectTransform>().position = ally.gameObject.transform.position;
                    // 유닛 하단에 HUD 위치
                    hud.GetComponent<HUD>().SetHUD(ally.GetComponent<Ally>());
                    ally.GetComponent<Ally>().SetHUD(hud);
                }
            }
        }
    }

    // 지정 프리팹 오브젝트를 스폰
    public void SpawnPrefab(GameObject prefab)
    {
        GameObject npc = Instantiate(prefab, position[3].transform.position, Quaternion.Euler(0, 0, 0));    // 오브젝트 스폰
        npc.transform.parent = position[3].transform;           // Parent 설정
        npc.GetComponent<SpriteRenderer>().flipX = true;        // 스프라이트 X축 반전
        npc.name = "NPC";    // 오브젝트 이름 명명
    }

    public void StoreNPCSpawn()
    {
        GameObject NPC;
        NPC = Instantiate(NPCPrefabArr[0], position[2].transform.position, Quaternion.Euler(0, 0, 0));
        NPC.transform.parent = position[2].transform;
        NPC.name = "StoreNPC";
        // GameManager.instance._StoreManager.storeNPC = NPC;
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
