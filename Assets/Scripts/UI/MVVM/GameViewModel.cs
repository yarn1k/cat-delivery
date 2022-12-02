using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;
using Core.Models;
using Core.Infrastructure.Signals.Cats;
using Core.Infrastructure.Signals.Game;
using Core.Audio;

namespace Core.UI
{
    [Binding]
    public class GameViewModel : MonoBehaviour, INotifyPropertyChanged, IPauseHandler
    {
        [field: SerializeField] private HealthViewModel HealthVM { get; set; }
        [field: SerializeField] private GameOverViewModel GameOverVM { get; set; }

        private SignalBus _signalBus;
        private GameSettings _gameSettings;
        private CatsSettings _catsSettings;
        private GameSounds _gameSounds;
        private Countdown _countdown;
        private float _time;
        private int _score;
        private int _streak;
        private bool _enabled;

        private readonly string _playerKeyOfHighScore = "HighScore";
        private readonly string _playerKeyOfBestTime = "BestTime";

        [Binding]
        public int Score
        {
            get => _score;
            set
            {
                if (_score == value) return;

                _score = value;
                OnPropertyChanged("Score");
            }
        }

        [Binding]
        public int Streak
        {
            get => _streak;
            set
            {
                if (_streak == value) return;

                _streak = value;
                OnPropertyChanged("Streak");
            }
        }

        [Binding]
        public float CurrentTime
        {
            get => _time;
            set
            {
                if (_time == value) return;

                _time = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        [Binding]
        public int HighScore
        {
            get
            {
                return PlayerPrefs.GetInt(_playerKeyOfHighScore);
            }
            set
            {
                if (value > PlayerPrefs.GetInt(_playerKeyOfHighScore))
                    PlayerPrefs.SetInt(_playerKeyOfHighScore, value);
            }
        }

        [Binding]
        public float BestTime
        {
            get
            {
                return PlayerPrefs.GetInt(_playerKeyOfBestTime);
            }
            set
            {
                if (value > PlayerPrefs.GetInt(_playerKeyOfBestTime))
                    PlayerPrefs.SetInt(_playerKeyOfBestTime, (int)value);
            }
        }

        public bool IsGameOver => HealthVM.IsGameOver;
        public event PropertyChangedEventHandler PropertyChanged;

        [Inject]
        private void Construct(
            SignalBus signalBus, 
            GameSettings gameSettings, 
            GameSounds gameSounds, 
            CatsSettings catsSettings,
            Countdown countdown,
            IPauseProvider pauseProvider)
        {
            _gameSettings = gameSettings;
            _gameSounds = gameSounds;
            _signalBus = signalBus;
            _catsSettings = catsSettings;
            _countdown = countdown;
            pauseProvider.Register(this);
        }
        private void Awake()
        {
            HealthVM.Clear();
            HealthVM.Init(_gameSettings.Lifes);

            _signalBus.Subscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.Subscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.Subscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
            _signalBus.Subscribe<PlayerWeaponMissedSignal>(OnPlayerWeaponMissed);
        }
        private IEnumerator Start()
        {
            yield return _countdown.StartCountdown(_gameSettings.PreparationTime);
            _enabled = true;
            SoundManager.PlayMusic(_gameSounds.GameBackground.Clip, _gameSounds.GameBackground.Volume);
        }
        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.TryUnsubscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.TryUnsubscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
            _signalBus.TryUnsubscribe<PlayerWeaponMissedSignal>(OnPlayerWeaponMissed);
        }
        private void Update()
        {
            if (!_enabled || HealthVM.IsGameOver) return;

            CurrentTime += Time.deltaTime;
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void OnCatFellSignal()
        {
            Score += _catsSettings.FallingReward;
        }
        private void OnCatSavedSignal()
        {
            Score += _catsSettings.SavedReward;
            Streak++;
        }
        private void OnCatKidnappedSignal()
        {
            Score -= _catsSettings.KidnapPenalty;
            Streak = 0;
            HealthVM.RemoveHealth(1);

            if (IsGameOver) OnGameOverSignal();
        }
        private void OnPlayerWeaponMissed()
        {
            Streak = 0;
        }
        private void OnGameOverSignal()
        {
            _signalBus.Fire<GameOverSignal>();

            HighScore = _score;
            BestTime = _time;

            _signalBus.TryUnsubscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.TryUnsubscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.TryUnsubscribe<CatKidnappedSignal>(OnCatKidnappedSignal);

            SoundManager.StopMusic();
            SoundManager.PlayOneShot(_gameSounds.GameOver.Clip, _gameSounds.GameOver.Volume);
            HealthVM.Reset();
            GameOverVM.Show();
        }

        void IPauseHandler.SetPaused(bool isPaused)
        {
            _enabled = !isPaused;
        }
    }
}