using UnityEngine;
using Zenject;
using Core.Infrastructure.Signals.Cats;
using Core.Infrastructure.Signals.Game;
using Core.UI;
using Core.Models;
using Core.Audio;

namespace Core
{
    public class GameController : IInitializable, ILateDisposable
    {
        private readonly SignalBus _signalBus;
        private GameSounds _gameSounds;
        private DisposableLabelVFX.Factory _labelFactory;
        private GameSettings _settings;

        public GameController(SignalBus signalBus, GameSounds gameSounds, DisposableLabelVFX.Factory labelFactory, GameSettings settings)
        {
            _signalBus = signalBus;
            _gameSounds = gameSounds;
            _labelFactory = labelFactory;
            _settings = settings;
        }

        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.Subscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
            _signalBus.Subscribe<GameOverSignal>(OnGameOverSignal);
            SoundManager.PlayMusic(_gameSounds.GameBackground.Clip, _gameSounds.GameBackground.Volume);
        }
        void ILateDisposable.LateDispose()
        {
            _signalBus.TryUnsubscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.TryUnsubscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
            _signalBus.TryUnsubscribe<GameOverSignal>(OnGameOverSignal);
        }

        private void OnCatSavedSignal(CatSavedSignal signal)
        {
            var label = _labelFactory.Create($"Saved\n+{_settings.SavedReward}", Color.green);
            label.transform.position = signal.SavedCat.transform.position;
        }
        private void OnCatKidnappedSignal(CatKidnappedSignal signal)
        {
            var label = _labelFactory.Create($"Kidnapped\n-{_settings.KidnapPenalty}", Color.red);
            label.transform.position = signal.KidnappedCat.transform.position;
        }
        private void OnGameOverSignal()
        {
            SoundManager.StopMusic();
            SoundManager.PlayOneShot(_gameSounds.GameOver.Clip, _gameSounds.GameOver.Volume);
        }
    }
}
