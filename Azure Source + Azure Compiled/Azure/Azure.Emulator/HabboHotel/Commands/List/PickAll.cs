using Azure.HabboHotel.GameClients;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class PickAll. This class cannot be inherited.
    /// </summary>
    internal sealed class PickAll : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PickAll"/> class.
        /// </summary>
        public PickAll()
        {
            MinRank = -2;
            Description = "Pegar todos os seus mobis do quarto.";
            Usage = ":pickall";
            MinParams = 0;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            var room = session.GetHabbo().CurrentRoom;
            var roomItemList = room.GetRoomItemHandler().RemoveAllFurniture(session);
            if (session.GetHabbo().GetInventoryComponent() == null)
            {
                return true;
            }
            session.GetHabbo().GetInventoryComponent().AddItemArray(roomItemList);
            session.GetHabbo().GetInventoryComponent().UpdateItems(false);
            return true;
        }
    }
}