using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowTorrentAPI.bencoding;

namespace RowTorrentTest.bencoding {
    [TestClass]
    public class BElementsTests {
        [TestMethod]
        public void BStringTest() {
            string unencodedString = "hello world";
            string expectedResult = $"11:{unencodedString}";
            var bString = new BString(unencodedString);

            Console.WriteLine(expectedResult);

            Assert.AreEqual(bString.BencodeElement(), expectedResult);
        }

        [TestMethod]
        public void BIntegerTest() {
            int unencodedInt = 10;
            int unencodedNegativeInt = -10;
            long unencodedLong = long.MaxValue;

            string expectedInt = $"i{unencodedInt}e";
            string expectedLong = $"i{long.MaxValue}e";
            string expectedNegative = $"i{unencodedNegativeInt}e";

            var bIntSmall = new BInteger(unencodedInt);
            var bIntLarge = new BInteger(unencodedLong);
            var bIntNegative = new BInteger(unencodedNegativeInt);

            Console.WriteLine(expectedInt);
            Console.WriteLine(expectedLong);
            Console.WriteLine(expectedNegative);

            Assert.AreEqual(expectedInt, bIntSmall.BencodedString);
            Assert.AreEqual(expectedLong, bIntLarge.BencodedString);
            Assert.AreEqual(expectedNegative, bIntNegative.BencodedString);
        }

        [TestMethod]
        public void BListTest() {
            var tempList = new List<AbstractBElement>();
            StringBuilder resultBuilder = new StringBuilder();
            string expectedResult;

            BList bList;

            resultBuilder.Append(NodeSymbols.ListStart);
            for (int i = 0; i < 5; i++) {
                tempList.Add(new BInteger(i));
                resultBuilder.Append($"{NodeSymbols.IntStart}{i}{NodeSymbols.DelimEnd}");
            }

            resultBuilder.Append(NodeSymbols.DelimEnd);
            expectedResult = resultBuilder.ToString();
            bList = new BList(tempList);

            Console.WriteLine(expectedResult);

            Assert.AreEqual(expectedResult, bList.BencodedString);
        }

        [TestMethod]
        public void BDictionaryTest() {
            var tempDict = new Dictionary<string, AbstractBElement>();
            StringBuilder resultBuilder = new StringBuilder();
            string expectedResult;
            BDictionary bDict;

            resultBuilder.Append(NodeSymbols.DictStart);
            for (int i = 0; i < 5; i++) {
                tempDict.Add(i.ToString(), new BInteger(i));
                // Manually bencodes the values to test against.
                resultBuilder.Append(
                    $"{i.ToString().Length}{NodeSymbols.Colon}" +
                    $"{i}{NodeSymbols.IntStart}{i}{NodeSymbols.DelimEnd}"
                    );
            }
            resultBuilder.Append(NodeSymbols.DelimEnd);
            expectedResult = resultBuilder.ToString();
            bDict = new BDictionary(tempDict);

            Console.WriteLine(expectedResult);
            Assert.AreEqual(expectedResult, bDict.BencodedString);
        }
    }
}