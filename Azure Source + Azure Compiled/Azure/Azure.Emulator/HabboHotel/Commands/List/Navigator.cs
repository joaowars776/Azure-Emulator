using Azure.HabboHotel.GameClients;
using Azure.Messages;
using Azure.Messages.Parsers;

namespace Azure.HabboHotel.Commands.List
{
    internal sealed class Navigator : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Navigator"/> class.
        /// </summary>
        public Navigator()
        {
            MinRank = 9;
            Description = "Enable/Disable new navigator.";
            Usage = ":navigator";
            MinParams = 0;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            session.GetHabbo().NewNavigator = !session.GetHabbo().NewNavigator;

            var msg = new ServerMessage(LibraryParser.OutgoingRequest("SendPerkAllowancesMessageComposer"));
            msg.AppendInteger(1);
            msg.AppendString("NAVIGATOR_PHASE_TWO_2014");
            msg.AppendString("");
            msg.AppendBool(session.GetHabbo().NewNavigator);

            session.SendMessage(msg);
            return true;
        }
    }
}
