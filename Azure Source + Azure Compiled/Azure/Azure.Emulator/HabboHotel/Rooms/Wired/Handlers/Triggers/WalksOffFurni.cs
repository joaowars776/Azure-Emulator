using Azure.HabboHotel.Items;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Azure.HabboHotel.Rooms.Wired.Handlers.Triggers
{
    internal class WalksOffFurni : IWiredItem, IWiredCycler
    {
        private long _mNext;

        public WalksOffFurni(RoomItem item, Room room)
        {
            this.Item = item;
            this.Room = room;
            this.ToWork = new Queue();
            this.Items = new List<RoomItem>();
        }

        public Interaction Type
        {
            get
            {
                return Interaction.TriggerWalkOffFurni;
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

        public Queue ToWork { get; set; }

        public ConcurrentQueue<RoomUser> ToWorkConcurrentQueue { get; set; }

        public bool Execute(params object[] stuff)
        {
            var roomUser = (RoomUser)stuff[0];
            var roomItem = (RoomItem)stuff[1];
            if (!this.Items.Contains(roomItem) || roomUser.LastItem != roomItem.Id)
            {
                return false;
            }
            if (
            roomItem.AffectedTiles.Values.Any(
                current => (current.X == roomUser.X && current.Y == roomUser.Y) || (roomUser.X == roomItem.X && roomUser.Y == roomItem.Y)))
            {
                return false;
            }
            this.ToWork.Enqueue(roomUser);
            if (this.Delay == 0)
            {
                this.OnCycle();
            }
            else
            {
                this._mNext = (Azure.Now() + (this.Delay));

                this.Room.GetWiredHandler().EnqueueCycle(this);
            }
            return true;
        }

        public bool OnCycle()
        {
            long num = Azure.Now();
            if (num <= this._mNext)
            {
                return false;
            }
            lock (this.ToWork.SyncRoot)
            {
                while (this.ToWork.Count > 0)
                {
                    var roomUser = (RoomUser)this.ToWork.Dequeue();
                    List<IWiredItem> conditions = this.Room.GetWiredHandler().GetConditions(this);
                    List<IWiredItem> effects = this.Room.GetWiredHandler().GetEffects(this);
                    if (conditions.Any())
                    {
                        foreach (IWiredItem current in conditions)
                        {
                            if (!current.Execute(new object[]
                            {
                                roomUser
                            }))
                            {
                                return false;
                            }
                            WiredHandler.OnEvent(current);
                        }
                    }
                    if (!effects.Any())
                    {
                        continue;
                    }
                    foreach (IWiredItem current2 in effects.Where(current2 => current2.Execute(new object[]
                    {
                        roomUser,
                        Type
                    })))
                    {
                        WiredHandler.OnEvent(current2);
                    }
                }
            }
            this._mNext = 0L;
            WiredHandler.OnEvent(this);
            return true;
        }
    }
}