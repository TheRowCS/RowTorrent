using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Connection {
    /// <summary>
    /// Globally available information about the client and utilities for connecting via the web.
    /// </summary>
    public class GlobalData {
        public readonly string Version;
        public static string APIVersion = "0.0.1";
        public string TargetDirectory { get; set; }
        public readonly int BlockSize;

        public string PeerId { get; private set; }
        public const short Port = 6882;
        public Socket Listener { get; private set; }

        public static GlobalData Instance => ThreadSafeInstance();

        private const byte IdLength = 20;
        private readonly string _userAgent;

        private static GlobalData _globalInstance = new GlobalData();
        private static readonly object _syncRoot = new object();
        private readonly Random _rng;

        private GlobalData() {
            Version = APIVersion;
            _userAgent = $"-ROW{Version.Replace(".", String.Empty)}-";
            _rng = new Random(1024);
            BlockSize = 1024*16;
            int seed = DateTime.Now.Millisecond + DateTime.Now.Minute + DateTime.Now.Hour + Port;
            _rng = new Random(seed);
            PeerId = Encoding.UTF8.GetString(GeneratePeerId());
            BindSocket();
        }

        //I hope this actually is threadsafe
        private static GlobalData ThreadSafeInstance() {
            if (_globalInstance == null) {
                lock (_syncRoot) {
                    _globalInstance = new GlobalData();
                }
            }
            return _globalInstance;
        }

        public int NextRandom(int max) {
            return NextRandom(0, max);
        }

        public int NextRandom(int min, int max) {
            lock (_rng) {
                return _rng.Next(min, max);
            }
        }

        private void BindSocket() {
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(new IPEndPoint(IPAddress.Any, Port));
            Listener.Listen(10);
        }

        private byte[] GeneratePeerId() {
            var id = new List<byte>(IdLength);
            lock (_rng) {
                id.AddRange(Encoding.UTF8.GetBytes(_userAgent));
                var tempRange = Enumerable.Repeat((byte) 0, IdLength - _userAgent.Length).ToArray();
                for (int i = 0; i < tempRange.Count(); i++) {
                    tempRange[i] = (byte) NextRandom('0', 'z');
                }
                id.AddRange(tempRange);
            }
            return id.ToArray();
        }
    }
}