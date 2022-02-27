using Azure.HabboHotel.GameClients;
using Azure.Messages;
using Azure.Messages.Parsers;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class HotelAlert. This class cannot be inherited.
    /// </summary>
    internal sealed class StaffAlert : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaffAlert"/> class.
        /// </summary>
        public StaffAlert()
        {
            MinRank = 15;
            Description = "Alerta para todos os Staffs.";
            Usage = ":sa [Mensagem]";
            MinParams = -1;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            var msg = string.Join(" ", pms);

            var message =
                new ServerMessage(LibraryParser.OutgoingRequest("SuperNotificationMessageComposer"));
            message.AppendString("staffcloud");
            message.AppendInteger(2);
            message.AppendString("title");
            message.AppendString("Aviso");
            message.AppendString("message");
            message.AppendString(
                string.Format(
                    "",
                    msg, session.GetHabbo().UserName));
            Azure.GetGame().GetClientManager().StaffAlert(message, 0);
            Azure.GetGame()
                .GetModerationTool()
                .LogStaffEntry(session.GetHabbo().UserName, string.Empty, "StaffAlert",
                    string.Format("Aviso Staff [{0}]", msg));

            return true;
        }
    }
}