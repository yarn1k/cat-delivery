using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Core
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField]
        private RawImage _vignette;

        public enum FadeMode { On, Off }

        public void Shake(float duration, float frequence)
        {
            StartCoroutine(ShakeCoroutine(duration, frequence));
        }
        private IEnumerator ShakeCoroutine(float duration, float frequence)
        {
            Vector3 startPosition = transform.position;
            float factor = 0f;
            while (factor < 1f)
            {
                transform.position = startPosition + (Vector3)UnityEngine.Random.insideUnitCircle * frequence;
                factor += Time.deltaTime / duration;
                yield return null;
            }
            transform.position = startPosition;
        }

        public void ShakeRotate(float duration, float angleDeg, Vector2 direction)
        {
            StartCoroutine(ShakeRotateCoroutine(duration, angleDeg, direction));
        }
        private IEnumerator ShakeRotateCoroutine(float duration, float angleDeg, Vector2 direction)
        {
            float elapsed = 0f;
            Quaternion startRotation = transform.localRotation;
            float halfDuration = duration / 2f;
            direction = direction.normalized;
            while (elapsed < duration)
            {
                Vector2 currentDirection = direction;
                float t = elapsed < halfDuration ? elapsed / halfDuration : (duration - elapsed) / halfDuration;
                float currentAngle = Mathf.Lerp(0f, angleDeg, t);
                currentDirection *= Mathf.Tan(currentAngle * Mathf.Deg2Rad);
                Vector3 resDirection = ((Vector3)currentDirection + Vector3.forward).normalized;
                transform.localRotation = Quaternion.FromToRotation(Vector3.forward, resDirection);

                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localRotation = startRotation;
        }

        public void Fade(FadeMode mode, float duration, Action onCompleted = null)
        {
            _vignette.gameObject.SetActive(true);
            float alpha = mode == FadeMode.On ? 0f : 1f;
            _vignette.color = _vignette.color.WithAlpha(1f - alpha);
            _vignette.DOFade(alpha, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    _vignette.gameObject.SetActive(false);
                    onCompleted?.Invoke();
                });
        }
    }
}