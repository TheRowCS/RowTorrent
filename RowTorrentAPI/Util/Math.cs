using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Util {
    static class Math {
        public static Boolean CheckRange(int lowerBound, int upperBound, int number) {
            return number >= lowerBound && number <= upperBound;
        }

        public static Boolean CheckRange(int lowerBound, int upperBound, string number) {
            int numberValue = Int32.Parse(number);
            return numberValue >= lowerBound && numberValue <= upperBound;
        }
    }
}