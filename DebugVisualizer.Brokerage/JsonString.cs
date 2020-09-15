using Newtonsoft.Json;

namespace DebugVisualizer.Brokerage
{
    public static class JsonString
    {
        public static JsonString<T> FromValue<T>(T value)
        {
            return new JsonString<T>(true, value, null);
        }

        public static JsonString<T> FromJson<T>(string json)
        {
            return new JsonString<T>(false, default, json);
        }
    }
    
    /// <summary>
    /// Represents an encoded JSON value that describes an instance of <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The described instance</typeparam>
    public class JsonString<T>
    {
        //public static implicit operator JsonString<T>(string json) => JsonString.FromJson<T>(json);

        private bool _hasValue;
        private T _value;
        private string? _json;

        internal JsonString(bool hasValue, T value, string? json)
        {
            _hasValue = hasValue;
            _value = value;
            _json = json;
        }

        public override string ToString()
        {
            return this._json ??= JsonConvert.SerializeObject(_value);
        }

        public T Parse()
        {
            if (!_hasValue)
            {
                _value = JsonConvert.DeserializeObject<T>(_json!);
                _hasValue = true;
            }

            return _value;
        }
    }
}