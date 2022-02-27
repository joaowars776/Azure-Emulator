using Azure.Collections;
using Azure.HabboHotel.Items;
using Azure.Util;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Azure.HabboHotel.Rooms
{
    /// <summary>
    /// Class GameItemHandler.
    /// </summary>
    internal class GameItemHandler
    {
        /// <summary>
        /// The _banzai teleports
        /// </summary>
        private QueuedDictionary<uint, RoomItem> _banzaiTeleports;

        /// <summary>
        /// The _banzai pyramids
        /// </summary>
        private ConcurrentDictionary<uint, RoomItem> _banzaiPyramids;

        /// <summary>
        /// The _room
        /// </summary>
        private Room _room;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameItemHandler"/> class.
        /// </summary>
        /// <param name="room">The room.</param>
        public GameItemHandler(Room room)
        {
            this._room = room;
            this._banzaiPyramids = new ConcurrentDictionary<uint, RoomItem>();
            this._banzaiTeleports = new QueuedDictionary<uint, RoomItem>();
        }

        /// <summary>
        /// Called when [cycle].
        /// </summary>
        internal void OnCycle()
        {
            this.CyclePyramids();
            this.CycleRandomTeleports();
        }

        /// <summary>
        /// Adds the pyramid.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="itemId">The item identifier.</param>
        internal void AddPyramid(RoomItem item, uint itemId)
        {
            if (this._banzaiPyramids.ContainsKey(itemId))
            {
                this._banzaiPyramids[itemId] = item;
                return;
            }
            this._banzaiPyramids.TryAdd(itemId, item);
        }

        /// <summary>
        /// Removes the pyramid.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        internal void RemovePyramid(uint itemId)
        {
            RoomItem e;
            this._banzaiPyramids.TryRemove(itemId, out e);
        }

        /// <summary>
        /// Adds the teleport.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="itemId">The item identifier.</param>
        internal void AddTeleport(RoomItem item, uint itemId)
        {
            if (this._banzaiTeleports.ContainsKey(itemId))
            {
                this._banzaiTeleports.Inner[itemId] = item;
                return;
            }
            this._banzaiTeleports.Add(itemId, item);
        }

        /// <summary>
        /// Removes the teleport.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        internal void RemoveTeleport(uint itemId)
        {
            this._banzaiTeleports.Remove(itemId);
        }

        /// <summary>
        /// Called when [teleport room user enter].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="item">The item.</param>
        internal void OnTeleportRoomUserEnter(RoomUser user, RoomItem item)
        {
            var items = _banzaiTeleports.Inner.Values.Where(p => p.Id != item.Id).ToList();

            if (!items.Any())
                return;

            var countId = Azure.GetRandomNumber(0, items.Count());
            var countAmount = 0;

            foreach (var current in items.Where(current => current != null))
            {
                if (countAmount != countId)
                {
                    countAmount++;
                    continue;
                }
                current.ExtraData = "1";
                current.UpdateNeeded = true;
                _room.GetGameMap().TeleportToItem(user, current);
                item.ExtraData = "1";
                item.UpdateNeeded = true;
                current.UpdateState();
                item.UpdateState();

                break;
            }
        }

        /// <summary>
        /// Destroys this instance.
        /// </summary>
        internal void Destroy()
        {
            if (this._banzaiTeleports != null)
            {
                this._banzaiTeleports.Destroy();
            }
            if (this._banzaiPyramids != null)
            {
                this._banzaiPyramids.Clear();
            }
            this._banzaiPyramids = null;
            this._banzaiTeleports = null;
            this._room = null;
        }

        /// <summary>
        /// Cycles the pyramids.
        /// </summary>
        private void CyclePyramids()
        {
            foreach (RoomItem item in this._banzaiPyramids.Select(pyramid => pyramid.Value).Where(current => current != null))
            {
                if (item.InteractionCountHelper == 0 && item.ExtraData == "1")
                {
                    this._room.GetGameMap().RemoveFromMap(item, false);
                    item.InteractionCountHelper = 1;
                }
                if (string.IsNullOrEmpty(item.ExtraData))
                    item.ExtraData = "0";

                var randomNumber = Azure.GetRandomNumber(0, 30);
                if (randomNumber <= 26)
                    continue;
                if (item.ExtraData == "0")
                {
                    item.ExtraData = "1";
                    item.UpdateState();
                    _room.GetGameMap().RemoveFromMap(item, false);
                }
                else
                {
                    if (!_room.GetGameMap().ItemCanBePlacedHere(item.X, item.Y))
                        continue;
                    item.ExtraData = "0";
                    item.UpdateState();
                    _room.GetGameMap().AddItemToMap(item, false);
                }
            }
        }

        /// <summary>
        /// Cycles the random teleports.
        /// </summary>
        private void CycleRandomTeleports()
        {
            this._banzaiTeleports.OnCycle();
        }
    }
}