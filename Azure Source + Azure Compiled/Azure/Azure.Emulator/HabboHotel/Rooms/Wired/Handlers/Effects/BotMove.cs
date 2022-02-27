using Azure.HabboHotel.Items;
using System;
using System.Collections.Generic;

namespace Azure.HabboHotel.Rooms.Wired.Handlers.Effects
{
    public class BotMove : IWiredItem
    {
        //private List<InteractionType> mBanned;
        public BotMove(RoomItem item, Room room)
        {
            this.Item = item;
            this.Items = new List<RoomItem>();
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
                return Interaction.ActionBotMove;
            }
        }

        public RoomItem Item { get; set; }

        public Room Room { get; set; }

        public List<RoomItem> Items { get; set; }

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
            //RoomUser roomUser = (RoomUser)stuff[0];
            //InteractionType item = (InteractionType)stuff[1];
            RoomUser bot = Room.GetRoomUserManager().GetBotByName(OtherString);
            if (bot == null) return false;
            Random rnd = new Random();
            RoomItem goal = Items[rnd.Next(Items.Count)];
            bot.MoveTo(goal.X, goal.Y);
            return true;
        }
    }
}