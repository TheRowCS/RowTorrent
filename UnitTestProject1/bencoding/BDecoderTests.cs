using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowTorrentAPI.bencoding;

namespace UnitTestProject1.bencoding {
    [TestClass]
    public class BDecoderTests {
        [TestMethod]
        public void DecodeString() {
            string encodedString = "3:moo";
            string expectedString = "moo";

            Console.WriteLine(expectedString);
            Assert.AreEqual(expectedString, BDecoder.Decode(encodedString).ToString());
        }
    }
}