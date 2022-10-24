using Core.Models;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    private SignalBus _signalBus;
    private GameSettings _settings;

    private Rigidbody2D _rigidbody2D;

    [Inject]
    private void Construct(SignalBus signalBus, GameSettings settings)
    {
        _signalBus = signalBus;
        _settings = settings;
    }

    public void Init(Transform firePoint)
    {
        transform.position = firePoint.position;
        transform.rotation = firePoint.rotation;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.AddForce(firePoint.up * _settings.BulletForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Debug.Log(collision.gameObject.name);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public class Factory : PlaceholderFactory<Bullet> { }
}
