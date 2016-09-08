using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;

namespace RowTorrentAPI.bencoding {
    public static class BDecoder {
        private static BinaryReader _binReader;
        private static Stream _stream;

        /// <summary>
        /// Decodes a bencoded string into its original value.
        /// </summary>
        /// <param name="bencoded">The string to be decoded.</param>
        /// <returns>Returns the decoded string.</returns>
        public static AbstractBElement Decode(string bencoded) {
            return Decode(bencoded.Select(c => (byte) c).ToArray());
        }

        /// <summary>
        /// Decodes an bencoded byte array.
        /// </summary>
        /// <exception cref="Exception">
        /// Throws an exception if byte stream cannot be parsed
        /// </exception>
        /// <param name="bencoded">An array of chars representing a bencoded string.</param>
        /// <returns>A decoded string.</returns>
        public static AbstractBElement Decode(byte[] bencoded) {
            try {
                using (_stream = new MemoryStream(bencoded))
                using (_binReader = new BinaryReader(_stream)) {
                    return ParseElement();
                }
            }
            catch (Exception e) {
                throw new Exception("Unable to parse stream.", e);
            }
        }

        // Routes bencoded node to the appropriate parsing method
        private static AbstractBElement ParseElement() {
            switch (CurrentNodeType()) {
                case BencodeType.Integer:
                    return ParseInteger();
                case BencodeType.String:
                    return ParseString();
                case BencodeType.List:
                    return ParseList();
                case BencodeType.Dictionary:
                    return ParseDictionary();
                default:
                    throw new Exception("Unrecognized node type.");
            }
        }

        private static BDictionary ParseDictionary() {
            var list = new BDictionary();
            if (_binReader.PeekChar() != NodeSymbols.DictStart)
                throw new Exception("Expected dictionary.");

            _binReader.Read();
            while ((char) _binReader.PeekChar() != NodeSymbols.DelimEnd) {
                string key = ParseElement() as BInteger;
                if (key == null) throw new Exception("Key is expected to be a string.");
                list.Add(key, ParseElement());
            }
            _binReader.Read();
            return list;
        }

        private static BList ParseList() {
            var list = new BList();

            if (_binReader.PeekChar() != NodeSymbols.ListStart) {
                throw new Exception("Expected list.");
            }

            _binReader.Read();
            while ((char) _binReader.PeekChar() != NodeSymbols.DelimEnd) {
                list.Add(ParseElement());
            }

            _binReader.Read();
            return list;
        }

        private static BString ParseString() {
            if (!char.IsDigit((char) _binReader.PeekChar()))
                throw new Exception("Expected to read string length.");
            long length = ReadIntegerValue(NodeSymbols.Colon);

            int len;
            var byteResult = new byte[length];
            if ((len = _binReader.Read(byteResult, 0, (int) length)) != length)
                throw new Exception(
                    $"Did not read the expected amount of {length} bytes, {len} instead.");
            return new BString(new string(byteResult.Select(b => (char) b).ToArray()));
        }

        private static BInteger ParseInteger() {
            if (_binReader.PeekChar() != NodeSymbols.IntStart) {
                throw new Exception("Expected integer.");
            }
            _binReader.Read();

            long result = ReadIntegerValue(NodeSymbols.DelimEnd);
            return new BInteger(result);
        }

        private static long ReadIntegerValue(char endChar) {
            char c;
            long result = 0;
            int negative = 1;
            if ((char) _binReader.PeekChar() == '-') {
                _binReader.Read();
                negative = -1;
            }
            while ((c = (char) _binReader.Read()) != endChar) {
                if (!char.IsDigit(c))
                    throw new Exception($"Expected a digit, got '{c}'.");
                result *= 10;
                result += ((long) char.GetNumericValue(c));
            }
            return result*negative;
        }

        private static BencodeType CurrentNodeType() {
            switch ((char) _binReader.PeekChar()) {
                case 'l':
                    return BencodeType.List;
                case 'd':
                    return BencodeType.Dictionary;
                case 'i':
                    return BencodeType.Integer;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return BencodeType.String;
                default:
                    throw new Exception("Temporary exception");
            }
        }
    }

    /// <summary>
    /// Supported bencodable formats.
    /// </summary>
    public enum BencodeType {
        String,
        Integer,
        List,
        Dictionary,
    }

    /// <summary>
    /// A collection of delimiter chars used by bencoding.
    /// </summary>
    public static class NodeSymbols {
        public const char Colon = ':';
        public const char IntStart = 'i';
        public const char ListStart = 'l';
        public const char DictStart = 'd';
        public const char DelimEnd = 'e';
    }
}