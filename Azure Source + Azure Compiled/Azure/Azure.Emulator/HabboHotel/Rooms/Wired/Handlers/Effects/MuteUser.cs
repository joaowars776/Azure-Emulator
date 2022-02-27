using Azure.HabboHotel.Items;
using System;
using System.Collections.Generic;

namespace Azure.HabboHotel.Rooms.Wired.Handlers.Effects
{
    public class MuteUser : IWiredItem
    {
        //private List<InteractionType> _mBanned;
        public MuteUser(RoomItem item, Room room)
        {
            this.Item = item;
            Room = room;
            this.OtherString = string.Empty;
            this.OtherExtraString = string.Empty;
            this.OtherExtraString2 = string.Empty;
            this.Delay = 0;
            //_mBanned = new List<InteractionType>();
        }

        public Interaction Type
        {
            get
            {
                return Interaction.ActionMuteUser;
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

        public int Delay { get; set; }

        public string OtherString { get; set; }

        public string OtherExtraString { get; set; }

        public string OtherExtraString2 { get; set; }

        public bool OtherBool { get; set; }

        public bool Execute(params object[] stuff)
        {
            if (stuff[0] == null) return false;
            var roomUser = (RoomUser)stuff[0];

            if (roomUser == null || roomUser.IsBot || roomUser.GetClient() == null ||
                roomUser.GetClient().GetHabbo() == null)
            {
                return false;
            }

            if (roomUser.GetClient().GetHabbo().Rank > 3)
            {
                return false;
            }

            if (this.Delay == 0)
            {
                return false;
            }

            int minutes = this.Delay / 500;
            uint userId = roomUser.GetClient().GetHabbo().Id;

            if (this.Room.MutedUsers.ContainsKey(userId))
            {
                this.Room.MutedUsers.Remove(userId);
            }
            this.Room.MutedUsers.Add(userId, Convert.ToUInt32((Azure.GetUnixTimeStamp() + (minutes * 60))));
            if (!String.IsNullOrEmpty(this.OtherString))
            {
                roomUser.GetClient().SendWhisper(this.OtherString);
            }
            return true;
        }
    }
}