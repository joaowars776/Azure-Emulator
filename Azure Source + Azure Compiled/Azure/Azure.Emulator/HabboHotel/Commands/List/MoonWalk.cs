using Azure.HabboHotel.GameClients;
using Azure.HabboHotel.Rooms;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class MoonWalk. This class cannot be inherited.
    /// </summary>
    internal sealed class MoonWalk : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoonWalk"/> class.
        /// </summary>
        public MoonWalk()
        {
            MinRank = 1;
            Description = "Andar de costas igual Michael Jackson.";
            Usage = ":moonwalk";
            MinParams = 0;
        }

        /// <summary>
        /// Executes the specified session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="pms">The PMS.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Execute(GameClient session, string[] pms)
        {
            var room = session.GetHabbo().CurrentRoom;
            if (room == null)
                return true;
            //FIXED
            var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            if (user == null)
                return true;

            user.IsMoonwalking = !user.IsMoonwalking;

            session.SendNotif(user.IsMoonwalking ? "Sucesso! Ande de costas.." : "Sucesso! Ande de frente..");
            return true;
        }
    }
}