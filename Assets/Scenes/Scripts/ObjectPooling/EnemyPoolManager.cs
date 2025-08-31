using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPoolManager : MonoBehaviour
{
    public GameObject enemyPrefab;    // HUD의 원본 프리팹
    public int poolSize = 4;       // 초기 풀 크기
    public int maxPoolSize = 6;    // 최대 풀 크기

    private ObjectPool<GameObject> enemyPool;

    public void Initialize()
    {
        // ObjectPool 초기화
        enemyPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(enemyPrefab, transform),    // 생성 로직
            actionOnGet: enemy => enemy.SetActive(true),                // 풀에서 가져올 때
            actionOnRelease: enemy => enemy.SetActive(false),           // 풀로 반환할 때,
            actionOnDestroy: enemy => Destroy(enemy),                   // 풀 크기 초과 시 삭제
            defaultCapacity: poolSize,                              // 초기 용량
            maxSize: maxPoolSize                                    // 최대 용량
        );
    }

    public GameObject GetEnemy(GameObject prefab)
    {
        enemyPrefab = prefab;       // 소환할 적 프리팹받기
        return enemyPool.Get();     // HUD를 풀에서 가져오기
    }

    public void ReturnEnemy(GameObject enemy)
    {
        Debug.Log("EnemyPool로 반환 중");
        enemyPool.Release(enemy);   // HUD를 풀로 반환하기
    }
}
