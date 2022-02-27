using Azure.HabboHotel.GameClients;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class HabNam. This class cannot be inherited.
    /// </summary>
    internal sealed class HabNam : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HabNam"/> class.
        /// </summary>
        public HabNam()
        {
            MinRank = 1;
            Description = "Dancinha divertida.";
            Usage = ":habnam";
            MinParams = 0;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            var room = session.GetHabbo().CurrentRoom;

            var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            session.GetHabbo()
                .GetAvatarEffectsInventoryComponent()
                .ActivateCustomEffect(user != null && user.CurrentEffect != 140 ? 140 : 0);

            return true;
        }
    }
}