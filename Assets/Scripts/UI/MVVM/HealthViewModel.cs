using System;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using DG.Tweening;

namespace Core.UI
{
    [Binding]
    public class HealthViewModel
    {
        private readonly Transform _parentPanel;
        private readonly GameObject _heathPrefab;

        private int _currentIndex;
        private byte _maxHealth;
        private Tweener _vibeTweener;

        public bool IsGameOver => _currentIndex == 0;

        public HealthViewModel(Transform parentPanel, GameObject heathPrefab)
        {
            _parentPanel = parentPanel;
            _heathPrefab = heathPrefab;

        }

        private void VibeLastHealth(bool isVibing)
        {
            if (isVibing)
            {
                _vibeTweener = _parentPanel.GetChild(0).DOScale(1.5f, 0.5f).SetEase(Ease.InOutSine);
            }
            else _vibeTweener.Kill();
        }

        public void Init(byte maxHealth)
        {
            _maxHealth = maxHealth;
            _currentIndex = maxHealth;
            for (byte i = 0; i < _maxHealth; i++)
            {
                GameObject prefab = GameObject.Instantiate(_heathPrefab, _parentPanel);
                prefab.transform.SetAsFirstSibling();
            }
        }
        public void Clear()
        {
            for (int i = 0; i < _parentPanel.childCount; i++)
            {
                GameObject.Destroy(_parentPanel.GetChild(i).gameObject);
            }
        }
        public void AddHealth(byte value)
        {
            if (_currentIndex == _maxHealth) return;

            int newValue = Math.Min(_currentIndex + value, _maxHealth);
            for (int i = _currentIndex - 1; i < newValue; i++)
            {
                RawImage image = _parentPanel.GetChild(i).GetComponent<RawImage>();
                image.color = Color.white;
            }
            _currentIndex = newValue;
        }
        public void RemoveHealth(byte value)
        {
            if (_currentIndex == 0) return;

            int newValue = Math.Max(_currentIndex - value, 0);
            for (int i = _currentIndex - 1; i >= newValue; i--)
            {
                RawImage image = _parentPanel.GetChild(i).GetComponent<RawImage>();
                image.color = Color.grey;
            }
            _currentIndex = newValue;
        }
    }
}