﻿using Azure.HabboHotel.GameClients;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class RefreshRanks. This class cannot be inherited.
    /// </summary>
    internal sealed class RefreshRanks : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshRanks"/> class.
        /// </summary>
        public RefreshRanks()
        {
            MinRank = 7;
            Description = "Refreshes Ranks from Database.";
            Usage = ":refresh_ranks";
            MinParams = 0;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            using (var adapter = Azure.GetDatabaseManager().GetQueryReactor()) Azure.GetGame().GetRoleManager().LoadRights(adapter);
            CommandsManager.UpdateInfo();
            session.SendNotif(Azure.GetLanguage().GetVar("command_refresh_ranks"));
            return true;
        }
    }
}