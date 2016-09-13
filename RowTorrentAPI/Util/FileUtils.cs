using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowTorrentAPI.bencoding;

namespace RowTorrentAPI.Util {
    static class FileUtils {
        public static void CheckFileLength(BInteger fileLength) {
            if (fileLength == null) {
                throw new NullReferenceException(nameof(fileLength));
            }
        }

        public static void CheckFileProperties(string[] filePathList, BInteger fileLength) {
            if (filePathList == null || fileLength == null) {
                throw new NullReferenceException();
            }
        }

        public static void CheckInfoFiles(BList files) {
            if (files == null) {
                throw new NullReferenceException(nameof(files));
            }
        }
    }
}