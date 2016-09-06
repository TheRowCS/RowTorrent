using System;
using System.Collections.Generic;
using System.IO;

namespace RowTorrentAPI.bencoding {
    public static class BEncoder {
        public static void Bencode(Object o, StreamWriter sw) {       
            if ((Type)o == typeof(Dictionary<string, BElement>)) {
                
            }
            else if ((Type)o == typeof(List<BElement>)) {

            }
            else {
                switch (Type.GetTypeCode(o.GetType())) {
                    case TypeCode.String:
                        break;
                    case TypeCode.Int64:
                        break;
                    case TypeCode.Int32:
                        break;
                    case TypeCode.Int16:
                        break;
                    default:
                        throw new ArgumentException(o.GetType() + "Cannot be bencoded.");
                }
            }
        }
    }
}
