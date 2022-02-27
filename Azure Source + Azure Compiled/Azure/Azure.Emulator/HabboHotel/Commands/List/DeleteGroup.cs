using Azure.HabboHotel.GameClients;
using System.Linq;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class DeleteGroup. This class cannot be inherited.
    /// </summary>
    internal sealed class DeleteGroup : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteGroup"/> class.
        /// </summary>
        public DeleteGroup()
        {
            MinRank = 1;
            Description = "Apague o seu grupo.";
            Usage = ":deletargrupo";
            MinParams = -2;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            if (!pms.Any() || pms[0].ToLower() != Azure.GetLanguage().GetVar("command_group_yes"))
            {
                session.SendNotif(Azure.GetLanguage().GetVar("command_group_delete_confirm"));
                return true;
            }
            var room = session.GetHabbo().CurrentRoom;
            if (room.RoomData == null || room.RoomData.Group == null)
            {
                session.SendWhisper(Azure.GetLanguage().GetVar("command_group_has_no_room"));
                return true;
            }
            var group = room.RoomData.Group;
            foreach (var user in @group.Members.Values)
            {
                var clientByUserId = Azure.GetGame().GetClientManager().GetClientByUserId(user.Id);
                if (clientByUserId == null) continue;
                clientByUserId.GetHabbo().UserGroups.Remove(user);
                if (clientByUserId.GetHabbo().FavouriteGroup == @group.Id) clientByUserId.GetHabbo().FavouriteGroup = 0;
            }
            room.RoomData.Group = null;
            room.RoomData.GroupId = 0;
            Azure.GetGame().GetGroupManager().DeleteGroup(@group.Id);
            Azure.GetGame().GetRoomManager().UnloadRoom(room, Azure.GetLanguage().GetVar("command_group_remove"));
            return true;
        }
    }
}