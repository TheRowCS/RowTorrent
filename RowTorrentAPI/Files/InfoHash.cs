using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RowTorrentAPI.Files {
    /// <summary>
    /// Represents the infoHash of a torrent file.
    /// </summary>
    public class InfoHash : IEnumerable<byte> {
        private const byte HashSize = 20;

        /// <summary>
        /// The byte data of the InfoHash.
        /// </summary>
        public readonly byte[] InnerValue;

        /// <summary>
        /// The length of the hash size.
        /// </summary>
        public byte Length => HashSize;

        public byte this[byte index] => InnerValue[index];

        /// <summary>
        /// Creates a new InfoHash object.
        /// </summary>
        /// <param name="data">The file hashed as sha1 to be stored.</param>
        public InfoHash(byte[] data) {
            if (data.Length != HashSize) {
                throw new ArgumentOutOfRangeException(nameof(data));
            }
            InnerValue = data;
        }

        public IEnumerator<byte> GetEnumerator() {
            return InnerValue.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}