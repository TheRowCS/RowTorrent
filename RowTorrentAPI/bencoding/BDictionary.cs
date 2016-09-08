using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.bencoding {
    public class BDictionary : AbstractBElement, IEnumerable {
        /// <summary>
        /// A dictionary of AbstractBElements with string keys.
        /// </summary>
        public readonly Dictionary<string, AbstractBElement> InnerValue;

        /// <summary>
        /// Creates a new empty BDictionary object.
        /// </summary>
        public BDictionary() {
            InnerValue = new Dictionary<String, AbstractBElement>();
        }

        /// <summary>
        /// Creates a new BDictionary object from an existing dictionary of String/AbstractBelement pairs.
        /// </summary>
        /// <param name="dict">The dictionary to be added to BDictionary object.</param>
        public BDictionary(Dictionary<String, AbstractBElement> dict) {
            InnerValue = new Dictionary<string, AbstractBElement>(dict);
        }

        /// <summary>
        /// Converts the dictionary to a bencoded string.
        /// </summary>
        /// <returns>The BDictionary object as a bencoded string.</returns>
        public override string BencodeElement() {
            var str = new StringBuilder();
            str.Append(NodeSymbols.DictStart);
            foreach (var item in InnerValue) {
                str.Append((new BString(item.Key)).BencodedString)
                    .Append(item.Value.BencodedString);
            }
            str.Append(NodeSymbols.DelimEnd);
            return str.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return InnerValue.GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, AbstractBElement>> GetEnumerator() {
            return InnerValue.GetEnumerator();
        }

        // Array accessor for dictionary.
        public AbstractBElement this[string key] {
            get { return InnerValue[key]; }
            set { InnerValue[key] = value; }
        }

        public void Add(string key, AbstractBElement value) {
            InnerValue.Add(key, value);
        }

        public bool Remove(string key) {
            return InnerValue.Remove(key);
        }

        public bool ContainsKey(string key) {
            return InnerValue.ContainsKey(key);
        }

        public override string ToString() {
            var buff = new StringBuilder();
            buff.Append("dictionary: { ");
            foreach (var el in InnerValue)
                buff.Append(el.Key + "->" + el.Value + ", ");
            buff.Remove(buff.Length - 2, 2);
            buff.Append(" } ");
            return buff.ToString();
        }
    }
}