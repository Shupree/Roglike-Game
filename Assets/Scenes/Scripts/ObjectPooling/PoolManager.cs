using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 모든 오브젝트 풀의 기반이 되는 제네릭 추상 클래스입니다.
/// </summary>
/// <typeparam name="T">풀링할 오브젝트의 컴포넌트 타입</typeparam>
public abstract class PoolManager<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Pooling Settings")]
    [SerializeField]
    private GameObject objectPrefab;    // 풀링할 오브젝트의 원본 프리팹

    [SerializeField]
    protected int initialPoolSize = 5;   // 초기 풀 크기

    [SerializeField]
    protected int maxPoolSize = 10;      // 최대 풀 크기

    // ObjectPool 인스턴스
    protected IObjectPool<GameObject> pool;

    // 풀 초기화 로직
    public void InitializePool()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: CreatePooledObject,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnReleaseToPool,
            actionOnDestroy: OnDestroyPooledObject,
            defaultCapacity: initialPoolSize,
            maxSize: maxPoolSize
        );
    }

    // 풀에서 오브젝트를 가져오는 공통 메서드
    public GameObject Get() => pool.Get();

    // 풀로 오브젝트를 반환하는 공통 메서드
    public void Release(GameObject obj) => pool.Release(obj);

    // 자식 클래스에서 구현해야 할 추상/가상 메서드들
    protected virtual GameObject CreatePooledObject()
    {
        GameObject instance = Instantiate(objectPrefab, transform);
        return instance;
    }
    protected virtual void OnGetFromPool(GameObject obj) => obj.gameObject.SetActive(true);
    protected virtual void OnReleaseToPool(GameObject obj) => obj.gameObject.SetActive(false);
    protected virtual void OnDestroyPooledObject(GameObject obj) => Destroy(obj.gameObject);
}
