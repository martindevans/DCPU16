using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Grammar
{
    internal class LabelMap
        : IReadOnlyDictionary<string, ushort>
    {
        private readonly Dictionary<string, ushort> _dictionary;

        public LabelMap()
        {
            _dictionary = new Dictionary<string, ushort>();
        }

        public void Add(string key, ushort value)
        {
            _dictionary.Add(Key(key), value);
        }

        public IEnumerator<KeyValuePair<string, ushort>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }

        public int Count => _dictionary.Count;

        public IEnumerable<string> Keys => _dictionary.Keys.Select(Key);

        public IEnumerable<ushort> Values => _dictionary.Values;

        private static string Key(string key)
        {
            return key.ToLowerInvariant();
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(Key(key));
        }

        public bool TryGetValue(string key, out ushort value)
        {
            return _dictionary.TryGetValue(Key(key), out value);
        }

        public ushort this[string key] => _dictionary[Key(key)];
    }
}
