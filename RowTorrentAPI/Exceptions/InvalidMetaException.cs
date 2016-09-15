using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Exceptions {
    class InvalidMetaException : Exception {
        public InvalidMetaException() {
        }

        public InvalidMetaException(string message) : base(message) {
        }

        public InvalidMetaException(String message, Exception inner) : base(message, inner) {
        }
    }
}