using Core.Cats;
using Core.Enemy;
using Core.Infrastructure.Signals.Game;
using System;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour, IDisposable
{
    private SignalBus _signalBus;

    [Inject]
    private Cat.Factory _catFactory;
    [Inject]
    private Laser.Factory _laserFactory;

    [SerializeField]
    private Transform _level;
    [SerializeField]
    private Transform _enemyFirePoint;

    [Inject]
    private void Construct(SignalBus signalBus) => _signalBus = signalBus;

    public void Start()
    {
        _signalBus.Subscribe<GameSpawnedCatSignal>(OnGameSpawnedCatSignal);
        _signalBus.Subscribe<GameSpawnedLaserSignal>(OnGameSpawnedLaserSignal);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameSpawnedCatSignal>(OnGameSpawnedCatSignal);
        _signalBus.Unsubscribe<GameSpawnedLaserSignal>(OnGameSpawnedLaserSignal);
    }

    private void OnGameSpawnedCatSignal(GameSpawnedCatSignal signal)
    {
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10.0f, 8.0f), signal.SpawnPositionY, 0);
        var cat = _catFactory.Create();
        SetSpawnPosition(cat.gameObject, spawnPosition);
    }

    private void OnGameSpawnedLaserSignal(GameSpawnedLaserSignal signal)
    {
        Vector3 spawnPosition = new Vector3(0, UnityEngine.Random.Range(0f, 3.0f), 0);
        var laser = _laserFactory.Create();
        SetSpawnPosition(laser.gameObject, _enemyFirePoint.position);
    }

    private void SetSpawnPosition(GameObject item, Vector3 spawnPosition)
    {
        item.transform.SetParent(_level);
        item.gameObject.transform.position = spawnPosition;
    }
}
