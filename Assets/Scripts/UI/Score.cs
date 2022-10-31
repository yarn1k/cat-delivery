using Core.Infrastructure.Signals.Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class Score : MonoBehaviour
    {
        private SignalBus _signalBus;
        [SerializeField]
        private TMP_Text _textMeshPro;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<GameScoreChangedSignal>(OnGameScoreChangedSignal);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameScoreChangedSignal>(OnGameScoreChangedSignal);
        }

        private void OnGameScoreChangedSignal(GameScoreChangedSignal signal)
        {
            _textMeshPro.text = "Score: " + signal.Value;
        }
    }
}
