using System;
using System.IO;
using System.Linq;
using System.Text;
using RowTorrentAPI.Exceptions;

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
            return Decode(Encoding.UTF8.GetBytes(bencoded));
        }

        /// <summary>
        /// Decodes an bencoded byte array.
        /// </summary>
        /// <exception cref="Exception">
        /// Throws an exception if byte stream cannot be Decoded
        /// </exception>
        /// <param name="bencoded">An array of chars representing a bencoded string.</param>
        /// <returns>A decoded string.</returns>
        public static AbstractBElement Decode(byte[] bencoded) {
            try {
                using (_stream = new MemoryStream(bencoded)) {
                    using (_binReader = new BinaryReader(_stream)) {
                        return DecodeElement();
                    }
                }
            }
            catch (Exception e) {
                throw new Exception("Unable to Decode stream.", e);
            }
        }

        // Routes bencoded node to the appropriate parsing method
        private static AbstractBElement DecodeElement() {
            switch (GetCurrentNodesType()) {
                case BencodeType.Integer:
                    return DecodeInteger();
                case BencodeType.String:
                    return DecodeString();
                case BencodeType.List:
                    return DecodeList();
                case BencodeType.Dictionary:
                    return DecodeDictionary();
                default:
                    throw new InvalidBencodeNodeException("Unrecognized node type.");
            }
        }

        private static BString DecodeString() {
            if (!char.IsDigit((char) _binReader.PeekChar())) {
                throw new InvalidCastException("Byte could not be cast to char in DecodeString()");
            }

            long length = ParseIntegerValue(NodeSymbols.Colon);
            var byteResult = new byte[length];
            int len = _binReader.Read(byteResult, 0, (int) length);

            if (len != length) {
                throw new IndexOutOfRangeException($"Expected length of {length} from BinaryRead. Got: {len}");
            }
            return new BString(Encoding.UTF8.GetString(byteResult));
        }

        private static BInteger DecodeInteger() {
            if (_binReader.PeekChar() != NodeSymbols.IntStart) {
                throw new UnexpectedNodeException("Expected integer.");
            }
            _binReader.Read();

            long result = ParseIntegerValue(NodeSymbols.DelimEnd);
            return new BInteger(result);
        }

        private static BDictionary DecodeDictionary() {
            var list = new BDictionary();
            char peekChar;
            if (_binReader.PeekChar() != NodeSymbols.DictStart) {
                throw new UnexpectedNodeException("Expected dictionary.");
            }

            _binReader.Read();
            peekChar = CharFromInt(_binReader.PeekChar());
            for (; peekChar != NodeSymbols.DelimEnd; peekChar = CharFromInt(_binReader.PeekChar())) {
                var key = DecodeElement() as BString;
                if (key == null) {
                    throw new InvalidKeyException("Dictionary Key is expected to be a string.");
                }
                list.Add(key, DecodeElement());
            }

            _binReader.Read();

            return list;
        }

        private static BList DecodeList() {
            var list = new BList();
            int peekChar = _binReader.PeekChar();
            if (peekChar != NodeSymbols.ListStart) {
                throw new UnexpectedNodeException("Expected list.");
            }

            _binReader.Read();
            for (; peekChar != NodeSymbols.DelimEnd; peekChar = _binReader.PeekChar()) {
                list.Add(DecodeElement());
            }

            _binReader.Read();
            return list;
        }

        private static long ParseIntegerValue(char delimiter) {
            const byte numBase = 10; // Base 10 conversion.
            long result = 0;
            sbyte signMultiplier = 1; // Indicates a positive or negative result
            if (CharFromInt(_binReader.PeekChar()) == '-') {
                _binReader.Read();
                signMultiplier = -1;
            }
            char c = CharFromInt(_binReader.Read());
            for (; c != delimiter; c = CharFromInt(_binReader.Read())) {
                if (!char.IsDigit(c)) {
                    throw new InvalidCastException("Couldnt cast byte to char in ParseIntegerValue");
                }
                result *= numBase;
                result += NumericValueToLong(char.GetNumericValue(c));
            }
            return result*signMultiplier;
        }

        private static BencodeType GetCurrentNodesType() {
            switch (CharFromInt(_binReader.PeekChar())) {
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
                    throw new UnexpectedNodeException("Unrecognised node used in GetCurretntNodesType");
            }
        }

        // Utility method to avoid casting.
        private static char CharFromInt(int value) {
            return (char) value;
        }

        // Same as CharFromInt.
        private static long NumericValueToLong(double digit) {
            return (long) digit;
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