using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.bencoding {
    public class BDictionary : AbstractBElement, IEnumerable {
        private readonly Dictionary<string, AbstractBElement> InnerValue;

        public BDictionary() {
            InnerValue = new Dictionary<String, AbstractBElement>();
        }

        public BDictionary(Dictionary<String, AbstractBElement> dict) {
            InnerValue = new Dictionary<string, AbstractBElement>(dict);
        }

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