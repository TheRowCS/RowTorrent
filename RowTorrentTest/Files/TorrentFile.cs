using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTFiles = RowTorrentAPI.Files;
using EasyEncryption;
using RowTorrentAPI.bencoding;
using RowTorrentAPI.Util;

namespace RowTorrentTest.Files {
    [TestClass]
    // Currently no tests are designed to pass in this file.
    public class TorrentFile {
        [TestMethod]
        public void Make() {
            string name = "Footorrent";
            string directory = "/usr/bin";
            List<RTFiles.FileData> fileList = new List<RTFiles.FileData>();
            List<byte[]> pieces = new List<byte[]>();
            for (int i = 0; i < 4; i++) {
                fileList.Add(new RTFiles.FileData($"{directory}/x{i}.elf", $"{directory}/x{i}.elf".Length));
                var sha1 = EasyEncryption.SHA.ComputeSHA1Hash(i.ToString());
                pieces.Add(Encoding.UTF8.GetBytes(sha1));
            }
            var announceUrl = "http://www.place";
            var announceList = new List<string>();
            announceList.Add("hello");
            announceList.Add("world");

            string torrFile = RTFiles.TorrentFile.MakeTorrent(
                name, fileList, directory, announceUrl, announceList,
                512, pieces
                );
            foreach (var chars in torrFile.ToCharArray()) {
                Console.Write($"{chars}");
            }
        }
    }
}