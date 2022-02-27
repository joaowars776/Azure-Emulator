using Azure.HabboHotel.Items;
using Azure.HabboHotel.Rooms.Games;
using System.Collections.Generic;

namespace Azure.HabboHotel.Rooms.Wired.Handlers.Effects
{
    public class LeaveTeam : IWiredItem
    {
        //private List<InteractionType> mBanned;
        public LeaveTeam(RoomItem item, Room room)
        {
            this.Item = item;
            Room = room;
            this.OtherString = string.Empty;
            this.OtherExtraString = string.Empty;
            this.OtherExtraString2 = string.Empty;
            //this.mBanned = new List<InteractionType>();
        }

        public Interaction Type
        {
            get
            {
                return Interaction.ActionLeaveTeam;
            }
        }

        public RoomItem Item { get; set; }

        public Room Room { get; set; }

        public List<RoomItem> Items
        {
            get
            {
                return new List<RoomItem>();
            }
            set
            {
            }
        }

        public int Delay
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public string OtherString { get; set; }

        public string OtherExtraString { get; set; }

        public string OtherExtraString2 { get; set; }

        public bool OtherBool { get; set; }

        public bool Execute(params object[] stuff)
        {
            if (stuff[0] == null) return false;
            RoomUser roomUser = (RoomUser)stuff[0];
            TeamManager t = roomUser.GetClient().GetHabbo().CurrentRoom.GetTeamManagerForFreeze();
            if (roomUser.Team != Team.none)
            {
                t.OnUserLeave(roomUser);
                roomUser.Team = Team.none;
            }
            //InteractionType item = (InteractionType)stuff[1];
            return true;
        }
    }
}