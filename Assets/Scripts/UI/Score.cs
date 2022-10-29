using Core.Infrastructure.Signals.UI;
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
            _signalBus.Subscribe<UIScoreChangedSignal>(OnUIScoreChangedSignal);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<UIScoreChangedSignal>(OnUIScoreChangedSignal);
        }

        private void OnUIScoreChangedSignal(UIScoreChangedSignal signal)
        {
            _textMeshPro.text = "Score: " + signal.Value;
        }
    }
}
