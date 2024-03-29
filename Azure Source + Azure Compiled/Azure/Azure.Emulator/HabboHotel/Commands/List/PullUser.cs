﻿using Azure.HabboHotel.GameClients;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class PullUser. This class cannot be inherited.
    /// </summary>
    internal sealed class PullUser : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PullUser"/> class.
        /// </summary>
        public PullUser()
        {
            MinRank = 5;
            Description = "Pull User.";
            Usage = ":pull [USERNAME]";
            MinParams = 1;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            var room = session.GetHabbo().CurrentRoom;
            var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            if (user == null) return true;

            var client = Azure.GetGame().GetClientManager().GetClientByUserName(pms[0]);
            if (client == null)
            {
                session.SendWhisper(Azure.GetLanguage().GetVar("user_not_found"));
                return true;
            }
            if (client.GetHabbo().Id == session.GetHabbo().Id)
            {
                session.SendWhisper(Azure.GetLanguage().GetVar("command_pull_error_own"));
                return true;
            }
            var user2 = room.GetRoomUserManager().GetRoomUserByHabbo(client.GetHabbo().Id);
            if (user2 == null) return true;
            if (user2.TeleportEnabled)
            {
                session.SendWhisper(Azure.GetLanguage().GetVar("command_error_teleport_enable"));
                return true;
            }

            if (PathFinding.PathFinder.GetDistance(user.X, user.Y, user2.X, user2.Y) > 2)
            {
                session.SendWhisper(Azure.GetLanguage().GetVar("command_pull_error_far_away"));
                return true;
            }
            if ((user.RotBody % 2) != 0) user.RotBody--;
            switch (user.RotBody)
            {
                case 0:
                    user2.MoveTo(user.X, user.Y - 1);
                    break;

                case 2:
                    user2.MoveTo(user.X + 1, user.Y);
                    break;

                case 4:
                    user2.MoveTo(user.X, user.Y + 1);
                    break;

                case 6:
                    user2.MoveTo(user.X - 1, user.Y);
                    break;
            }
            return true;
        }
    }
}