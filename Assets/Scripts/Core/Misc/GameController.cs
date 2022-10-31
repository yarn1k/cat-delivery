using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Core.Infrastructure.Signals.Cats;
using Core.Infrastructure.Signals.Game;

namespace Core.Match
{
    public class AsyncProcessor : MonoBehaviour
    {
    }

    public class GameController : IInitializable, ILateDisposable
    {
        private readonly SignalBus _signalBus;
        private AsyncProcessor _asyncProcessor;
        private int _score;
        private int _lives = 3;
        private bool _isGameOver;

        public GameController(SignalBus signalBus, AsyncProcessor asyncProcessor)
        {
            _signalBus = signalBus;
            _asyncProcessor = asyncProcessor;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.Subscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.Subscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
        }

        public void LateDispose()
        {
            _signalBus.TryUnsubscribe<CatFellSignal>(OnCatFellSignal);
            _signalBus.TryUnsubscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.TryUnsubscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
            _asyncProcessor.StopCoroutine(GameOver());
        }

        private void OnCatFellSignal(CatFellSignal signal)
        {
            _score += 10;
            _signalBus.Fire(new GameScoreChangedSignal { Value = _score });
        }

        private void OnCatSavedSignal(CatSavedSignal signal)
        {
            signal.SavedCat.Save();
            _score += 50;
            _signalBus.Fire(new GameScoreChangedSignal { Value = _score });
        }

        private void OnCatKidnappedSignal(CatKidnappedSignal signal)
        {
            signal.KidnappedCat.Kidnap();
            _score -= 30;
            _signalBus.Fire(new GameScoreChangedSignal { Value = _score });
            if (_lives - 1 != 0)
                _lives -= 1;
            else
                _asyncProcessor.StartCoroutine(GameOver());
        }

        private IEnumerator GameOver()
        {
            _isGameOver = true;
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(0);
        }
    }
}
