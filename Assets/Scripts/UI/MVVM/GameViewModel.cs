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
    public class GameViewModel : MonoBehaviour, INotifyPropertyChanged
    {
        [SerializeField]
        private Transform _healthParentTransform;
        [SerializeField]
        private GameObject _healthPrefab;
        [SerializeField]
        private GameObject _gameOverPanel;

        private SignalBus _signalBus;
        private GameSettings _gameSettings;
        private CatsSettings _catsSettings;
        private GameSounds _gameSounds;
        private float _startTime;
        private int _time;
        private int _score;

        private HealthViewModel _healthVM;
        private GameOverViewModel _gameOverVM;

        [Binding]
        public HealthViewModel HealthVM
        {
            get
            {
                _healthVM ??= new HealthViewModel(_healthParentTransform, _healthPrefab);
                return _healthVM;
            }
        }

        [Binding]
        public GameOverViewModel GameOverVM
        {
            get
            {
                _gameOverVM ??= new GameOverViewModel(_gameOverPanel);
                return _gameOverVM;
            }
        }

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
        public int CurrentTime
        {
            get => _time;
            set
            {
                if (_time == value) return;

                _time = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        public bool IsGameOver => HealthVM.IsGameOver;
        public event PropertyChangedEventHandler PropertyChanged;

        [Inject]
        private void Construct(SignalBus signalBus, GameSettings gameSettings, GameSounds gameSounds, CatsSettings catsSettings)
        {
            _gameSettings = gameSettings;
            _gameSounds = gameSounds;
            _signalBus = signalBus;
            _catsSettings = catsSettings;
        }
        private void Awake()
        {
            HealthVM.Clear();
            HealthVM.Init(_gameSettings.Lifes);

            _startTime = Time.realtimeSinceStartup;

            _signalBus.Subscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.Subscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.Subscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
        }
        private void Start()
        {
            SoundManager.PlayMusic(_gameSounds.GameBackground.Clip, _gameSounds.GameBackground.Volume);
        }
        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.TryUnsubscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.TryUnsubscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
        }
        private void Update()
        {
            if (HealthVM.IsGameOver) return;

            CurrentTime = (int)Mathf.Floor(Time.realtimeSinceStartup - _startTime);
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
        }
        private void OnCatKidnappedSignal()
        {
            Score -= _catsSettings.KidnapPenalty;
            HealthVM.RemoveHealth(1);

            if (IsGameOver)
            {
                _signalBus.Fire<GameOverSignal>();
                OnGameOverSignal();
            }
        }
        private void OnGameOverSignal()
        {
            _signalBus.TryUnsubscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.TryUnsubscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.TryUnsubscribe<CatKidnappedSignal>(OnCatKidnappedSignal);

            SoundManager.StopMusic();
            SoundManager.PlayOneShot(_gameSounds.GameOver.Clip, _gameSounds.GameOver.Volume);
            GameOverVM.Show();
        }
    }
}