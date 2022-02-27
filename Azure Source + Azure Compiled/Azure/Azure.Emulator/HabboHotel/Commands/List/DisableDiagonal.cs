using Azure.HabboHotel.GameClients;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class DisableDiagonal. This class cannot be inherited.
    /// </summary>
    internal sealed class DisableDiagonal : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisableDiagonal"/> class.
        /// </summary>
        public DisableDiagonal()
        {
            MinRank = 1;
            Description = "Desative a Diagonal em seu Quarto.";
            Usage = ":disablediagonal";
            MinParams = 0;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            var room = session.GetHabbo().CurrentRoom;

            room.GetGameMap().DiagonalEnabled = !room.GetGameMap().DiagonalEnabled;
            session.SendNotif(Azure.GetLanguage().GetVar("command_disable_diagonal"));

            return true;
        }
    }
}