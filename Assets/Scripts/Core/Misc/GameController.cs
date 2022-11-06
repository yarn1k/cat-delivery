using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Core.Infrastructure.Signals.Cats;
using Core.Infrastructure.Signals.Game;
using Core.UI;
using Core.Models;

namespace Core.Match
{
    public class AsyncProcessor : MonoBehaviour
    {
    }

    public class GameController : IInitializable, ILateDisposable
    {
        private readonly SignalBus _signalBus;
        private AsyncProcessor _asyncProcessor;
        private BulletLabelVFX.Factory _labelFactory;
        private GameSettings _settings;
        private AudioSource _audioSource;
        private AudioGameSettings _audioGameSettings;
        private AudioCatsSettings _audioCatsSettings;
        private int _score;
        private int _lives = 3;
        public bool IsGameOver => _lives == 0;

        public GameController(SignalBus signalBus, AsyncProcessor asyncProcessor, BulletLabelVFX.Factory labelFactory, GameSettings settings,
            AudioSource audioSource, AudioGameSettings audioGameSettings, AudioCatsSettings audioCatsSettings)
        {
            _signalBus = signalBus;
            _asyncProcessor = asyncProcessor;
            _labelFactory = labelFactory;
            _settings = settings;
            _audioSource = audioSource;
            _audioGameSettings = audioGameSettings;
            _audioCatsSettings = audioCatsSettings;
        }

        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.Subscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.Subscribe<CatKidnappedSignal>(OnCatKidnappedSignal);

            Audio gameLevelStart = _audioGameSettings.GameLevelStart;
            _audioSource.PlayOneShot(gameLevelStart.Clip, gameLevelStart.Volume);
            InitBackgroundMusic();
        }
        void ILateDisposable.LateDispose()
        {
            _signalBus.TryUnsubscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.TryUnsubscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.TryUnsubscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
            _asyncProcessor.StopCoroutine(GameOver());
        }

        private void InitBackgroundMusic()
        {
            Audio gameBackground = _audioGameSettings.GameBackground;
            _audioSource.clip = gameBackground.Clip;
            _audioSource.volume = gameBackground.Volume;
            _audioSource.Play();
        }
        private void OnCatFellSignal(CatFellSignal signal)
        {
            _score += 10;
            _signalBus.Fire(new GameScoreChangedSignal { Value = _score });
        }

        private void OnCatSavedSignal(CatSavedSignal signal)
        {
            _score += _settings.SavedReward;
            var label = _labelFactory.Create($"Saved\n+{_settings.SavedReward}", Color.green);
            label.transform.position = signal.SavedCat.transform.position;

            _signalBus.Fire(new GameScoreChangedSignal { Value = _score });
        }

        private void OnCatKidnappedSignal(CatKidnappedSignal signal)
        {
            Audio catGrabbed = _audioCatsSettings.CatGrabbed;
            _audioSource.PlayOneShot(catGrabbed.Clip, catGrabbed.Volume);
            _score -= _settings.KidnapPenalty;
            var label = _labelFactory.Create($"Kidnapped\n-{_settings.KidnapPenalty}", Color.red);
            label.transform.position = signal.KidnappedCat.transform.position;

            _signalBus.Fire(new GameScoreChangedSignal { Value = _score });

            if (!IsGameOver)
                _lives -= 1;
            else
                _asyncProcessor.StartCoroutine(GameOver());
        }

        private IEnumerator GameOver()
        {
            Audio gameOver = _audioGameSettings.GameOver;
            _audioSource.PlayOneShot(gameOver.Clip, gameOver.Volume);
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(0);
        }
    }
}
