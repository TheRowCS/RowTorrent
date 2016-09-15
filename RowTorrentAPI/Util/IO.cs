using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Util {
    public class IO {
        /// <summary>
        /// Converts a byte array to a string of format "Data(length): {val1, val2, .., ..}"
        /// and outputs it to the console.
        /// </summary>
        /// <param name="data">The data to be pretty printed.</param>
        public static void PrettyPrint(IEnumerable<byte> data) {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Data({data.Count()}: {{ ");
            foreach (var value in data) {
                sb.Append($"{value}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(" }");
            Console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Converts a multidimensional byte array to a string of format 
        /// "Data(length): [subLength1]:{val 1, val 2}, [subLength2]:{val1, val2}, ..."
        /// and outputs it to the screen.
        /// </summary>
        /// <param name="data">The data to be prettyPrinted</param>
        public static void PrettyPrint(IEnumerable<byte[]> data) {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Data({data.Count()}): {{ ");
            foreach (var outer in data) {
                sb.Append($"[{outer.Length}]:{{");
                foreach (var inner in outer) {
                    sb.Append($"{inner}, ");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append("}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(" }");
            Console.WriteLine(sb.ToString());
        }

        public static void PrettyPrint(IEnumerable<IEnumerable<byte>> data) {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Data({data.Count()}): {{ ");
            foreach (var outer in data) {
                sb.Append($"({outer.Count()}){{");
                foreach (var inner in outer) {
                    sb.Append($"{inner}, ");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append("}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(" }");
            Console.WriteLine(sb.ToString());
        }
    }
}