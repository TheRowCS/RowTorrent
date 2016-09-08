using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RowTorrentAPI.bencoding {
    public class BList : AbstractBElement, IEnumerable {
        public readonly List<AbstractBElement> InnerValue;

        public BList() {
            InnerValue = new List<AbstractBElement>();
        }

        public BList(List<AbstractBElement> elements) {
            InnerValue = new List<AbstractBElement>(elements);
        }

        public BList(IEnumerable<string> collection) {
            foreach (var item in collection) {
                InnerValue.Add(new BString(item));
            }
        }

        public BList(IEnumerable<long> collection) {
            foreach (var item in collection) {
                InnerValue.Add(new BInteger(item));
            }
        }

        public override string BencodeElement() {
            var str = new StringBuilder();
            str.Append(NodeSymbols.ListStart);
            foreach (AbstractBElement item in InnerValue) {
                str.Append(item.BencodedString);
            }
            str.Append(NodeSymbols.DelimEnd);
            return str.ToString();
        }

        public IEnumerator<AbstractBElement> GetEnumerator() {
            return InnerValue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return InnerValue.GetEnumerator();
        }

        public void Add(AbstractBElement value) {
            InnerValue.Add(value);
        }

        public bool Remove(AbstractBElement value) {
            return InnerValue.Remove(value);
        }

        public override string ToString() {
            var buff = new StringBuilder();
            buff.Append("list: { ");
            foreach (AbstractBElement el in InnerValue)
                buff.Append(el + ", ");
            buff.Remove(buff.Length - 2, 2);
            buff.Append(" } ");
            return buff.ToString();
        }
    }
}