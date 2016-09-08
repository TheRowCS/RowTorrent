using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.bencoding {
    /// <summary>
    /// Abstract class representing the behavior of bencoded values.
    /// </summary>
    public abstract class AbstractBElement {
        /// <summary>
        /// Property representing the bencoded string of a value. Alias for BencodedElement().
        /// </summary>
        public string BencodedString => BencodeElement();

        /// <summary>
        /// Returns the value as a bencoded string.
        /// </summary>
        /// <returns></returns>
        public abstract string BencodeElement();
    }
}