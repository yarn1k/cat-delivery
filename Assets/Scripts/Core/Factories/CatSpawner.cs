using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Models;
using Core.Infrastructure.Signals.Cats;
using Core.UI;

namespace Core
{
    public class CatSpawner : IInitializable, ITickable, IPauseHandler
    {
        private SignalBus _signalBus;
        private CatView.Factory _catFactory;
        private DisposableLabelVFX.Factory _labelFactory;
        private CatsSettings _catsSettings;
        private float _timer;
        private float _spawnTime;
        private bool _enabled;

        private LinkedList<CatView> _cats = new LinkedList<CatView>();

        [Inject]
        private void Construct(
            SignalBus signalBus, 
            CatView.Factory catFactory, 
            DisposableLabelVFX.Factory labelFactory, 
            CatsSettings catsSettings)
        {
            _signalBus = signalBus;
            _catFactory = catFactory;
            _labelFactory = labelFactory;
            _catsSettings = catsSettings;
        }

        void IInitializable.Initialize()
        {
            _spawnTime = Random.Range(_catsSettings.SpawnInterval.x, _catsSettings.SpawnInterval.y);
        }
        void ITickable.Tick()
        {
            if (!_enabled) return;

            if (_timer >= _spawnTime)
            {
                SpawnCat();
                _spawnTime = Random.Range(_catsSettings.SpawnInterval.x, _catsSettings.SpawnInterval.y);
                _timer = 0f;
            }
            else _timer += Time.deltaTime;
        }
        void IPauseHandler.SetPaused(bool isPaused) => SetEnabled(!isPaused);
        private void SpawnCat()
        {
            float width = _catsSettings.SpawnWidth / 2f;
            Vector2 spawnPosition = new Vector2(Random.Range(-width, width), 5.85f);
            CatView cat = _catFactory.Create();
            cat.transform.position = spawnPosition;

            cat.Disposed += OnCatDisposed;
            cat.Saved += OnCatSaved;
            cat.Kidnapped += OnCatKidnapped;

            cat.SetInteractable(true);
            cat.SetDirection(Vector2.down, _catsSettings.CatsFallingSpeed);

            int rand = Random.Range(0, _catsSettings.Skins.Length);
            cat.SetSkin(_catsSettings.Skins[rand]);

            _cats.AddLast(cat);
        }

        private void OnCatKidnapped(CatView cat, Vector2 direction)
        {
            cat.SetInteractable(false);
            cat.SetDirection(direction, _catsSettings.CatsKidnapSpeed);
            cat.Dispose();
            _signalBus.Fire<CatKidnappedSignal>();

            var label = _labelFactory.Create($"Kidnapped\n-{_catsSettings.KidnapPenalty}", Color.red);
            label.transform.position = cat.transform.position;
        }
        private void OnCatSaved(CatView cat)
        {
            cat.SetInteractable(false);
            cat.SetDirection(Vector2.down, _catsSettings.CatsSaveSpeed);
            _signalBus.Fire<CatSavedSignal>();

            var label = _labelFactory.Create($"Saved\n+{_catsSettings.SavedReward}", Color.green);
            label.transform.position = cat.transform.position;
        }
        private void OnCatDisposed(CatView cat)
        {
            cat.Disposed -= OnCatDisposed;
            cat.Saved -= OnCatSaved;
            cat.Kidnapped -= OnCatKidnapped;
            _cats.Remove(cat);
        }
        public void SetEnabled(bool isEnabled)
        {
            _enabled = isEnabled;
        }
        public void Dispose()
        {
            while (_cats.Count > 0)
            {
                _cats.First.Value.Dispose();
            }
        }
    }
}