using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.bencoding {
    /// <summary>
    /// Represents a Bencoded numeric object, Stored as an Int64.
    /// </summary>
    public class BInteger : AbstractBElement {
        /// <summary>
        /// The original numeric value the object was created with.
        /// </summary>
        public readonly long InnerValue;

        /// <summary>
        /// Creates a new BInteger object.
        /// </summary>
        /// <param name="value">The numeric value to be bencoded.</param>
        public BInteger(long value) {
            InnerValue = value;
        }

        /// <summary>
        /// Bencodes the objects InnerValue.
        /// </summary>
        /// <returns>A bencoded Int64.</returns>
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