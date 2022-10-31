using TMPro;
using UnityEngine;
using Zenject;
using Core.Infrastructure.Signals.Game;

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
            _signalBus.Subscribe<GameScoreChangedSignal>(OnUIScoreChangedSignal);
        }
        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameScoreChangedSignal>(OnUIScoreChangedSignal);
        }

        private void OnUIScoreChangedSignal(GameScoreChangedSignal signal)
        {
            _textMeshPro.text = "Score: " + signal.Value;
        }
    }
}
