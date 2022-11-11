using System;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;

namespace Core.UI
{
    [Binding]
    public class HealthViewModel
    {
        private readonly Transform _parentPanel;
        private readonly GameObject _heathPrefab;

        private int _currentIndex;
        private byte _maxHealth;

        public bool IsGameOver => _currentIndex == 0;

        public HealthViewModel(Transform parentPanel, GameObject heathPrefab)
        {
            _parentPanel = parentPanel;
            _heathPrefab = heathPrefab;

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