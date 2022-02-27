using System;
using System.Text;
using Azure.HabboHotel.GameClients;
using Azure.Messages;
using Azure.Messages.Parsers;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class About. This class cannot be inherited.
    /// </summary>
    internal sealed class About : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            MinRank = 1;
            Description = "Shows information about the server.";
            Usage = ":about";
            MinParams = 0;
        }

        public override bool Execute(GameClient client, string[] pms)
        {
            var message =
                new ServerMessage(LibraryParser.OutgoingRequest("SuperNotificationMessageComposer"));

            message.AppendString("Azure");
            message.AppendInteger(4);
            message.AppendString("title");
            // Respect Azure Emulator and don't remove the developers credits!
            message.AppendString("Informações do Servidor");
            // Respect Azure Emulator and don't remove the developers credits!
            message.AppendString("message");
            var info = new StringBuilder();
            // Respect Azure Emulator and don't remove the developers credits!
            info.Append("<center> <b><font color=\"#0174DF\" size=\"26\">       Azure</font> <font color=\"#000000\" size=\"18\"> Emulator</font> <font color=\"000000\" size=\"12\"> 2.0 #BETA</font></b></center><br> <font color=\"000000\" size=\"9\" style=\"padding-right:100px;\">                                          Powered by Azure Group</font><br> <b><br />- TimNL         - Jamal        - Diesel        - Boris <br />- Lucca          - Antoine          - Jaden           - IhToN<br />");
            // Respect Azure Emulator and don't remove the developers credits!
            info.Append("<br />");
            message.AppendString(info.ToString());
            message.AppendString("linkUrl");
            message.AppendString("event:");
            message.AppendString("linkTitle");
            message.AppendString("Fechar");
            client.SendMessage(message);

            return true;
        }
    }
}