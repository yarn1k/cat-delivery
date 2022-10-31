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
        private int lives = 3;
        private bool isGameOver;

        public GameController(SignalBus signalBus, AsyncProcessor asyncProcessor)
        {
            _signalBus = signalBus;
            _asyncProcessor = asyncProcessor;
        }

        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.Subscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
        }
        void ILateDisposable.LateDispose()
        {
            _signalBus.TryUnsubscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.TryUnsubscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
            _asyncProcessor.StopCoroutine(GameOver());
        }

        // TODO
        //private void OnCatFaltSignal(CatFaltSignal signal)
        //{
        //    _score += 10;
        //    _signalBus.Fire(new GameScoreChangedSignal { Value = _score });
        //}

        private void OnCatSavedSignal(CatSavedSignal signal)
        {
            _score += 50;
            _signalBus.Fire(new GameScoreChangedSignal { Value = _score });
        }
        private void OnCatKidnappedSignal(CatKidnappedSignal signal)
        {
            signal.KidnappedCat.Kidnap();
            if (lives - 1 != 0)
                lives -= 1;
            else
                _asyncProcessor.StartCoroutine(GameOver());
        }

        private IEnumerator GameOver()
        {
            isGameOver = true;
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(0);
        }
    }
}
