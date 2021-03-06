﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RowTorrentAPI.bencoding;
using RowTorrentAPI.Connection;
using RowTorrentAPI.Exceptions;
using RowTorrentAPI.Util;

//TODO create custom exceptions.

namespace RowTorrentAPI.Files {
    /// <summary>
    /// Class for managing and creating .torrent data.
    /// </summary>
    public class TorrentFile {
        public string Name { get; private set; }
        public byte[] Data { get; private set; }

        /// <summary>
        /// The individual piece size of the torrents packets.
        /// </summary>
        public int PieceSize { get; private set; }

        /// <summary>
        /// The total number of pieces in the torrent.
        /// </summary>
        public int PieceCount { get; private set; }

        /// <summary>
        /// Total size of the torrent.
        /// </summary>
        public int TotalLength { get; private set; }

        public string Tracker { get; private set; }
        public List<string> Trackers { get; private set; }
        public IEnumerable<string> EnumerableTrackers { get; private set; }

        public List<FileData> Files { get; private set; }
        public List<byte[]> Checksums { get; private set; }
        public InfoHash InfoHash { get; private set; }

        private const int CheckSumSize = 20;

        /// <summary>
        /// Creates a new TorrentFile object.
        /// </summary>
        /// <param name="data">The torrents metadata in binary form. </param>
        public TorrentFile(byte[] data) {
            Data = data;

            ReadMetaInfo();
        }

        /// <summary>
        /// Creates a new TorrentFile object.
        /// </summary>
        /// <param name="path">The path to the file to extract metainfo from.</param>
        public TorrentFile(string path)
            : this(File.ReadAllBytes(path)) {
            if (path == null) {
                throw new DirectoryNotFoundException();
            }
        }

        /// <summary>
        /// Creates a bencoded string of metadata representing the .torrent file.
        /// </summary>
        /// <param name="name">The name of the torrent.</param>
        /// <param name="files">A list of FileData objects representing the files to be tracked.</param>
        /// <param name="directory">The parent directory of the torrent.</param>
        /// <param name="tracker">Tracker announce url to use.</param>
        /// <param name="trackers">A list of announce urls</param>
        /// <param name="pieceLength">the size of individual pieces to be used.</param>
        /// <param name="pieces">A list of sha1 hashes representing the files to be delivered.</param>
        /// <returns></returns>
        public static string MakeTorrent(string name, List<FileData> files, string directory, string tracker,
            List<string> trackers, int pieceLength, List<byte[]> pieces) {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (files.Count < 1) throw new ArgumentOutOfRangeException();
            if (tracker == null) throw new ArgumentNullException(nameof(tracker));
            if (pieces == null) throw new ArgumentNullException(nameof(pieces));

            var fileData = new BDictionary();
            var info = new BDictionary();
            var aList = new BList(trackers);

            AddInfo(name, directory, info, pieceLength, files, pieces);
            AddDictFields(fileData, aList, info, tracker);

            return fileData.BencodedString;
        }

        private void LoadMetadata() {
            BDictionary metadata = ReadMetaInfo();

            var announce = metadata["announce"] as BString;

            var info = metadata["info"] as BDictionary;

            //CheckMetadata(announce, info);

            var pieceSize = info["piece length"] as BInteger;

            List<byte[]> checksumList = GetRawChecksums(info, pieceSize);
            BString name = GetNameFromInfo(info);
            List<FileData> decodedFiles = DecodeFiles(info, name);
            if (metadata.ContainsKey("announce-list")) {
                var announceList = (BList) metadata["announce-list"];
                Trackers = DecodeTrackerList(announceList);
            }
            Name = name;
            Files = decodedFiles;
            Tracker = announce;
            Checksums = checksumList;

            PieceSize = (int) pieceSize.InnerValue;
            PieceCount = Checksums.Count;
            TotalLength = (int) Files.Sum(f => f.Length);
            InfoHash = new InfoHash(ComputeInfoHash(info));

            EnumerableTrackers = CreateTrackerList(Tracker, Trackers);
        }

        private IEnumerable<string> CreateTrackerList(string tracker, List<string> trackers) {
            var list = new List<string>(trackers);
            list.Add(Tracker);
            return list;
        }

        private byte[] ComputeInfoHash(BDictionary info) {
            SHA1 infoHasher = SHA1.Create();
            byte[] data = Encoding.ASCII.GetBytes(info.BencodedString);
            byte[] dataHash = infoHasher.ComputeHash(data);
            return dataHash;
        }

        private List<string> DecodeTrackerList(BList trackers) {
            var list = new List<string>();
            foreach (var abstractBElement in trackers) {
                var urls = (BList) abstractBElement;
                list.Add((BString) urls.First());
            }
            return list;
        }

        private List<FileData> DecodeFiles(BDictionary info, BString name) {
            var decodedFileList = new List<FileData>();
            if (info.ContainsKey("files")) {
                DecodeMultipleFiles(info, decodedFileList);
            }
            else {
                DecodeSingleFile(info, name, decodedFileList);
            }
            return decodedFileList;
        }

        private void DecodeMultipleFiles(BDictionary info, List<FileData> decodedFileList) {
            var files = info["files"] as BList;
            FileUtils.CheckInfoFiles(files);
            Debug.Assert(files != null, "files != null");
            foreach (var abstractBElement in files) {
                var file = (BDictionary) abstractBElement;
                FileData torrentFile = CreateTorrentFile(file);
                decodedFileList.Add(torrentFile);
            }
        }

        private void DecodeSingleFile(BDictionary info, BString name, List<FileData> decodedFileList) {
            var fileLength = info["length"] as BInteger;
            FileUtils.CheckFileLength(fileLength);
            decodedFileList.Add(new FileData(name, fileLength.InnerValue));
        }

        private FileData CreateTorrentFile(BDictionary file) {
            var fileLength = file["length"] as BInteger;
            var paths = file["path"] as BList;
            var filePathList = new List<string>();
            string[] filePathArray;

            if (paths == null) throw new NullBencodedEntryException($"{nameof(paths)} list cannot be null");

            foreach (var path in paths) {
                var entry = path as BString;
                filePathList.Add(entry);
            }

            filePathArray = filePathList.ToArray();

            FileUtils.CheckFileProperties(filePathArray, fileLength);

            string filePath = Path.Combine(filePathArray);
            var torrentFile = new FileData(filePath, fileLength.InnerValue);

            return torrentFile;
        }

        private BString GetNameFromInfo(BDictionary info) {
            var name = info["name"] as BString;
            if (name == null) {
                throw new NullBencodedEntryException($"{nameof(Name)}) cannot be null");
            }
            return name;
        }

        private List<byte[]> GetRawChecksums(BDictionary info, BInteger pieceLength) {
            /************************************************************************************/
            /* Splits pieces string into blocks of 20 bytes (the size of the SHA1 of each file.)*/
            /************************************************************************************/
            if (info["pieces"] == null) throw new NullReferenceException();
            var infoPieces = info["pieces"] as BString;
            Debug.Assert(infoPieces != null, "infoPieces != null");
            byte[] rawChecksum = Encoding.ASCII.GetBytes(infoPieces.InnerValue);
            if (pieceLength == null || rawChecksum == null || rawChecksum.Length%CheckSumSize != 0) {
                throw new Exception();
            }

            return SplitPieces(info, pieceLength).ToList();
        }

        private IEnumerable<byte[]> SplitPieces(BDictionary info, BInteger pieceLength) {
            /***********************************************************************************/
            /* Performs the actual splitting of sha1 data.                                     */
            /***********************************************************************************/
            var bString = info[TorrentKeys.InfoPieces] as BString;
            if (bString != null) {
                byte[] rawChecksums = Encoding.ASCII.GetBytes(bString.InnerValue);
                return rawChecksums.Split(20);
            }
            throw new NullBencodedEntryException($"{TorrentKeys.InfoPieces} cannot be null");
        }

        private BDictionary ReadMetaInfo() {
            var metadata = BDecoder.Decode(Data) as BDictionary;
            CheckMetadata(metadata);
            return metadata;
        }

        private void CheckMetadata(BDictionary metadata) {
            if (metadata == null) throw new InvalidMetaException("Invalid metadata.");
            if (!metadata.ContainsKey("announce")) {
                throw new InvalidMetaException("Invalid metadata, 'announce' not found.");
            }
            if (!metadata.ContainsKey("info")) throw new InvalidMetaException("Invalid metadata, 'info' not found.");
        }

        // Adds metadata about how files are treated.
        private static void AddInfo(string torrentName, string directoryRoot, BDictionary info, int pieceLength,
            List<FileData> files, List<byte[]> pieces) {
            info.Add(TorrentKeys.InfoPieceLength, new BInteger(pieceLength));
            var bencodedPieces = new StringBuilder();

            foreach (var piece in pieces) {
                foreach (var data in piece) {
                    bencodedPieces.Append((char) data);
                }
            }
            info.Add(TorrentKeys.InfoPieces, new BString(bencodedPieces.ToString()));

            if (files.Count == 1) {
                info.Add(TorrentKeys.InfoName, new BString(torrentName));
                info.Add(TorrentKeys.InfoFilesLength, new BInteger(files[0].Length));
            }
            else if (files.Count > 1) {
                info.Add(TorrentKeys.InfoName, new BString(directoryRoot));
                var filesList = new BList();

                foreach (FileData inputFile in files) {
                    var aFile = new BDictionary();
                    aFile.Add(TorrentKeys.InfoFilesLength, new BInteger(inputFile.Length));

                    var filePath = new BList(inputFile.Path.Split());
                    aFile.Add(TorrentKeys.InfoFilesPath, filePath);
                    filesList.Add(aFile);
                }
                info.Add(TorrentKeys.InfoFiles, filesList);
            }
            else {
                throw new ArgumentOutOfRangeException(nameof(files));
            }
        }

        // Adds metadata about the client, torrent creator and announce urls.
        private static void AddDictFields(BDictionary fileData, BList alist, BDictionary info, string announceUrl) {
            fileData.Add(TorrentKeys.Announce, new BString(announceUrl));
            fileData.Add(TorrentKeys.AnnounceList, alist);
            fileData.Add(TorrentKeys.Creator, new BString($"RowTorrent/{GlobalData.APIVersion}"));
            fileData.Add(TorrentKeys.CreationDate, new BInteger(Util.Time.GetUnixTime()));
            fileData.Add(TorrentKeys.Encoding, new BString("UTF-8"));
            fileData.Add(TorrentKeys.Info, info);
        }
    }

    /// <summary>
    /// Represents a file inside the torrents root directory.
    /// </summary>
    public class FileData {
        public readonly string Path;
        public string Name => System.IO.Path.GetFileName(Path);
        public readonly long Length;

        public FileData(string path, long length) {
            Path = path;
            Length = length;
        }
    }

    internal static class TorrentKeys {
        // Todo create static collection of dictionary keys used by torrent files.
        public const string Info = "info";
        public const string Announce = "announce";
        public const string AnnounceList = "announce-list";
        public const string CreationDate = "creation date";
        public const string Creator = "created by";
        public const string Comment = "comment"; // optional.
        public const string Encoding = "encoding"; // optional.

        //Info keys
        public const string InfoName = "name";
        public const string InfoPieceLength = "piece length";
        public const string InfoPieces = "pieces";
        public const string InfoPrivate = "private"; // optional.
        public const string InfoFiles = "files";
        public const string InfoFilesLength = "length";
        public const string InfoFilesPath = "path";
    }
}