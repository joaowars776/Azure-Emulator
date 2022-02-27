using Azure.HabboHotel.Items;
using System;

namespace Azure.HabboHotel.Rooms
{
    /// <summary>
    /// Class ItemTriggeredArgs.
    /// </summary>
    public class ItemTriggeredArgs : EventArgs
    {
        /// <summary>
        /// The triggering user
        /// </summary>
        internal readonly RoomUser TriggeringUser;

        /// <summary>
        /// The triggering item
        /// </summary>
        internal readonly RoomItem TriggeringItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTriggeredArgs"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="item">The item.</param>
        public ItemTriggeredArgs(RoomUser user, RoomItem item)
        {
            this.TriggeringUser = user;
            this.TriggeringItem = item;
        }
    }
}