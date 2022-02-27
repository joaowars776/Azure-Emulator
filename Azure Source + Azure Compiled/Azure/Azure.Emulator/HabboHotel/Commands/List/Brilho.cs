using Azure.HabboHotel.GameClients;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class Brilho. This class cannot be inherited.
    /// </summary>
    internal sealed class Brilho : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Brilho"/> class.
        /// </summary>
        public Brilho()
        {
            MinRank = 1;
            Description = "Brilhe mais que o sol.";
            Usage = ":brilho";
            MinParams = 0;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            var room = session.GetHabbo().CurrentRoom;

            var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            session.GetHabbo()
                .GetAvatarEffectsInventoryComponent()
                .ActivateCustomEffect(user != null && user.CurrentEffect != 59 ? 59 : 0);

            return true;
        }
    }
}