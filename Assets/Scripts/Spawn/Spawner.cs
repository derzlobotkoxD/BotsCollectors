using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T _prefab;

    private Vector3 _spawnPoint;
    private ObjectPool<T> _pool;
    private int _defaultCapacitPool = 5;
    private int _maxSizePool = 5;

    private void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ActivateInstance(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _defaultCapacitPool,
            maxSize: _maxSizePool);
    }

    public T GetInstance(Vector3 position) 
    {
        _spawnPoint = position;
        T resource = _pool.Get();

        return resource;
    }

    protected virtual void ReleaseInstance(T instance) =>
        _pool.Release(instance);

    protected virtual void ActivateInstance(T instance)
    {
        instance.transform.position = _spawnPoint;
        instance.gameObject.SetActive(true);
    }
}