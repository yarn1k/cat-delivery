using System.Collections;
using UnityEngine;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        private float _fadeTime;

        [Inject]
        private void Construct(Models.GameSettings settings)
        {
            _fadeTime = settings.FadeTime;
        }

        private IEnumerator Start()
        {
            yield return Fade(UI.FadeMode.On, _fadeTime);
        }

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
                transform.position = startPosition + (Vector3)Random.insideUnitCircle * frequence;
                factor += Time.unscaledDeltaTime / duration;
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

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            transform.localRotation = startRotation;
        }

        public IEnumerator Fade(UI.FadeMode mode, float duration)
        {
            var asset = Resources.Load<UI.Vignette>("Prefabs/UI/Vignette");
            var vignette = Instantiate(asset);
            yield return vignette.Fade(mode, duration);
        }
    }
}