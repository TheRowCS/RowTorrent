using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Connection {
    public enum TorrentState {
        Failed,
        Seeding,
        Hashing,
        Finished,
        NotRunning,
        Downloading,
        WaitingForDisk,
        WaitingForTracker,
    }
}