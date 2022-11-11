namespace UnityWeld.Binding.Adapters
{
    /// <summary>
    /// Adapter for converting from an int to a string.
    /// </summary>
    [Adapter(typeof(int), typeof(string), typeof(FloatToStringAdapterOptions))]
    public class IntToStringAdapter : IAdapter
    {
        public object Convert(object valueIn, AdapterOptions options)
        {
            var format = ((FloatToStringAdapterOptions)options).Format;
            return ((int)valueIn).ToString(format);
        }
    }
}
