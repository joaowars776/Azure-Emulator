using Azure.Database.Manager.Database.Session_Details.Interfaces;
using Azure.Messages;
using System.Collections.Generic;

namespace Azure.HabboHotel.Support
{
    /// <summary>
    /// Class SupportTicket.
    /// </summary>
    internal class SupportTicket
    {
        /// <summary>
        /// The score
        /// </summary>
        internal int Score;

        /// <summary>
        /// The category
        /// </summary>
        internal int Category;

        /// <summary>
        /// The type
        /// </summary>
        internal int Type;

        /// <summary>
        /// The status
        /// </summary>
        internal TicketStatus Status;

        /// <summary>
        /// The sender identifier
        /// </summary>
        internal uint SenderId;

        /// <summary>
        /// The reported identifier
        /// </summary>
        internal uint ReportedId;

        /// <summary>
        /// The moderator identifier
        /// </summary>
        internal uint ModeratorId;

        /// <summary>
        /// The message
        /// </summary>
        internal string Message;

        /// <summary>
        /// The room identifier
        /// </summary>
        internal uint RoomId;

        /// <summary>
        /// The room name
        /// </summary>
        internal string RoomName;

        /// <summary>
        /// The timestamp
        /// </summary>
        internal double Timestamp;

        /// <summary>
        /// The reported chats
        /// </summary>
        internal List<string> ReportedChats;

        /// <summary>
        /// The _sender name
        /// </summary>
        private readonly string _senderName;

        /// <summary>
        /// The _reported name
        /// </summary>
        private readonly string _reportedName;

        /// <summary>
        /// The _mod name
        /// </summary>
        private string _modName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportTicket"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="score">The score.</param>
        /// <param name="category">The category.</param>
        /// <param name="type">The type.</param>
        /// <param name="senderId">The sender identifier.</param>
        /// <param name="reportedId">The reported identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="roomId">The room identifier.</param>
        /// <param name="roomName">Name of the room.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="reportedChats">The reported chats.</param>
        internal SupportTicket(uint id, int score, int category, int type, uint senderId, uint reportedId, string message, uint roomId,
            string roomName, double timestamp, List<string> reportedChats)
        {
            this.TicketId = id;
            this.Score = score;
            this.Category = category;
            this.Type = type;
            this.Status = TicketStatus.Open;
            this.SenderId = senderId;
            this.ReportedId = reportedId;
            this.ModeratorId = 0u;
            this.Message = message;
            this.RoomId = roomId;
            this.RoomName = roomName;
            this.Timestamp = timestamp;
            this._senderName = Azure.GetGame().GetClientManager().GetNameById(senderId);
            this._reportedName = Azure.GetGame().GetClientManager().GetNameById(reportedId);
            this._modName = Azure.GetGame().GetClientManager().GetNameById(this.ModeratorId);
            this.ReportedChats = reportedChats;
        }

        /// <summary>
        /// Gets the tab identifier.
        /// </summary>
        /// <value>The tab identifier.</value>
        internal int TabId
        {
            get
            {
                if (this.Status == TicketStatus.Open)
                {
                    return 1;
                }
                if (this.Status == TicketStatus.Picked)
                {
                    return 2;
                }
                if (this.Status == TicketStatus.Abusive || this.Status == TicketStatus.Invalid || this.Status == TicketStatus.Resolved)
                {
                    return 0;
                }
                if (this.Status == TicketStatus.Deleted)
                {
                    return 0;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the ticket identifier.
        /// </summary>
        /// <value>The ticket identifier.</value>
        internal uint TicketId { get; private set; }

        /// <summary>
        /// Picks the specified p moderator identifier.
        /// </summary>
        /// <param name="pModeratorId">The p moderator identifier.</param>
        /// <param name="updateInDb">if set to <c>true</c> [update in database].</param>
        internal void Pick(uint pModeratorId, bool updateInDb)
        {
            this.Status = TicketStatus.Picked;
            this.ModeratorId = pModeratorId;
            this._modName = Azure.GetHabboById(pModeratorId).UserName;
            if (!updateInDb)
            {
                return;
            }
            using (IQueryAdapter queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
            {
                queryReactor.RunFastQuery(string.Concat(new object[]
                {
                    "UPDATE moderation_tickets SET status = 'picked', moderator_id = ",
                    pModeratorId,
                    ", timestamp = '",
                    Azure.GetUnixTimeStamp(),
                    "' WHERE id = ",
                    this.TicketId
                }));
            }
        }

        /// <summary>
        /// Closes the specified new status.
        /// </summary>
        /// <param name="newStatus">The new status.</param>
        /// <param name="updateInDb">if set to <c>true</c> [update in database].</param>
        internal void Close(TicketStatus newStatus, bool updateInDb)
        {
            this.Status = newStatus;
            if (!updateInDb)
            {
                return;
            }
            string text;
            switch (newStatus)
            {
                case TicketStatus.Abusive:
                    text = "abusive";
                    goto IL_41;
                case TicketStatus.Invalid:
                    text = "invalid";
                    goto IL_41;
            }
            text = "resolved";
        IL_41:
            using (IQueryAdapter queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
            {
                queryReactor.RunFastQuery(string.Concat(new object[]
                {
                    "UPDATE moderation_tickets SET status = '",
                    text,
                    "' WHERE id = ",
                    this.TicketId
                }));
            }
        }

        /// <summary>
        /// Releases the specified update in database.
        /// </summary>
        /// <param name="updateInDb">if set to <c>true</c> [update in database].</param>
        internal void Release(bool updateInDb)
        {
            this.Status = TicketStatus.Open;
            if (!updateInDb)
            {
                return;
            }
            using (IQueryAdapter queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
            {
                queryReactor.RunFastQuery(string.Format("UPDATE moderation_tickets SET status = 'open' WHERE id = {0}", this.TicketId));
            }
        }

        /// <summary>
        /// Deletes the specified update in database.
        /// </summary>
        /// <param name="updateInDb">if set to <c>true</c> [update in database].</param>
        internal void Delete(bool updateInDb)
        {
            this.Status = TicketStatus.Deleted;
            if (!updateInDb)
            {
                return;
            }
            using (IQueryAdapter queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
            {
                queryReactor.RunFastQuery(string.Format("UPDATE moderation_tickets SET status = 'deleted' WHERE id = {0}", this.TicketId));
            }
        }

        /// <summary>
        /// Serializes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ServerMessage.</returns>
        internal ServerMessage Serialize(ServerMessage message)
        {
            message.AppendInteger(TicketId); // id
            message.AppendInteger(TabId); // state
            message.AppendInteger(Type); // type (3 or 4 for new style)
            message.AppendInteger(Category);
            message.AppendInteger((Azure.GetUnixTimeStamp() - (int)Timestamp) * 1000); // -->> timestamp
            message.AppendInteger(Score); // priority
            message.AppendInteger(1); // ensures that more tickets of the same reporter/reported user get merged
            message.AppendInteger(SenderId); // sender id 8 ints
            message.AppendString(_senderName); // sender name
            message.AppendInteger(ReportedId);
            message.AppendString(_reportedName);
            message.AppendInteger((Status == TicketStatus.Picked) ? ModeratorId : 0); // mod id
            message.AppendString(_modName); // mod name
            message.AppendString(this.Message); // issue message
            message.AppendInteger(0); // is room public?

            message.AppendInteger(ReportedChats.Count);
            foreach (string str in this.ReportedChats)
            {
                message.AppendString(str);
                message.AppendInteger(-1);
                message.AppendInteger(-1);
            }

            return message;
        }
    }
}