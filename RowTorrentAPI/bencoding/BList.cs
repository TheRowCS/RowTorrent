using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RowTorrentAPI.bencoding {
    public class BList : AbstractBElement, IEnumerable {
        /// <summary>
        /// A list of AbstractBElements.
        /// </summary>
        public readonly List<AbstractBElement> InnerValue;

        /// <summary>
        /// Creates an empty BList object.
        /// </summary>
        public BList() {
            InnerValue = new List<AbstractBElement>();
        }

        /// <summary>
        /// Creates a BList object using an existing List of AbstractElements.
        /// </summary>
        /// <param name="elements">
        /// The collection of elements to be stored and bencoded.
        /// </param>
        public BList(List<AbstractBElement> elements) {
            InnerValue = new List<AbstractBElement>(elements);
        }

        /// <summary>
        /// Creates a new list of AbstractBElements from a list of strings.
        /// </summary>
        /// <param name="collection">The list of strings to be bencoded.</param>
        public BList(IEnumerable<string> collection) {
            foreach (var item in collection) {
                InnerValue.Add(new BString(item));
            }
        }

        /// <summary>
        /// Creates a new BList object from a list of Int64.
        /// </summary>
        /// <param name="collection">The list of 64bit ints to be bencoded.</param>
        public BList(IEnumerable<long> collection) {
            foreach (var item in collection) {
                InnerValue.Add(new BInteger(item));
            }
        }

        /// <summary>
        /// Creates a bencode string from the objects list of AbstractBElements.
        /// </summary>
        /// <returns></returns>
        public override string BencodeElement() {
            var str = new StringBuilder();
            str.Append(NodeSymbols.ListStart);
            foreach (AbstractBElement item in InnerValue) {
                str.Append(item.BencodedString);
            }
            str.Append(NodeSymbols.DelimEnd);
            return str.ToString();
        }

        /// <summary>
        /// Adds a new AbstractBElement to the list.
        /// </summary>
        /// <param name="value">Bencoded element to be added.</param>
        public void Add(AbstractBElement value) {
            InnerValue.Add(value);
        }

        /// <summary>
        /// Removes a selected item from the list.
        /// </summary>
        /// <param name="value">The value of the element to be removed.</param>
        /// <returns>The removed value, if it exists.</returns>
        public bool Remove(AbstractBElement value) {
            return InnerValue.Remove(value);
        }

        public IEnumerator<AbstractBElement> GetEnumerator() {
            return InnerValue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return InnerValue.GetEnumerator();
        }

        // Array accessor for list.
        public AbstractBElement this[int key] {
            get { return InnerValue[key]; }
            set { InnerValue[key] = value; }
        }

        public override string ToString() {
            var buff = new StringBuilder();
            buff.Append("list: { ");
            foreach (AbstractBElement el in InnerValue) {
                buff.Append(el + ", ");
            }
            buff.Remove(buff.Length - 2, 2);
            buff.Append(" } ");
            return buff.ToString();
        }
    }
}