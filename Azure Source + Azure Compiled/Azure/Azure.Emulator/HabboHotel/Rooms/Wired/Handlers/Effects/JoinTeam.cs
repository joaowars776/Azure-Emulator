using Azure.HabboHotel.Items;
using Azure.HabboHotel.Rooms.Games;
using System.Collections.Generic;

namespace Azure.HabboHotel.Rooms.Wired.Handlers.Effects
{
    public class JoinTeam : IWiredItem
    {
        private int _mDelay;

        //private List<InteractionType> mBanned;
        public JoinTeam(RoomItem item, Room room)
        {
            this.Item = item;
            Room = room;
            this._mDelay = 0;
            //this.mBanned = new List<InteractionType>();
        }

        public Interaction Type
        {
            get
            {
                return Interaction.ActionJoinTeam;
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
                return _mDelay;
            }
            set
            {
                _mDelay = value;
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
            int team = this._mDelay / 500;
            TeamManager t = roomUser.GetClient().GetHabbo().CurrentRoom.GetTeamManagerForFreeze();
            if (roomUser.Team != Team.none)
            {
                t.OnUserLeave(roomUser);
                roomUser.Team = Team.none;
            }
            switch (team)
            {
                case 1:
                    roomUser.Team = Games.Team.red;
                    break;

                case 2:
                    roomUser.Team = Games.Team.green;
                    break;

                case 3:
                    roomUser.Team = Games.Team.blue;
                    break;

                case 4:
                    roomUser.Team = Games.Team.yellow;
                    break;
            }
            t.AddUser(roomUser);
            roomUser.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(Delay + 39);
            //InteractionType item = (InteractionType)stuff[1];

            return true;
        }
    }
}