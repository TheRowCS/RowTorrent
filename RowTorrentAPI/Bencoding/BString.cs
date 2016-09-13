using System;

namespace RowTorrentAPI.bencoding {
    /// <summary>
    /// Represents a Bencoded string object.
    /// </summary>
    public class BString : AbstractBElement {
        /// <summary>
        /// The original unencoded string that the object was created with.
        /// </summary>
        public readonly string InnerValue;

        /// <summary>
        /// The length of the original string.
        /// </summary>
        public int Length => InnerValue.Length;

        /// <summary>
        /// Creates a new BString object.
        /// </summary>
        /// <param name="value">The string to be encoded.</param>
        public BString(string value) {
            InnerValue = value;
        }

        /// <summary>
        /// Encodes the objects stored string.
        /// </summary>
        /// <returns>The bencoded string.</returns>
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
                throw new InvalidCastException(e.Message);
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