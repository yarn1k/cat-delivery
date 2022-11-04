using System;
using UnityEngine;
using TMPro;
using Zenject;

namespace Core.UI
{
    [RequireComponent(typeof(TextMeshPro))]
    public class BulletLabelVFX : MonoBehaviour, IDisposable, IPoolable<string, Color, IMemoryPool>
    {
        private TextMeshPro _text;
        private IMemoryPool _pool;
        private bool _disposed;
        private float _timer;

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }
        private void Update()
        {
            if (_disposed) return;

            if (Time.realtimeSinceStartup - _timer >= 1f)
            {
                Dispose();
            }
        }
        public void Dispose()
        {
            _disposed = true;
            _pool?.Despawn(this);
        }
        void IPoolable<string, Color, IMemoryPool>.OnDespawned()
        {
            _pool = null;
        }
        void IPoolable<string, Color, IMemoryPool>.OnSpawned(string label, Color color, IMemoryPool pool)
        {
            _pool = pool;
            _disposed = false;
            _text.text = label;
            _text.color = color;
            _timer = Time.realtimeSinceStartup;
        }

        public class Factory : PlaceholderFactory <string, Color, BulletLabelVFX> { }
    }
}