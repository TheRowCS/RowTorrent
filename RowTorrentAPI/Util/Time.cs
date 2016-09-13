using System;

namespace RowTorrentAPI.Util {
    public static class Time {
        public static int GetUnixTime() {
            return (int) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}