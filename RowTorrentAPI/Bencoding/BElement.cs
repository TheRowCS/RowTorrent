using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace RowTorrentAPI.bencoding {
    public class BElement {
        public object Value { get; private set; }

        public BElement(byte[] value) {
            Value = value;
        }

        public BElement(string value) {
            Value = Encoding.UTF8.GetBytes(value);
        }

        public BElement(int value) {
            Value = value;
        }

        public BElement(long value) {
            Value = value;
        }

        public BElement(List<BElement> value) {
            Value = value;
        }

        public BElement(Dictionary<String, BElement> value) {
            Value = value;
        }

        public byte[] GetBytes() {
            try {
                return (byte[]) Value;
            }
            catch (Exception e) {
                throw new InvalidCastException(e.Message);
            }
        }

        public String GetString() {
            return Encoding.UTF8.GetString(GetBytes());
        }

        public int GetInt() {
            try {
                return (int) Value;
            }
            catch (Exception e) {
                throw new InvalidCastException(e.Message);
            }
        }

        public long GetLong() {
            try {
                return (long) Value;
            }
            catch (Exception e) {
                throw new InvalidCastException(e.Message);
            }
        }

        public short GetShort() {
            try {
                return (short) Value;
            }
            catch (Exception e) {
                throw new InvalidCastException(e.Message);
            }
        }

        public List<BElement> GetList() {
            try {
                return (List<BElement>) Value;
            }
            catch (Exception e) {
                throw new InvalidCastException(e.Message);
            }
        }

        public Dictionary<string, BElement> GetDictionary() {
            try {
                return (Dictionary<string, BElement>) Value;
            }
            catch (Exception e) {
                throw new InvalidCastException(e.Message);
            }
        }
    }
}