using Azure.HabboHotel.Items;
using System.Collections.Generic;

namespace Azure.HabboHotel.Rooms.Wired
{
    public interface IWiredItem
    {
        Interaction Type { get; }

        RoomItem Item { get; set; }

        Room Room { get; set; }

        List<RoomItem> Items { get; set; }

        string OtherString { get; set; }

        bool OtherBool { get; set; }

        string OtherExtraString { get; set; }

        string OtherExtraString2 { get; set; }

        int Delay { get; set; }

        bool Execute(params object[] stuff);
    }
}