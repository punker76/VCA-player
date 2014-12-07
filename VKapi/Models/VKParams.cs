using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace VKapi.Models
{
    public class VKParams : IEnumerable
    {
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public Dictionary<string, string>.Enumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public void Add(string param, object value)
        {
            Debug.Assert(value != null);

            if (!_values.ContainsKey(param))
            {
                _values.Add(param, value.ToString());
            }
            else
            {
                _values[param] = value.ToString();
            }
        }

        public string Get(string param)
        {
            if (_values.ContainsKey(param))
            {
                return _values[param];
            }

            return String.Empty;
        }
    }
}