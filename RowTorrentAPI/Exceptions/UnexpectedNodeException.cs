using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Exceptions {
    class UnexpectedNodeException : Exception {
        public UnexpectedNodeException() {
        }

        public UnexpectedNodeException(string message) : base(message) {
        }

        public UnexpectedNodeException(string message, Exception inner) : base(message, inner) {
        }
    }
}