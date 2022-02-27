using Azure.Messages.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Azure.Messages.Parsers
{
    /// <summary>
    /// Class LibraryParser.
    /// </summary>
    internal static class LibraryParser
    {
        /// <summary>
        /// The incoming
        /// </summary>
        internal static Dictionary<int, StaticRequestHandler> Incoming;

        /// <summary>
        /// The library
        /// </summary>
        internal static Dictionary<string, string> Library;

        /// <summary>
        /// The outgoing
        /// </summary>
        internal static Dictionary<string, int> Outgoing;

        /// <summary>
        /// The configuration
        /// </summary>
        internal static Dictionary<string, string> Config;

        /// <summary>
        /// The count releases
        /// </summary>
        internal static int CountReleases;

        /// <summary>
        /// The release name
        /// </summary>
        internal static string ReleaseName;

        /// <summary>
        /// Delegate ParamLess
        /// </summary>
        public delegate void ParamLess();

        /// <summary>
        /// Delegate StaticRequestHandler
        /// </summary>
        /// <param name="handler">The handler.</param>
        internal delegate void StaticRequestHandler(GameClientMessageHandler handler);

        /// <summary>
        /// Outgoings the request.
        /// </summary>
        /// <param name="packetName">Name of the packet.</param>
        /// <returns>System.Int32.</returns>
        public static int OutgoingRequest(string packetName)
        {
            if (!Outgoing.ContainsKey(packetName))
                return 0;
            var packetId = Convert.ToInt32(Outgoing[packetName]);
            return packetId;
        }

        /// <summary>
        /// Libraries the request.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public static void LibRequest(string handler)
        {
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            Out.WriteLine(string.Format("Loaded {0} Habbo Releases", CountReleases), "Azure.Packets");
            Out.WriteLine(string.Format("Loaded {0} Event Controllers", Incoming.Count), "Azure.Packets");
        }

        /// <summary>
        /// Handles the packet.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="message">The message.</param>
        public static void HandlePacket(GameClientMessageHandler handler, ClientMessage message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            if (Incoming.ContainsKey(message.Id))
            {
                if (Azure.DebugMode)
                {
                    Console.WriteLine();
                    Console.Write("INCOMING ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("HANDLED ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(message.Id + Environment.NewLine + message);
                    if (message.Length > 0)
                        Console.WriteLine();
                    Console.WriteLine();
                }
                var staticRequestHandler = Incoming[message.Id];
                staticRequestHandler(handler);
            }
            else if (Azure.DebugMode)
            {
                Console.WriteLine();
                Console.Write("INCOMING ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("REFUSED ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(message.Id + Environment.NewLine + message);
                if (message.Length > 0)
                    Console.WriteLine();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Reloads the dictionarys.
        /// </summary>
        internal static void ReloadDictionarys()
        {
            Incoming.Clear();
            Outgoing.Clear();
            Library.Clear();
            Config.Clear();

            RegisterLibrary();
            RegisterConfig();
            RegisterIncoming();
            RegisterOutgoing();
        }

        /// <summary>
        /// Registers the incoming.
        /// </summary>
        internal static void RegisterIncoming()
        {
            CountReleases = 0;
            var filePaths = Directory.GetFiles(string.Format("{0}\\Packets", Application.StartupPath), "*.incoming");
            foreach (var fileContents in filePaths.Select(currentFile => File.ReadAllLines(currentFile, Encoding.UTF8)))
            {
                CountReleases++;
                foreach (var fields in fileContents.Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith("[")).Select(line => line.Replace(" ", string.Empty).Split('=')))
                {
                    var packetName = fields[0];
                    if (fields[1].Contains('/')) // anti comments
                        fields[1] = fields[1].Split('/')[0];

                    var packetId = fields[1].ToLower().Contains('x') ? Convert.ToInt32(fields[1], 16) : Convert.ToInt32(fields[1]);
                    if (!Library.ContainsKey(packetName))
                        continue;
                    var libValue = Library[packetName];
                    var del =
                        (PacketLibrary.GetProperty)
                            Delegate.CreateDelegate(typeof(PacketLibrary.GetProperty), typeof(PacketLibrary),
                                libValue);
                    if (Incoming.ContainsKey(packetId))
                        Console.WriteLine(
                            "> A Incoming Packet with Same Id was Founded: " + packetId);
                    else
                        Incoming.Add(packetId, new StaticRequestHandler(del));
                }
            }
        }

        /// <summary>
        /// Registers the configuration.
        /// </summary>
        internal static void RegisterConfig()
        {
            var filePaths = Directory.GetFiles(string.Format("{0}\\Packets", Application.StartupPath), "*.inf");
            foreach (var fields in filePaths.Select(File.ReadAllLines).SelectMany(fileContents => fileContents.Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith("[")).Select(line => line.Split('='))))
            {
                if (fields[1].Contains('/')) // anti comments
                    fields[1] = fields[1].Split('/')[0];

                Config.Add(fields[0], fields[1]);
            }
        }

        /// <summary>
        /// Registers the outgoing.
        /// </summary>
        internal static void RegisterOutgoing()
        {
            var filePaths = Directory.GetFiles(string.Format("{0}\\Packets", Application.StartupPath), "*.outgoing");
            foreach (var fields in filePaths.Select(File.ReadAllLines).SelectMany(fileContents => fileContents.Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith("[")).Select(line => line.Replace(" ", string.Empty).Split('='))))
            {
                if (fields[1].Contains('/')) // anti comments
                    fields[1] = fields[1].Split('/')[0];

                var packetName = fields[0];
                var packetId = int.Parse(fields[1]);
                Outgoing.Add(packetName, packetId);
            }
        }

        /// <summary>
        /// Registers the library.
        /// </summary>
        internal static void RegisterLibrary()
        {
            var filePaths = Directory.GetFiles(string.Format("{0}\\Packets", Application.StartupPath), "*.library");
            foreach (var fields in filePaths.Select(File.ReadAllLines).SelectMany(fileContents => fileContents.Select(line => line.Split('='))))
            {
                if (fields[1].Contains('/')) // anti comments
                    fields[1] = fields[1].Split('/')[0];

                var incomingName = fields[0];
                var libraryName = fields[1];
                Library.Add(incomingName, libraryName);
            }
        }
    }
}