using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _repeatRate = 30f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: OnGet,
            actionOnRelease: OnRealise,
            actionOnDestroy: cube => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Update()
    {
        if (_pool.CountActive < _poolMaxSize)
            _pool.Get();
    }

    private void OnGet(Cube cube)
    {
        cube.Init(GetRandomStartPosition());
        cube.gameObject.SetActive(true);
        cube.Changed += ChangePool;
    }

    private void OnRealise(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.Changed -= ChangePool;
    }

    private void ChangePool(Cube cube)
    {
        _pool.Release(cube);
    }

    private Vector3 GetRandomStartPosition()
    {
        int minRandomValue = -10;
        int maxRandomValue = 10;
        System.Random random = new System.Random();
        int valueRandom = random.Next(minRandomValue, maxRandomValue);

        return new Vector3(
            _startPoint.transform.position.x + valueRandom,
            _startPoint.transform.position.y,
            _startPoint.transform.position.z + valueRandom);
    }
}
