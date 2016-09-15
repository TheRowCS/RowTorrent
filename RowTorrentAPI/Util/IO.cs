using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Util {
    public class IO {
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

        public static void PrettyPrint(IEnumerable<byte[]> data) {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Data({data.Count()}): {{ ");
            foreach (var outer in data) {
                sb.Append($"({outer.Length}){{");
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