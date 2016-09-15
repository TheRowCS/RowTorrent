using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Exceptions {
    class InvalidBencodeNodeException : Exception {
        public InvalidBencodeNodeException() {
        }

        public InvalidBencodeNodeException(string message) : base(message) {
        }

        public InvalidBencodeNodeException(string message, Exception inner) : base(message, inner) {
        }
    }
}

}