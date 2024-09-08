using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private bool _isDestroed;

    public event UnityAction<Cube> Changed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDestroed)
            return;

        if (collision.gameObject.TryGetComponent<Platform>(out _) == false)
            return;

        _renderer.material.color = Random.ColorHSV();
        _isDestroed = true;

        StartCoroutine(DeleteCubeWhithDelay());
    }

    public void Init(Vector3 start)
    {
        transform.position = start;
        transform.rotation = Quaternion.identity;
        _renderer.material.color = Color.yellow;
        _rigidbody.velocity = Vector3.zero;
        _isDestroed = false;
    }

    private IEnumerator DeleteCubeWhithDelay()
    {
        int minRandomValue = 2;
        int maxRandomValue = 5;
        System.Random random = new System.Random();
        int valueRandom = random.Next(minRandomValue, maxRandomValue + 1);

        yield return new WaitForSeconds(valueRandom);

        Changed?.Invoke(this);
    }
}
