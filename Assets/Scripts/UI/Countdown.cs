using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Core.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Countdown : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        public IEnumerator StartCountdown(float seconds)
        {
            _text.text = seconds.ToString();
            _text.gameObject.SetActive(true);
            _text.color = _text.color.WithAlpha(0f);
            _text.DOColor(_text.color.WithAlpha(1f), 1f);
            yield return StartCoroutine(Coroutine(seconds));
        }

        private IEnumerator Coroutine(float seconds)
        {
            for (float i = seconds; i > 0; i--)
            {
                yield return new WaitForSeconds(1f);
                _text.text = i.ToString();
            }
            _text.DOColor(_text.color.WithAlpha(0f), 1f)
                .OnComplete(() => _text.gameObject.SetActive(false));
        }
    }
}
