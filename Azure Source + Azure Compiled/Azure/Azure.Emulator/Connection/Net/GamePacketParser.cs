using Azure.Configuration;
using Azure.Connection.Connection;
using Azure.HabboHotel.GameClients;
using Azure.Messages;
using Azure.Messages.Parsers;
using System;

namespace Azure.Connection.Net
{
    /// <summary>
    /// Class GamePacketParser.
    /// </summary>
    public class GamePacketParser : IDataParser
    {
        /// <summary>
        /// The _current client
        /// </summary>
        private readonly GameClient _currentClient;

        /// <summary>
        /// The _con
        /// </summary>
        private ConnectionInformation _con;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePacketParser"/> class.
        /// </summary>
        /// <param name="me">Me.</param>
        internal GamePacketParser(GameClient me)
        {
            _currentClient = me;
        }

        /// <summary>
        /// Delegate HandlePacket
        /// </summary>
        /// <param name="message">The message.</param>
        public delegate void HandlePacket(ClientMessage message);

        /// <summary>
        /// Sets the connection.
        /// </summary>
        /// <param name="con">The con.</param>
        public void SetConnection(ConnectionInformation con)
        {
            _con = con;
        }

        /// <summary>
        /// Handles the packet data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void HandlePacketData(byte[] data)
        {
            if (data.Length == 0 || _currentClient == null) return;
            var i = 0;

            while (i < data.Length)
            {
                if (data.Length - i < 6) return;
                short messageId = 0;
                try
                {
                    var length = HabboEncoding.DecodeInt32(new[]
                    {
                        data[i++],
                        data[i++],
                        data[i++],
                        data[i++]
                    });
                    if (length < 2 || length > 2048) return;
                    messageId = HabboEncoding.DecodeInt16(new[]
                    {
                        data[i++],
                        data[i++]
                    });
                    var packetContent = new byte[length - 2];
                    var num2 = 0;
                    while (num2 < packetContent.Length && i < data.Length)
                    {
                        packetContent[num2] = data[i++];
                        num2++;
                    }
                    using (var clientMessage = new ClientMessage(messageId, packetContent))
                    {
                        _currentClient.GetMessageHandler().HandleRequest(clientMessage);
                    }
                }
                catch (Exception exception)
                {
                    Logging.HandleException(exception, string.Format("packet handling ----> {0}", messageId));
                    _con.Dispose();
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return new GamePacketParser(_currentClient);
        }
    }
}