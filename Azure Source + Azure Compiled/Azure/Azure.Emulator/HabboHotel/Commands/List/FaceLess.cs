﻿using Azure.HabboHotel.GameClients;
using Azure.Messages;
using Azure.Messages.Parsers;
using System.Linq;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class FaceLess. This class cannot be inherited.
    /// </summary>
    internal sealed class FaceLess : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceLess"/> class.
        /// </summary>
        public FaceLess()
        {
            MinRank = 1;
            Description = "Fique sem olhos, boca e nariz.";
            Usage = ":faceless";
            MinParams = 0;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            var head = session.GetHabbo().Look.Split('.').FirstOrDefault(element => element.StartsWith("hd-"));
            var color = "1";
            if (!string.IsNullOrEmpty(head))
            {
                color = head.Split('-')[2];
                session.GetHabbo().Look = session.GetHabbo().Look.Replace('.' + head, string.Empty);
            }
            session.GetHabbo().Look += ".hd-99999-" + color;

            using (var dbClient = Azure.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery(
                    "UPDATE users SET look = @look WHERE id = " + session.GetHabbo().Id);
                dbClient.AddParameter("look", session.GetHabbo().Look);
                dbClient.RunQuery();
            }
            var room = session.GetHabbo().CurrentRoom;
            var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            if (user == null) return true;

            var roomUpdate = new ServerMessage(LibraryParser.OutgoingRequest("UpdateUserDataMessageComposer"));
            roomUpdate.AppendInteger(user.VirtualId);
            roomUpdate.AppendString(session.GetHabbo().Look);
            roomUpdate.AppendString(session.GetHabbo().Gender.ToLower());
            roomUpdate.AppendString(session.GetHabbo().Motto);
            roomUpdate.AppendInteger(session.GetHabbo().AchievementPoints);
            room.SendMessage(roomUpdate);

            return true;
        }
    }
}