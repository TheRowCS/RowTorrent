using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Exceptions {
    class NullBencodedEntryException : Exception {
        public NullBencodedEntryException() {
        }

        public NullBencodedEntryException(string message) : base(message) {
        }

        public NullBencodedEntryException(String message, Exception inner) : base(message, inner) {
        }
    }
}