using Azure.Configuration;
using System;
using System.Collections.Generic;

namespace Azure.HabboHotel.Rooms
{
    /// <summary>
    /// Class RoomRankConfig.
    /// </summary>
    internal class RoomRankConfig
    {
        /// <summary>
        /// The room s_ t o_ modify
        /// </summary>
        internal List<int> RoomsToModify;

        /// <summary>
        /// The bot s_ defaul t_ color
        /// </summary>
        internal int BotsDefaultColor;

        /// <summary>
        /// The bot s_ defaul t_ badge
        /// </summary>
        internal String BotsDefaultBadge;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            RoomsToModify = new List<int>();

            var roomWithsColors = ConfigurationData.Data["game.roomswithbotscolor"];
            if (string.IsNullOrEmpty(roomWithsColors) && roomWithsColors.Contains(","))
            {
                var v = roomWithsColors.Split(',');
                foreach (var t in v) RoomsToModify.Add(int.Parse(t));
            }
            else RoomsToModify.Add(int.Parse(roomWithsColors));

            BotsDefaultColor = int.Parse(ConfigurationData.Data["game.botdefaultcolor"]);
            BotsDefaultBadge = ConfigurationData.Data["game.botbadge"];
        }
    }
}