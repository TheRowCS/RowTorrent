using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Exceptions {
    class InvalidKeyException : Exception {
        public InvalidKeyException() {
        }

        public InvalidKeyException(string message) : base(message) {
        }

        public InvalidKeyException(string message, Exception inner) : base(message, inner) {
        }
    }
}