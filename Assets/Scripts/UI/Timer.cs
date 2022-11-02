using System.Text;
using UnityEngine;
using TMPro;

namespace Core.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Timer : MonoBehaviour
    {
        private StringBuilder _builder = new StringBuilder();
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        private void Update()
        {
            SetTime(Time.realtimeSinceStartup);
        }

        private void SetTime(float elapsedTime)
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
    }
}