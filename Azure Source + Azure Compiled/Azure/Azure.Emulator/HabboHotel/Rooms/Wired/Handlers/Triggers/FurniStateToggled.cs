using Azure.HabboHotel.Items;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Azure.HabboHotel.Rooms.Wired.Handlers.Triggers
{
    public class FurniStateToggled : IWiredItem, IWiredCycler
    {
        private readonly List<RoomUser> _mUsers;

        private long _mNext;

        public FurniStateToggled(RoomItem item, Room room)
        {
            this.Item = item;
            Room = room;
            this.Items = new List<RoomItem>();
            this.Delay = 0;
            this._mUsers = new List<RoomUser>();
        }

        public Interaction Type
        {
            get
            {
                return Interaction.TriggerStateChanged;
            }
        }

        public RoomItem Item { get; set; }

        public Room Room { get; set; }

        public List<RoomItem> Items { get; set; }

        public int Delay { get; set; }

        public string OtherString
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public string OtherExtraString
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public string OtherExtraString2
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public bool OtherBool
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public Queue ToWork
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public ConcurrentQueue<RoomUser> ToWorkConcurrentQueue { get; set; }

        public bool Execute(params object[] stuff)
        {
            var roomUser = (RoomUser)stuff[0];
            var roomItem = (RoomItem)stuff[1];
            if (roomUser == null || roomItem == null)
            {
                return false;
            }
            if (!this.Items.Contains(roomItem))
            {
                return false;
            }
            this._mUsers.Add(roomUser);
            if (this.Delay == 0)
            {
                WiredHandler.OnEvent(this);
                this.OnCycle();
            }
            else
            {
                if (this._mNext == 0L || this._mNext < Azure.Now())
                {
                    this._mNext = (Azure.Now() + (this.Delay));
                }
                Room.GetWiredHandler().EnqueueCycle(this);
            }
            return true;
        }

        public bool OnCycle()
        {
            long num = Azure.Now();
            if (this._mNext >= num)
            {
                return false;
            }
            List<IWiredItem> conditions = Room.GetWiredHandler().GetConditions(this);
            List<IWiredItem> effects = Room.GetWiredHandler().GetEffects(this);
            foreach (RoomUser current in this._mUsers)
            {
                if (conditions.Any())
                {
                    RoomUser current3 = current;
                    foreach (IWiredItem current2 in conditions.Where(current2 => current2.Execute(new object[]
                    {
                        current3
                    })))
                    {
                        WiredHandler.OnEvent(current2);
                    }
                }
                if (!effects.Any())
                {
                    continue;
                }
                RoomUser current1 = current;
                foreach (IWiredItem current3 in effects.Where(current3 => current3.Execute(new object[]
                {
                    current1,
                    this.Type
                })))
                {
                    WiredHandler.OnEvent(current3);
                }
            }
            WiredHandler.OnEvent(this);
            this._mNext = 0L;
            return true;
        }
    }
}