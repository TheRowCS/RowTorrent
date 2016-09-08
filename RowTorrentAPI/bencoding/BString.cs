using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.bencoding {
    public class BString : AbstractBElement {
        public readonly string InnerValue;

        public int Length => InnerValue.Length;

        public BString(string value) {
            InnerValue = value;
        }

        public override string BencodeElement() {
            return $"{Length}{NodeSymbols.Colon}{InnerValue}";
        }

        public override string ToString() {
            return InnerValue;
        }

        public override bool Equals(object obj) {
            try {
                BString other = (BString) obj;
                return BencodedString.Equals(other.BencodedString);
            }
            catch (InvalidCastException e) {
                throw new InvalidCastException();
            }
        }

        public override int GetHashCode() {
            return Length;
        }

        public static implicit operator string(BString value) {
            return value.InnerValue;
        }
    }
}