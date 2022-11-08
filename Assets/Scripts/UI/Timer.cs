using System.Text;
using UnityEngine;
using TMPro;
using Zenject;
using Core.Infrastructure.Signals.Game;

namespace Core.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Timer : MonoBehaviour
    {
        private StringBuilder _builder = new StringBuilder();
        private TextMeshProUGUI _text;
        private SignalBus _signalBus;
        private bool _enable = true;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _signalBus.Subscribe<GameOverSignal>(OnGameOver);
        }
        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<GameOverSignal>(OnGameOver);
        }
        private void Update()
        {
            if (_enable)
            {
                SetTime(Time.realtimeSinceStartup);
            }
        }

        private void OnGameOver()
        {
            StopTimer();
            ResetTime();
        }
        public void StopTimer()
        {
            _enable = false;
        }
        public void SetTime(float elapsedTime)
        {
            _builder.Clear();

            int elapsed = (int)elapsedTime;

            int minutes = elapsed / 60;
            if (minutes < 10) _builder.Append("0");
            _builder.Append(minutes);
            _builder.Append(":");

            int seconds = elapsed - minutes * 60;
            if (seconds < 10) _builder.Append("0");
            _builder.Append(seconds);

            _text.text = _builder.ToString();
        }
        public void ResetTime() => _text.text = "00:00";
    }
}