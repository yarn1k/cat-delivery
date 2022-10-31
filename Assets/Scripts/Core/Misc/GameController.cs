using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Core.Infrastructure.Signals.Cats;
using Core.Infrastructure.Signals.Game;
using Core.Infrastructure.Signals.UI;

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

        public void Initialize()
        {
            //_signalBus.Subscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.Subscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
            _signalBus.Subscribe<GameScoreChangedSignal>(OnGameScoreChangedSignal);
        }

        public void LateDispose()
        {
            //_signalBus.TryUnsubscribe<CatSavedSignal>(OnCatSavedSignal);
            _signalBus.TryUnsubscribe<CatKidnappedSignal>(OnCatKidnappedSignal);
            _signalBus.TryUnsubscribe<GameScoreChangedSignal>(OnGameScoreChangedSignal);
            _asyncProcessor.StopCoroutine(GameOver());
        }


        //private void OnCatSavedSignal(CatSavedSignal signal)
        //{
        //    signal.SavedCat.SetSaveState();
        //}

        private void OnCatKidnappedSignal(CatKidnappedSignal signal)
        {
            signal.KidnappedCat.Kidnap();
            if (lives - 1 != 0)
                lives -= 1;
            else
                _asyncProcessor.StartCoroutine(GameOver());
        }

        private void OnGameScoreChangedSignal(GameScoreChangedSignal signal)
        {
            _score += signal.Value;
            _signalBus.Fire(new UIScoreChangedSignal { Value = _score });
        }

        private IEnumerator GameOver()
        {
            isGameOver = true;
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(0);
        }
    }
}
