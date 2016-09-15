using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace RowTorrentAPI.Util {
    static class StreamHelper {
        public static byte[] ObjectToByteArray(this object obj) {
            if (obj == null) {
                return null;
            }
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream()) {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Object ByteArrayToObject(this byte[] arrBytes) {
            using (var memStream = new MemoryStream()) {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        public static string HexHash(this string hashMe) {
            var sha = new SHA1CryptoServiceProvider();
            string b64 = ByteArrayToString(Encoding.ASCII.GetBytes(hashMe));
            var b64Bytes = Encoding.ASCII.GetBytes(b64);
            var result = sha.ComputeHash(b64Bytes);
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        public static string ByteArrayToString(this byte[] ba) {
            StringBuilder hex = new StringBuilder(ba.Length*2);
            foreach (byte b in ba) {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString().ToLower();
        }

        /// <summary>
        /// Splits an array into several smaller arrays.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array to split.</param>
        /// <param name="size">The size of the smaller arrays.</param>
        /// <returns>An array containing smaller arrays.</returns>
        public static IEnumerable<T[]> Split<T>(this T[] array, int size) {
            for (var i = 0; i < (float) array.Length/size; i++) {
                yield return array.Skip(i*size).Take(size).ToArray();
            }
        }
    }
}