using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

/*
public class HUDPoolManager : MonoBehaviour
{
    public GameObject hudPrefab;    // HUD의 원본 프리팹
    public int poolSize = 5;       // 초기 풀 크기
    public int maxPoolSize = 9;    // 최대 풀 크기

    private ObjectPool<GameObject> hudPool;

    public void Initialize()
    {
        // ObjectPool 초기화
        hudPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(hudPrefab, transform),    // 생성 로직
            actionOnGet: hud => hud.SetActive(true),                // 풀에서 가져올 때
            actionOnRelease: hud => hud.SetActive(false),           // 풀로 반환할 때,
            actionOnDestroy: hud => Destroy(hud),                   // 풀 크기 초과 시 삭제
            defaultCapacity: poolSize,                              // 초기 용량
            maxSize: maxPoolSize                                    // 최대 용량
        );
    }

    public GameObject GetHUD()
    {
        return hudPool.Get();   // HUD를 풀에서 가져오기
    }

    public void ReturnHUD(GameObject hud)
    {
        Debug.Log("HUD 반환 중");
        hudPool.Release(hud);   // HUD를 풀로 반환하기
    }
}
*/

public class HUDPoolManager : PoolManager<HUD>
{
    public GameObject hudPrefab;    // HUD의 원본 프리팹

    private void Awake()
    {
        initialPoolSize = 5;
        maxPoolSize = 9;
    }

    protected override GameObject CreatePooledObject()
    {
        GameObject instance = Instantiate(hudPrefab, transform);
        return instance;
    }
}


