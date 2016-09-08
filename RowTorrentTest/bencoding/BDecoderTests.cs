using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowTorrentAPI.bencoding;

namespace RowTorrentTest.bencoding {
    [TestClass]
    public class BDecoderTests {
        [TestMethod]
        public void DecodeString() {
            string encodedString = "3:moo";
            string expectedString = "moo";

            Console.WriteLine(encodedString);
            Console.WriteLine(expectedString);
            Assert.AreEqual(expectedString, BDecoder.Decode(encodedString).ToString());
        }

        [TestMethod]
        public void DecodeInteger() {
            string encodedString = "i3e";
            int expectedResult = 3;
            var parsedInt = Convert.ToInt32(
                BDecoder.Decode(encodedString).ToString());

            Console.WriteLine(encodedString);
            Console.WriteLine(parsedInt);

            Assert.AreEqual(expectedResult, parsedInt);
        }

        [TestMethod]
        public void DecodeList() {
            string bencodedList = "li0ei1ei2ei3ei4ee";
            string expectedResult = "list: { 0, 1, 2, 3, 4 }";
            var result = BDecoder.Decode(bencodedList);

            Console.WriteLine(bencodedList);
            Console.WriteLine(result.ToString());

            Assert.AreEqual(expectedResult, result.ToString());
        }

        [TestMethod]
        public void DecodeDictionary() {
            string bencodedDict = "d1:0i0e1:1i1e1:2i2e1:3i3e1:4i4ee";
            string expectedResult = "dictionary: { 0->0, 1->1, 2->2, 3->3, 4->4 }";
            var result = BDecoder.Decode(bencodedDict);

            Console.WriteLine(bencodedDict);
            Console.WriteLine(result.ToString());
            Assert.AreEqual(expectedResult, result.ToString());
        }
    }
}