using Azure.Database.Manager.Database.Session_Details.Interfaces;
using Azure.HabboHotel.GameClients;
using Azure.Messages;
using Azure.Messages.Parsers;
using System;
using System.Collections.Generic;
using System.Data;

namespace Azure.HabboHotel.Rooms
{
    /// <summary>
    /// Class RoomEvents.
    /// </summary>
    internal class RoomEvents
    {
        /// <summary>
        /// The _events
        /// </summary>
        private readonly Dictionary<uint, RoomEvent> _events;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomEvents"/> class.
        /// </summary>
        internal RoomEvents()
        {
            this._events = new Dictionary<uint, RoomEvent>();
            using (IQueryAdapter queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
            {
                queryReactor.SetQuery("SELECT * FROM rooms_events WHERE `expire` > UNIX_TIMESTAMP()");
                DataTable table = queryReactor.GetTable();
                foreach (DataRow dataRow in table.Rows)
                {
                    this._events.Add((uint)dataRow[0],
                        new RoomEvent((uint)dataRow[0], dataRow[1].ToString(), dataRow[2].ToString(), (int)dataRow[3], (int)dataRow[4]));
                }
            }
        }

        /// <summary>
        /// Adds the new event.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="eventDesc">The event desc.</param>
        /// <param name="session">The session.</param>
        /// <param name="time">The time.</param>
        /// <param name="category">The category.</param>
        internal void AddNewEvent(uint roomId, string eventName, string eventDesc, GameClient session, int time = 7200, int category = 1)
        {
            {
                if (this._events.ContainsKey(roomId))
                {
                    RoomEvent roomEvent = this._events[roomId];
                    roomEvent.Name = eventName;
                    roomEvent.Description = eventDesc;
                    if (roomEvent.HasExpired)
                    {
                        roomEvent.Time = Azure.GetUnixTimeStamp() + time;
                    }
                    else
                    {
                        roomEvent.Time += time;
                    }
                    using (IQueryAdapter queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
                    {
                        queryReactor.SetQuery("REPLACE INTO rooms_events VALUES ('@id','@name','@desc','@time','@category')");
                        queryReactor.AddParameter("id", roomId);
                        queryReactor.AddParameter("name", eventName);
                        queryReactor.AddParameter("desc", eventDesc);
                        queryReactor.AddParameter("time", roomEvent.Time);
                        queryReactor.AddParameter("category", category);
                        queryReactor.RunQuery();
                        goto IL_17C;
                    }
                }
                using (IQueryAdapter queryreactor2 = Azure.GetDatabaseManager().GetQueryReactor())
                {
                    queryreactor2.SetQuery(string.Concat(new object[]
                    {
                        "REPLACE INTO rooms_events VALUES (",
                        roomId,
                        ", @name, @desc, ",
                        Azure.GetUnixTimeStamp() + 7200,
                        ", @category)"
                    }));
                    queryreactor2.AddParameter("name", eventName);
                    queryreactor2.AddParameter("desc", eventDesc);
                    queryreactor2.AddParameter("category", category);
                    queryreactor2.RunQuery();
                }
                this._events.Add(roomId, new RoomEvent(roomId, eventName, eventDesc, 0));
            IL_17C:
                Azure.GetGame().GetRoomManager().GenerateRoomData(roomId).Event = this._events[roomId];
                Room room = Azure.GetGame().GetRoomManager().GetRoom(roomId);
                if (room != null)
                {
                    room.RoomData.Event = this._events[roomId];
                }
                if (session.GetHabbo().CurrentRoomId == roomId)
                {
                    this.SerializeEventInfo(roomId);
                }
            }
        }

        /// <summary>
        /// Removes the event.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        internal void RemoveEvent(uint roomId)
        {
            this._events.Remove(roomId);
            this.SerializeEventInfo(roomId);
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns>Dictionary&lt;System.UInt32, RoomEvent&gt;.</returns>
        internal Dictionary<uint, RoomEvent> GetEvents()
        {
            return this._events;
        }

        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        /// <returns>RoomEvent.</returns>
        internal RoomEvent GetEvent(uint roomId)
        {
            return this._events.ContainsKey(roomId) ? this._events[roomId] : null;
        }

        /// <summary>
        /// Rooms the has events.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool RoomHasEvents(uint roomId)
        {
            return this._events.ContainsKey(roomId);
        }

        /// <summary>
        /// Serializes the event information.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        internal void SerializeEventInfo(uint roomId)
        {
            Room room = Azure.GetGame().GetRoomManager().GetRoom(roomId);
            if (room == null)
            {
                return;
            }
            RoomEvent @event = this.GetEvent(roomId);
            if (@event == null || @event.HasExpired)
            {
                return;
            }
            if (!this.RoomHasEvents(roomId))
            {
                return;
            }
            var serverMessage = new ServerMessage();
            serverMessage.Init(LibraryParser.OutgoingRequest("RoomEventMessageComposer"));
            serverMessage.AppendInteger(roomId);
            serverMessage.AppendInteger(room.RoomData.OwnerId);
            serverMessage.AppendString(room.RoomData.Owner);
            serverMessage.AppendInteger(1);
            serverMessage.AppendInteger(1);
            serverMessage.AppendString(@event.Name);
            serverMessage.AppendString(@event.Description);
            serverMessage.AppendInteger(0);
            serverMessage.AppendInteger(
                ((int)Math.Floor((@event.Time - Azure.GetUnixTimeStamp()) / 60.0)));

            serverMessage.AppendInteger(@event.Category);
            room.SendMessage(serverMessage);
        }

        /// <summary>
        /// Updates the event.
        /// </summary>
        /// <param name="Event">The event.</param>
        internal void UpdateEvent(RoomEvent Event)
        {
            using (IQueryAdapter queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
            {
                queryReactor.SetQuery(string.Concat(new object[]
                {
                    "REPLACE INTO rooms_events VALUES (",
                    Event.RoomId,
                    ", @name, @desc, ",
                    Event.Time,
                    ")"
                }));
                queryReactor.AddParameter("name", Event.Name);
                queryReactor.AddParameter("desc", Event.Description);
                queryReactor.RunQuery();
            }
            this.SerializeEventInfo(Event.RoomId);
        }
    }
}