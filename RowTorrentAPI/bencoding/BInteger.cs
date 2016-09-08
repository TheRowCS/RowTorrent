using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.bencoding {
    public class BInteger : AbstractBElement {
        public readonly long InnerValue;

        public BInteger(long value) {
            InnerValue = value;
        }

        public override string BencodeElement() {
            return string.Format(NodeSymbols.IntStart + InnerValue.ToString()
                                 + NodeSymbols.DelimEnd);
        }

        public override string ToString() {
            return InnerValue.ToString();
        }

        public static implicit operator string(BInteger value) {
            return value.InnerValue.ToString();
        }
    }
}