using System.Text;

namespace UnityWeld.Binding.Adapters
{
    /// <summary>
    /// Adapter that converts a int to a string.
    /// </summary>
    [Adapter(typeof(int), typeof(string))]
    public class IntToTimeAdapter : IAdapter
    {
        private StringBuilder _builder = new StringBuilder();
        public object Convert(object valueIn, AdapterOptions options)
        {
            _builder.Clear();

            int elapsed = System.Convert.ToInt32(valueIn);

            int minutes = elapsed / 60;
            if (minutes < 10) _builder.Append("0");
            _builder.Append(minutes);
            _builder.Append(":");

            int seconds = elapsed - minutes * 60;
            if (seconds < 10) _builder.Append("0");
            _builder.Append(seconds);
            return _builder.ToString();
        }
    }
}
