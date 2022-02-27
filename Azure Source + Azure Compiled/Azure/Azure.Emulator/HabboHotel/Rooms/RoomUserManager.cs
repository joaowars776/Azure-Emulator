using Azure.Collections;
using Azure.Configuration;
using Azure.Database.Manager.Database.Session_Details.Interfaces;
using Azure.HabboHotel.GameClients;
using Azure.HabboHotel.Items;
using Azure.HabboHotel.Navigators;
using Azure.HabboHotel.Pathfinding;
using Azure.HabboHotel.PathFinding;
using Azure.HabboHotel.Pets;
using Azure.HabboHotel.Quests;
using Azure.HabboHotel.RoomBots;
using Azure.HabboHotel.Rooms.Games;
using Azure.Messages;
using Azure.Messages.Parsers;
using Azure.Util;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace Azure.HabboHotel.Rooms
{
    /// <summary>
    /// Class RoomUserManager.
    /// </summary>
    internal class RoomUserManager
    {
        /// <summary>
        /// The users by user name
        /// </summary>
        internal HybridDictionary UsersByUserName;

        /// <summary>
        /// The users by user identifier
        /// </summary>
        internal HybridDictionary UsersByUserId;

        /// <summary>
        /// To set
        /// </summary>
        internal Dictionary<Point, RoomUser> ToSet;

        /// <summary>
        /// The _to remove
        /// </summary>
        private readonly List<RoomUser> _toRemove;

        /// <summary>
        /// The _room
        /// </summary>
        private Room _room;

        /// <summary>
        /// The _pets
        /// </summary>
        private HybridDictionary _pets;

        /// <summary>
        /// The _bots
        /// </summary>
        private HybridDictionary _bots;

        /// <summary>
        /// The _user count
        /// </summary>
        private uint _userCount;

        /// <summary>
        /// The _primary private user identifier
        /// </summary>
        private int _primaryPrivateUserId;

        /// <summary>
        /// The _secondary private user identifier
        /// </summary>
        private int _secondaryPrivateUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomUserManager"/> class.
        /// </summary>
        /// <param name="room">The room.</param>
        public RoomUserManager(Room room)
        {
            _room = room;
            UserList = new ConcurrentDictionary<int, RoomUser>();
            _pets = new HybridDictionary();
            _bots = new HybridDictionary();
            UsersByUserName = new HybridDictionary();
            UsersByUserId = new HybridDictionary();
            _primaryPrivateUserId = 0;
            _secondaryPrivateUserId = 0;
            _toRemove = new List<RoomUser>((int)room.RoomData.UsersMax);
            ToSet = new Dictionary<Point, RoomUser>();
            PetCount = 0;
            _userCount = 0;
        }

        internal event RoomEventDelegate OnUserEnter;

        /// <summary>
        /// Gets the pet count.
        /// </summary>
        /// <value>The pet count.</value>
        internal int PetCount { get; private set; }

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <value>The user list.</value>
        internal ConcurrentDictionary<int, RoomUser> UserList { get; private set; }

        /// <summary>
        /// Gets the room user by habbo.
        /// </summary>
        /// <param name="pId">The p identifier.</param>
        /// <returns>RoomUser.</returns>
        public RoomUser GetRoomUserByHabbo(uint pId)
        {
            return UsersByUserId.Contains(pId) ? (RoomUser)UsersByUserId[pId] : null;
        }

        /// <summary>
        /// Gets the room user count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        internal int GetRoomUserCount()
        {
            return (UserList.Count - _bots.Count - _pets.Count);
        }

        /// <summary>
        /// Deploys the bot.
        /// </summary>
        /// <param name="bot">The bot.</param>
        /// <param name="petData">The pet data.</param>
        /// <returns>RoomUser.</returns>
        internal RoomUser DeployBot(RoomBot bot, Pet petData)
        {
            var virtualId = _primaryPrivateUserId++;
            var roomUser = new RoomUser(0u, _room.RoomId, virtualId, _room, false);
            var num = _secondaryPrivateUserId++;
            roomUser.InternalRoomId = num;
            UserList.TryAdd(num, roomUser);
            OnUserAdd(roomUser);

            var model = _room.GetGameMap().Model;
            var coord = new Point(bot.X, bot.Y);
            if (bot.X > 0 && bot.Y > 0 && bot.X < model.MapSizeX && bot.Y < model.MapSizeY)
            {
                _room.GetGameMap().AddUserToMap(roomUser, coord);
                roomUser.SetPos(bot.X, bot.Y, bot.Z);
                roomUser.SetRot(bot.Rot, false);
            }
            else
            {
                bot.X = model.DoorX;
                bot.Y = model.DoorY;
                roomUser.SetPos(model.DoorX, model.DoorY, model.DoorZ);
                roomUser.SetRot(model.DoorOrientation, false);
            }

            bot.RoomUser = roomUser;
            roomUser.BotData = bot;

            {
                roomUser.BotAI = bot.GenerateBotAI(roomUser.VirtualId, (int)bot.BotId);
                if (roomUser.IsPet)
                {
                    roomUser.BotAI.Init(bot.BotId, roomUser.VirtualId, _room.RoomId, roomUser, _room);
                    roomUser.PetData = petData;
                    roomUser.PetData.VirtualId = roomUser.VirtualId;
                }
                else
                    roomUser.BotAI.Init(bot.BotId, roomUser.VirtualId, _room.RoomId, roomUser, _room);

                UpdateUserStatus(roomUser, false);
                roomUser.UpdateNeeded = true;
                var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("SetRoomUserMessageComposer"));
                serverMessage.AppendInteger(1);
                roomUser.Serialize(serverMessage, _room.GetGameMap().GotPublicPool);
                _room.SendMessage(serverMessage);
                roomUser.BotAI.OnSelfEnterRoom();
                if (roomUser.IsPet)
                {
                    if (_pets.Contains(roomUser.PetData.PetId))
                        _pets[roomUser.PetData.PetId] = roomUser;
                    else
                        _pets.Add(roomUser.PetData.PetId, roomUser);
                    PetCount++;
                }

                roomUser.BotAI.Modified();
                if (roomUser.BotData.AiType != AIType.Generic)
                    return roomUser;
                if (_bots.Contains(roomUser.BotData.BotId))
                    _bots[roomUser.BotData.BotId] = roomUser;
                else
                    _bots.Add(roomUser.BotData.BotId, roomUser);
                serverMessage.Init(LibraryParser.OutgoingRequest("DanceStatusMessageComposer"));
                serverMessage.AppendInteger(roomUser.VirtualId);
                serverMessage.AppendInteger(roomUser.BotData.DanceId);
                _room.SendMessage(serverMessage);
                PetCount++;

                return roomUser;
            }
        }

        /// <summary>
        /// Updates the bot.
        /// </summary>
        /// <param name="virtualId">The virtual identifier.</param>
        /// <param name="roomUser">The room user.</param>
        /// <param name="name">The name.</param>
        /// <param name="motto">The motto.</param>
        /// <param name="look">The look.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="speech">The speech.</param>
        /// <param name="responses">The responses.</param>
        /// <param name="speak">if set to <c>true</c> [speak].</param>
        /// <param name="speechDelay">The speech delay.</param>
        /// <param name="mix">if set to <c>true</c> [mix].</param>
        internal void UpdateBot(int virtualId, RoomUser roomUser, string name, string motto, string look, string gender,
            List<string> speech, List<string> responses, bool speak, int speechDelay, bool mix)
        {
            var bot = GetRoomUserByVirtualId(virtualId);
            if (bot == null || !bot.IsBot) return;

            var rBot = bot.BotData;

            rBot.Name = name;
            rBot.Motto = motto;
            rBot.Look = look;
            rBot.Gender = gender;
            rBot.RandomSpeech = speech;
            rBot.Responses = responses;
            rBot.AutomaticChat = speak;
            rBot.SpeechInterval = speechDelay;
            rBot.RoomUser = roomUser;
            rBot.MixPhrases = mix;

            if (rBot.RoomUser == null || rBot.RoomUser.BotAI == null) return;

            rBot.RoomUser.BotAI.Modified();
        }

        /// <summary>
        /// Removes the bot.
        /// </summary>
        /// <param name="virtualId">The virtual identifier.</param>
        /// <param name="kicked">if set to <c>true</c> [kicked].</param>
        internal void RemoveBot(int virtualId, bool kicked)
        {
            var roomUserByVirtualId = GetRoomUserByVirtualId(virtualId); 
            if (roomUserByVirtualId == null || !roomUserByVirtualId.IsBot) return;

            if (roomUserByVirtualId.IsPet)
            {
                _pets.Remove(roomUserByVirtualId.PetData.PetId);
                PetCount--;
            }
            roomUserByVirtualId.BotAI.OnSelfLeaveRoom(kicked);
            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("UserLeftRoomMessageComposer"));
            serverMessage.AppendString(roomUserByVirtualId.VirtualId.ToString());
            _room.SendMessage(serverMessage);

            RoomUser roomUser;
            UserList.TryRemove(roomUserByVirtualId.InternalRoomId, out roomUser);
        }

        /// <summary>
        /// Gets the user for square.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>RoomUser.</returns>
        internal RoomUser GetUserForSquare(int x, int y)
        {
            return _room.GetGameMap().GetRoomUsers(new Point(x, y)).FirstOrDefault();
        }

        /// <summary>
        /// Adds the user to room.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="spectator">if set to <c>true</c> [spectator].</param>
        /// <param name="snow">if set to <c>true</c> [snow].</param>
        internal void AddUserToRoom(GameClient session, bool spectator, bool snow = false)
        {
            if (session == null || session.GetHabbo() == null)
                return;
            var roomUser = new RoomUser(session.GetHabbo().Id, _room.RoomId, _primaryPrivateUserId++, _room, spectator);
            if (roomUser.GetClient() == null || roomUser.GetClient().GetHabbo() == null)
                return;

            roomUser.UserId = session.GetHabbo().Id;
            var userName = session.GetHabbo().UserName;
            var userId = roomUser.UserId;
            if (UsersByUserName.Contains(userName.ToLower()))
                UsersByUserName.Remove(userName.ToLower());
            if (UsersByUserId.Contains(userId))
                UsersByUserId.Remove(userId);
            UsersByUserName.Add(session.GetHabbo().UserName.ToLower(), roomUser);
            UsersByUserId.Add(session.GetHabbo().Id, roomUser);
            var num = _secondaryPrivateUserId++;
            roomUser.InternalRoomId = num;
            session.CurrentRoomUserId = num;
            session.GetHabbo().CurrentRoomId = _room.RoomId;
            UserList.TryAdd(num, roomUser);
            OnUserAdd(roomUser);

            session.GetHabbo().LoadingRoom = 0;

            if (Azure.GetGame().GetNavigator().PrivateCategories.Contains(_room.RoomData.Category))
                ((FlatCat)Azure.GetGame().GetNavigator().PrivateCategories[_room.RoomData.Category]).UsersNow++;
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        internal void UpdateUser(string oldName, string newName)
        {
            if (oldName == newName)
                return;

            if (!UsersByUserName.Contains(oldName))
                return;
            UsersByUserName.Add(newName, UsersByUserName[oldName]);
            UsersByUserName.Remove(oldName);
            //
            Azure.GetGame().GetClientManager().UpdateClient(oldName, newName);
        }

        /// <summary>
        /// Removes the user from room.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="notifyClient">if set to <c>true</c> [notify client].</param>
        /// <param name="notifyKick">if set to <c>true</c> [notify kick].</param>
        internal void RemoveUserFromRoom(GameClient session, bool notifyClient, bool notifyKick)
        {
            try
            {
                if (session == null || session.GetHabbo() == null || _room == null)
                    return;
                var userId = session.GetHabbo().Id;

                session.GetHabbo().GetAvatarEffectsInventoryComponent().OnRoomExit();
                //using (var queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
                //    queryReactor.RunFastQuery("UPDATE users_rooms_visits SET exit_timestamp = '" + Azure.GetUnixTimeStamp() + "' WHERE room_id = '" + _room.RoomId + "' AND user_id = '" + session.GetHabbo().Id + "' ORDER BY entry_timestamp DESC LIMIT 1");

                var roomUserByHabbo = GetRoomUserByHabbo(userId);
                if (roomUserByHabbo == null)
                    return;
                if (notifyKick)
                {
                    var room = Azure.GetGame().GetRoomManager().GetRoom(roomUserByHabbo.RoomId);
                    var model = room.GetGameMap().Model;
                    roomUserByHabbo.MoveTo(model.DoorX, model.DoorY);
                    roomUserByHabbo.CanWalk = false;
                    session.GetMessageHandler()
                        .GetResponse()
                        .Init(LibraryParser.OutgoingRequest("RoomErrorMessageComposer"));
                    session.GetMessageHandler().GetResponse().AppendInteger(4008);
                    session.GetMessageHandler().SendResponse();

                    session.GetMessageHandler()
                        .GetResponse()
                        .Init(LibraryParser.OutgoingRequest("OutOfRoomMessageComposer"));
                    session.GetMessageHandler().GetResponse().AppendShort(2);
                    session.GetMessageHandler().SendResponse();
                }
                else if (notifyClient)
                {
                    var serverMessage =
                        new ServerMessage(LibraryParser.OutgoingRequest("UserIsPlayingFreezeMessageComposer"));
                    serverMessage.AppendBool(roomUserByHabbo.Team != Team.none);
                    roomUserByHabbo.GetClient().SendMessage(serverMessage);
                    session.GetMessageHandler()
                        .GetResponse()
                        .Init(LibraryParser.OutgoingRequest("OutOfRoomMessageComposer"));
                    session.GetMessageHandler().GetResponse().AppendShort(2);
                    session.GetMessageHandler().SendResponse();
                }
                if (roomUserByHabbo.Team != Team.none)
                {
                    _room.GetTeamManagerForBanzai().OnUserLeave(roomUserByHabbo);
                    _room.GetTeamManagerForFreeze().OnUserLeave(roomUserByHabbo);
                }
                if (roomUserByHabbo.RidingHorse)
                {
                    roomUserByHabbo.RidingHorse = false;
                    var horse = GetRoomUserByVirtualId((int)roomUserByHabbo.HorseId);
                    if (horse != null)
                    {
                        horse.RidingHorse = false;
                        horse.HorseId = 0u;
                    }
                }
                if (roomUserByHabbo.IsLyingDown || roomUserByHabbo.IsSitting)
                {
                    roomUserByHabbo.IsSitting = false;
                    roomUserByHabbo.IsLyingDown = false;
                }
                RemoveRoomUser(roomUserByHabbo);
                if (session.GetHabbo() != null && !roomUserByHabbo.IsSpectator)
                {
                    if (roomUserByHabbo.CurrentItemEffect != ItemEffectType.None)
                        roomUserByHabbo.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect = -1;
                    if (session.GetHabbo() != null)
                    {
                        if (_room.HasActiveTrade(session.GetHabbo().Id))
                            _room.TryStopTrade(session.GetHabbo().Id);
                        session.GetHabbo().CurrentRoomId = 0;
                        if (session.GetHabbo().GetMessenger() != null)
                            session.GetHabbo().GetMessenger().OnStatusChanged(true);
                    }

                    using (var queryreactor2 = Azure.GetDatabaseManager().GetQueryReactor())
                        if (session.GetHabbo() != null)
                            queryreactor2.RunFastQuery(string.Concat(new object[]
                            {
                                "UPDATE users_rooms_visits SET exit_timestamp = '",
                                Azure.GetUnixTimeStamp(),
                                "' WHERE room_id = '",
                                _room.RoomId,
                                "' AND user_id = '",
                                userId,
                                "' ORDER BY exit_timestamp DESC LIMIT 1"
                            }));
                }
                UsersByUserId.Remove(roomUserByHabbo.UserId);
                if (session.GetHabbo() != null)
                    UsersByUserName.Remove(session.GetHabbo().UserName.ToLower());
                roomUserByHabbo.Dispose();
            }
            catch (Exception ex)
            {
                Logging.LogCriticalException(string.Format("Error during removing user from room:{0}", ex));
            }
        }

        /// <summary>
        /// Removes the room user.
        /// </summary>
        /// <param name="user">The user.</param>
        internal void RemoveRoomUser(RoomUser user)
        {
            RoomUser junk;
            if (!UserList.TryRemove(user.InternalRoomId, out junk)) return;

            user.InternalRoomId = -1;
            _room.GetGameMap().GameMap[user.X, user.Y] = user.SqState;
            _room.GetGameMap().RemoveUserFromMap(user, new Point(user.X, user.Y));

            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("UserLeftRoomMessageComposer"));
            serverMessage.AppendString(user.VirtualId.ToString());
            _room.SendMessage(serverMessage);

            OnRemove(junk);
        }

        /// <summary>
        /// Gets the pet.
        /// </summary>
        /// <param name="petId">The pet identifier.</param>
        /// <returns>RoomUser.</returns>
        internal RoomUser GetPet(uint petId)
        {
            if (_pets.Contains(petId))
                return (RoomUser)_pets[petId];
            return null;
        }

        /// <summary>
        /// Gets the bot.
        /// </summary>
        /// <param name="botId">The bot identifier.</param>
        /// <returns>RoomUser.</returns>
        internal RoomUser GetBot(uint botId)
        {
            if (_bots.Contains(botId))
                return (RoomUser)_bots[botId];
            return null;
        }

        internal RoomUser GetBotByName(string name)
        {
            var roomUser = UserList.Values.FirstOrDefault(b => b.BotData != null && b.BotData.Name == name);
            return roomUser;
        }

        /// <summary>
        /// Updates the user count.
        /// </summary>
        /// <param name="count">The count.</param>
        internal void UpdateUserCount(uint count)
        {
            _userCount = count;
            _room.RoomData.UsersNow = count;
            using (var queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
                queryReactor.RunFastQuery("UPDATE rooms_data SET users_now = " + count + " WHERE id = " + _room.RoomId + " LIMIT 1");
            Azure.GetGame().GetRoomManager().QueueActiveRoomUpdate(_room.RoomData);
        }

        /// <summary>
        /// Gets the room user by virtual identifier.
        /// </summary>
        /// <param name="virtualId">The virtual identifier.</param>
        /// <returns>RoomUser.</returns>
        internal RoomUser GetRoomUserByVirtualId(int virtualId)
        {
            return UserList.ContainsKey(virtualId) ? UserList[virtualId] : null;
        }

        /// <summary>
        /// Gets the users in camping tent.
        /// </summary>
        /// <returns>List&lt;RoomUser&gt;.</returns>
        internal List<RoomUser> GetUsersInCampingTent()
        {
            return GetRoomUsers().Where(x => x.OnCampingTent).ToList();
        }

        /// <summary>
        /// Gets the room users.
        /// </summary>
        /// <returns>HashSet&lt;RoomUser&gt;.</returns>
        internal HashSet<RoomUser> GetRoomUsers()
        {
            return new HashSet<RoomUser>(UserList.Values.Where(x => x.IsBot == false));
        }

        /// <summary>
        /// Gets the room user by rank.
        /// </summary>
        /// <param name="minRank">The minimum rank.</param>
        /// <returns>List&lt;RoomUser&gt;.</returns>
        internal List<RoomUser> GetRoomUserByRank(int minRank)
        {
            return
                UserList.Values.Where(
                    current =>
                        !current.IsBot && current.GetClient() != null && current.GetClient().GetHabbo() != null &&
                        current.GetClient().GetHabbo().Rank > (ulong)minRank).ToList();
        }

        /// <summary>
        /// Gets the room user by habbo.
        /// </summary>
        /// <param name="pName">Name of the p.</param>
        /// <returns>RoomUser.</returns>
        internal RoomUser GetRoomUserByHabbo(string pName)
        {
            if (UsersByUserName.Contains(pName.ToLower()))
                return (RoomUser)UsersByUserName[pName.ToLower()];
            return null;
        }

        /// <summary>
        /// Saves the pets.
        /// </summary>
        /// <param name="dbClient">The database client.</param>
        internal void SavePets(IQueryAdapter dbClient)
        {
            try
            {
                if (GetPets().Any())
                    AppendPetsUpdateString(dbClient);
            }
            catch (Exception ex)
            {
                Logging.LogCriticalException(string.Concat(new object[]
                {
                    "Error during saving furniture for room ",
                    _room.RoomId,
                    ". Stack: ",
                    ex.ToString()
                }));
            }
        }

        /// <summary>
        /// Appends the pets update string.
        /// </summary>
        /// <param name="dbClient">The database client.</param>
        internal void AppendPetsUpdateString(IQueryAdapter dbClient)
        {
            var queryChunk = new QueryChunk("INSERT INTO bots (id,user_id,room_id,name,x,y,z) VALUES ");
            var queryChunk2 =
                new QueryChunk(
                    "INSERT INTO pets_data (type,race,color,experience,energy,createstamp,nutrition,respect) VALUES ");
            var queryChunk3 = new QueryChunk();
            var list = new List<uint>();
            foreach (var current in GetPets().Where(current => !list.Contains(current.PetId)))
            {
                list.Add(current.PetId);
                switch (current.DbState)
                {
                    case DatabaseUpdateState.NeedsInsert:
                        queryChunk.AddParameter(string.Format("{0}name", current.PetId), current.Name);
                        queryChunk2.AddParameter(string.Format("{0}race", current.PetId), current.Race);
                        queryChunk2.AddParameter(string.Format("{0}color", current.PetId), current.Color);
                        queryChunk.AddQuery(string.Concat(new object[]
                        {
                            "(",
                            current.PetId,
                            ",",
                            current.OwnerId,
                            ",",
                            current.RoomId,
                            ",@",
                            current.PetId,
                            "name,", current.X, ",", current.Y, ",", current.Z, ")"
                        }));
                        queryChunk2.AddQuery(string.Concat(new object[]
                        {
                            "(",
                            current.Type,
                            ",@",
                            current.PetId,
                            "race,@",
                            current.PetId,
                            "color,0,100,'",
                            current.CreationStamp,
                            "',0,0)"
                        }));
                        break;

                    case DatabaseUpdateState.NeedsUpdate:
                        queryChunk3.AddParameter(string.Format("{0}name", current.PetId), current.Name);
                        queryChunk3.AddParameter(string.Format("{0}race", current.PetId), current.Race);
                        queryChunk3.AddParameter(string.Format("{0}color", current.PetId), current.Color);
                        queryChunk3.AddQuery(string.Concat(new object[]
                        {
                            "UPDATE bots SET room_id = ",
                            current.RoomId,
                            ", name = @",
                            current.PetId,
                            "name, x = ",
                            current.X,
                            ", Y = ",
                            current.Y,
                            ", Z = ",
                            current.Z,
                            " WHERE id = ",
                            current.PetId
                        }));
                        queryChunk3.AddQuery(string.Concat(new object[]
                        {
                            "UPDATE pets_data SET race = @",
                            current.PetId,
                            "race, color = @",
                            current.PetId,
                            "color, type = ",
                            current.Type,
                            ", experience = ",
                            current.Experience,
                            ", energy = ",
                            current.Energy,
                            ", nutrition = ",
                            current.Nutrition,
                            ", respect = ",
                            current.Respect,
                            ", createstamp = '",
                            current.CreationStamp,
                            "' WHERE id = ",
                            current.PetId
                        }));
                        break;
                }
                current.DbState = DatabaseUpdateState.Updated;
            }
            queryChunk.Execute(dbClient);
            queryChunk3.Execute(dbClient);
            queryChunk.Dispose();
            queryChunk3.Dispose();
            queryChunk = null;
            queryChunk3 = null;
        }

        /// <summary>
        /// Gets the pets.
        /// </summary>
        /// <returns>List&lt;Pet&gt;.</returns>
        internal List<Pet> GetPets()
        {
            var list = UserList.ToList();
            return
                (from current in list select current.Value into value where value.IsPet select value.PetData).ToList();
        }

        /// <summary>
        /// Serializes the status updates.
        /// </summary>
        /// <param name="all">if set to <c>true</c> [all].</param>
        /// <returns>ServerMessage.</returns>
        internal ServerMessage SerializeStatusUpdates(bool all)
        {
            var list = new List<RoomUser>();
            foreach (var current in UserList.Values)
            {
                if (!all)
                {
                    if (!current.UpdateNeeded)
                        continue;
                    current.UpdateNeeded = false;
                }
                list.Add(current);
            }
            if (!list.Any())
                return null;

            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("UpdateUserStatusMessageComposer"));
            serverMessage.AppendInteger(list.Count);
            foreach (var current2 in list)
                current2.SerializeStatus(serverMessage);
            return serverMessage;
        }

        /// <summary>
        /// Backups the counters.
        /// </summary>
        /// <param name="primaryCounter">The primary counter.</param>
        /// <param name="secondaryCounter">The secondary counter.</param>
        internal void BackupCounters(ref int primaryCounter, ref int secondaryCounter)
        {
            primaryCounter = _primaryPrivateUserId;
            secondaryCounter = _secondaryPrivateUserId;
        }

        /// <summary>
        /// Updates the user status.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cycleGameItems">if set to <c>true</c> [cyclegameitems].</param>
        internal void UpdateUserStatus(RoomUser user, bool cycleGameItems)
        {
            if (user == null) return;

            var isBot = user.IsBot;
            if (isBot) cycleGameItems = false;

            try
            {
                var coordItemSearch = new CoordItemSearch(_room.GetGameMap().CoordinatedItems);
                var allRoomItemForSquare = coordItemSearch.GetAllRoomItemForSquare(user.X, user.Y);
                var itemsOnSquare = _room.GetGameMap().GetCoordinatedItems(new Point(user.X, user.Y));

                var newZ = _room.GetGameMap().SqAbsoluteHeight(user.X, user.Y, itemsOnSquare) + ((user.RidingHorse && user.IsPet == false) ? 1 : 0);

                if (Math.Abs(newZ - user.Z) > 0)
                {
                    user.Z = newZ;
                    user.UpdateNeeded = true;
                }

                if (!allRoomItemForSquare.Any()) user.LastItem = 0;
                using (var enumerator = allRoomItemForSquare.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var item = enumerator.Current;
                        if (cycleGameItems)
                        {
                            item.UserWalksOnFurni(user);
                            Azure.GetGame()
                                .GetQuestManager()
                                .ProgressUserQuest(user.GetClient(), QuestType.StandOn, item.GetBaseItem().ItemId);
                        }

                        if (item.GetBaseItem().IsSeat)
                        {
                            if (!user.Statusses.ContainsKey("sit"))
                                if (item.GetBaseItem().StackMultipler && !string.IsNullOrWhiteSpace(item.ExtraData))
                                    if (item.ExtraData != "0")
                                    {
                                        var num2 = Convert.ToInt32(item.ExtraData);
                                        user.Statusses.Add("sit",
                                            item.GetBaseItem().ToggleHeight[num2].ToString(CultureInfo.InvariantCulture)
                                                .Replace(',', '.'));
                                    }
                                    else user.Statusses.Add("sit", TextHandling.GetString(item.GetBaseItem().Height));
                                else user.Statusses.Add("sit", TextHandling.GetString(item.GetBaseItem().Height));

                            if (Math.Abs(user.Z - item.Z) > 0 || user.RotBody != item.Rot)
                            {
                                user.Z = item.Z;
                                user.RotHead = item.Rot;
                                user.RotBody = item.Rot;
                                user.UpdateNeeded = true;
                            }
                        }

                        var interactionType = item.GetBaseItem().InteractionType;

                        switch (interactionType)
                        {
                            case Interaction.QuickTeleport:
                            case Interaction.GuildGate:
                            case Interaction.WalkInternalLink:
                                {
                                    item.Interactor.OnUserWalk(user.GetClient(), item, user);
                                    break;
                                }
                            case Interaction.None:
                                break;

                            case Interaction.Bed:
                                {
                                    if (!user.Statusses.ContainsKey("lay"))
                                        user.Statusses.Add("lay", TextHandling.GetString(item.GetBaseItem().Height));
                                    else if (user.Statusses["lay"] != TextHandling.GetString(item.GetBaseItem().Height)) user.Statusses["lay"] = TextHandling.GetString(item.GetBaseItem().Height);

                                    user.Z = item.Z;
                                    user.RotBody = item.Rot;
                                    break;
                                }
                                
                            case Interaction.Guillotine:
                                {
                                    user.Z = item.Z;
                                    user.RotBody = item.Rot;

                                    item.ExtraData = "1";
                                    item.UpdateState();
                                    var avatarEffectsInventoryComponent =
                                        user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent();

                                    avatarEffectsInventoryComponent.ActivateCustomEffect(170);
                                    break;
                                }

                            case Interaction.FootballGate:
                                break;

                            case Interaction.BanzaiGateBlue:
                            case Interaction.BanzaiGateRed:
                            case Interaction.BanzaiGateYellow:
                            case Interaction.BanzaiGateGreen:
                                {
                                    if (!cycleGameItems) break;
                                    var num3 = (int)(item.Team + 32);
                                    var teamManagerForBanzai =
                                        user.GetClient().GetHabbo().CurrentRoom.GetTeamManagerForBanzai();
                                    var avatarEffectsInventoryComponent =
                                        user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent();
                                    if (user.Team == Team.none)
                                    {
                                        if (!teamManagerForBanzai.CanEnterOnTeam(item.Team)) break;
                                        if (user.Team != Team.none) teamManagerForBanzai.OnUserLeave(user);
                                        user.Team = item.Team;
                                        teamManagerForBanzai.AddUser(user);
                                        if (avatarEffectsInventoryComponent.CurrentEffect != num3) avatarEffectsInventoryComponent.ActivateCustomEffect(num3);
                                        break;
                                    }
                                    if (user.Team != Team.none && user.Team != item.Team)
                                    {
                                        teamManagerForBanzai.OnUserLeave(user);
                                        user.Team = Team.none;
                                        avatarEffectsInventoryComponent.ActivateCustomEffect(0);
                                        break;
                                    }
                                    teamManagerForBanzai.OnUserLeave(user);
                                    if (avatarEffectsInventoryComponent.CurrentEffect == num3) avatarEffectsInventoryComponent.ActivateCustomEffect(0);
                                    user.Team = Team.none;
                                    break;
                                }
                            case Interaction.Jump:
                                break;

                            case Interaction.Pinata:
                                {
                                    if (!user.IsWalking || item.ExtraData.Length <= 0) break;
                                    var num5 = int.Parse(item.ExtraData);
                                    if (num5 >= 100 || user.CurrentEffect != 158) break;
                                    var num6 = num5 + 1;
                                    item.ExtraData = num6.ToString();
                                    item.UpdateState();
                                    Azure.GetGame()
                                        .GetAchievementManager()
                                        .ProgressUserAchievement(user.GetClient(), "ACH_PinataWhacker", 1, false);
                                    if (num6 == 100)
                                    {
                                        Azure.GetGame().GetPinataHandler().DeliverRandomPinataItem(user, _room, item);
                                        Azure.GetGame()
                                            .GetAchievementManager()
                                            .ProgressUserAchievement(user.GetClient(), "ACH_PinataBreaker", 1, false);
                                    }
                                    break;
                                }
                            case Interaction.TileStackMagic:
                            case Interaction.Poster:
                                break;

                            case Interaction.Tent:
                            case Interaction.BedTent:
                                if (user.LastItem == item.Id) break;
                                if (!user.IsBot && !user.OnCampingTent)
                                {
                                    var serverMessage22 = new ServerMessage();
                                    serverMessage22.Init(
                                        LibraryParser.OutgoingRequest("UpdateFloorItemExtraDataMessageComposer"));
                                    serverMessage22.AppendString(item.Id.ToString());
                                    serverMessage22.AppendInteger(0);
                                    serverMessage22.AppendString("1");
                                    user.GetClient().SendMessage(serverMessage22);
                                    user.OnCampingTent = true;
                                    user.LastItem = item.Id;
                                }
                                break;

                            case Interaction.RunWaySage:
                                {
                                    var num7 = new Random().Next(1, 4);
                                    item.ExtraData = num7.ToString();
                                    item.UpdateState();
                                    break;
                                }
                            case Interaction.Shower:
                            case Interaction.ChairState:
                            case Interaction.PressurePad:
                                {
                                    item.ExtraData = "1";
                                    item.UpdateState();
                                    break;
                                }
                            case Interaction.BanzaiTele:
                                {
                                    if(user.IsWalking)
                                        _room.GetGameItemHandler().OnTeleportRoomUserEnter(user, item);
                                    break;
                                }
                            case Interaction.FreezeYellowGate:
                            case Interaction.FreezeRedGate:
                            case Interaction.FreezeGreenGate:
                            case Interaction.FreezeBlueGate:
                                {
                                    if (cycleGameItems)
                                    {
                                        var num4 = (int)(item.Team + 39);
                                        var teamManagerForFreeze =
                                            user.GetClient().GetHabbo().CurrentRoom.GetTeamManagerForFreeze();
                                        var avatarEffectsInventoryComponent2 =
                                            user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent();
                                        if (user.Team != item.Team)
                                        {
                                            if (teamManagerForFreeze.CanEnterOnTeam(item.Team))
                                            {
                                                if (user.Team != Team.none) teamManagerForFreeze.OnUserLeave(user);
                                                user.Team = item.Team;
                                                teamManagerForFreeze.AddUser(user);
                                                if (avatarEffectsInventoryComponent2.CurrentEffect != num4) avatarEffectsInventoryComponent2.ActivateCustomEffect(num4);
                                            }
                                        }
                                        else
                                        {
                                            teamManagerForFreeze.OnUserLeave(user);
                                            if (avatarEffectsInventoryComponent2.CurrentEffect == num4) avatarEffectsInventoryComponent2.ActivateCustomEffect(0);
                                            user.Team = Team.none;
                                        }
                                        var serverMessage33 =
                                            new ServerMessage(
                                                LibraryParser.OutgoingRequest("UserIsPlayingFreezeMessageComposer"));
                                        serverMessage33.AppendBool(user.Team != Team.none);
                                        user.GetClient().SendMessage(serverMessage33);
                                    }
                                    break;
                                }
                        }

                        if (item.GetBaseItem().InteractionType == Interaction.BedTent) user.OnCampingTent = true;

                        user.LastItem = item.Id;
                    }
                }

                if (user.IsSitting && user.TeleportEnabled)
                {
                    user.Z -= 0.35;
                    user.UpdateNeeded = true;
                }
                if (!cycleGameItems) return;
                if (_room.GotSoccer()) _room.GetSoccer().OnUserWalk(user);
                if (_room.GotBanzai()) _room.GetBanzai().OnUserWalk(user);
                _room.GetFreeze().OnUserWalk(user);
            }
            catch (Exception e)
            {
                Logging.HandleException(e, "RoomUserManager.cs:UpdateUserStatus");
            }
        }

        /// <summary>
        /// Turns the heads.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="senderId">The sender identifier.</param>
        internal void TurnHeads(int x, int y, uint senderId)
        {
            foreach (
                var current in
                    UserList.Values.Where(
                        current => current.HabboId != senderId && !current.RidingHorse && !current.IsPet))
                current.SetRot(PathFinder.CalculateRotation(current.X, current.Y, x, y), true);
        }

        /// <summary>
        /// Called when [cycle].
        /// </summary>
        /// <param name="idleCount">The idle count.</param>
        internal void OnCycle(ref int idleCount)
        {
            var count = 0u;

            lock (_toRemove)
                _toRemove.Clear();

            try
            {
                if (_room != null && _room.DiscoMode && _room.TonerData != null && _room.TonerData.Enabled == 1)
                {
                    var item = _room.GetRoomItemHandler().GetItem(_room.TonerData.ItemId);
                    if (item != null)
                    {
                        _room.TonerData.Data1 = Azure.GetRandomNumber(0, 255);
                        _room.TonerData.Data2 = Azure.GetRandomNumber(0, 255);
                        _room.TonerData.Data3 = Azure.GetRandomNumber(0, 255);

                        var message = new ServerMessage(LibraryParser.OutgoingRequest("UpdateRoomItemMessageComposer"));
                        item.Serialize(message);
                        _room.SendMessage(message);
                    }
                }
            }
            catch (Exception e)
            {
                Writer.Writer.LogException("Disco mode: " + e);
            }

            foreach (var roomUser in UserList.Values)
            {
                roomUser.IdleTime++;

                if (!IsValid(roomUser))
                {
                    if (roomUser.GetClient() != null) RemoveUserFromRoom(roomUser.GetClient(), false, false);
                    else RemoveRoomUser(roomUser);
                }

                lock (_toRemove)
                {
                    if (roomUser.NeedsAutokick && !_toRemove.Contains(roomUser))
                    {
                        _toRemove.Add(roomUser);
                        continue;
                    }
                }

                if (!roomUser.IsAsleep && roomUser.IdleTime >= 600 && !roomUser.IsBot && !roomUser.IsPet)
                {
                    roomUser.IsAsleep = true;
                    var sleepMsg = new ServerMessage(LibraryParser.OutgoingRequest("RoomUserIdleMessageComposer"));
                    sleepMsg.AppendInteger(roomUser.VirtualId);
                    sleepMsg.AppendBool(true);
                    _room.SendMessage(sleepMsg);
                }

                if(!roomUser.IsOwner() && roomUser.IdleTime >= 300)
                {
                    var ownergame = Azure.GetGame().GetClientManager().GetClientByUserId((uint)_room.RoomData.OwnerId);
                    Azure.GetGame().GetAchievementManager().ProgressUserAchievement(ownergame, "ACH_RoomDecoHosting", 1, false);
                }

                if (roomUser.CarryItemId > 0)
                {
                    roomUser.CarryTimer--;
                    if (roomUser.CarryTimer <= 0) roomUser.CarryItem(0);
                }

                if (_room.GotFreeze()) Freeze.CycleUser(roomUser);

                var invalidStep = false;

                if (roomUser.SetStep)
                {
                    if (_room.GetGameMap().CanWalk(roomUser.SetX, roomUser.SetY, roomUser.AllowOverride) || roomUser.RidingHorse)
                    {
                        _room.GetGameMap().UpdateUserMovement(new Point(roomUser.Coordinate.X, roomUser.Coordinate.Y),
                            new Point(roomUser.SetX, roomUser.SetY), roomUser);
                        var items = _room.GetGameMap().GetCoordinatedItems(new Point(roomUser.X, roomUser.Y));

                        roomUser.X = roomUser.SetX;
                        roomUser.Y = roomUser.SetY;
                        roomUser.Z = roomUser.SetZ;

                        try
                        {
                            foreach (var itemE in items.ToArray())
                            {
                                itemE.UserWalksOffFurni(roomUser);

                                switch (itemE.GetBaseItem().InteractionType)
                                {
                                    case Interaction.Tent:
                                    case Interaction.BedTent:
                                        if (!roomUser.IsBot && roomUser.OnCampingTent)
                                        {
                                            var serverMessage = new ServerMessage();
                                            serverMessage.Init(
                                                LibraryParser.OutgoingRequest("UpdateFloorItemExtraDataMessageComposer"));
                                            serverMessage.AppendString(itemE.Id.ToString());
                                            serverMessage.AppendInteger(0);
                                            serverMessage.AppendString("0");
                                            roomUser.GetClient().SendMessage(serverMessage);
                                            roomUser.OnCampingTent = false;
                                        }
                                        break;

                                    case Interaction.RunWaySage:
                                    case Interaction.ChairState:
                                    case Interaction.Shower:
                                    case Interaction.PressurePad:
                                    case Interaction.Guillotine:
                                        {
                                            itemE.ExtraData = "0";
                                            itemE.UpdateState();
                                        }
                                        break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Writer.Writer.LogException("RoomUserManager OnCycle: " + Environment.NewLine + e);
                        }

                        lock (_toRemove)
                        {
                            if (roomUser.X == _room.GetGameMap().Model.DoorX && roomUser.Y == _room.GetGameMap().Model.DoorY &&
                                !_toRemove.Contains(roomUser) && !roomUser.IsBot)
                            {
                                _toRemove.Add(roomUser);
                                continue;
                            }
                        }

                        UpdateUserStatus(roomUser, true);
                    }
                    else invalidStep = true;

                    roomUser.SetStep = false;
                }

                if (!roomUser.IsPet && !roomUser.IsBot) count++;

                if ((roomUser.GoalX == roomUser.X && roomUser.GoalY == roomUser.Y) || roomUser.Freezed)
                {
                    roomUser.IsWalking = false;
                    roomUser.ClearMovement();
                    roomUser.SetStep = false;
                    UpdateUserStatus(roomUser, false);
                }

                if (roomUser.PathRecalcNeeded)
                {
                    roomUser.Path.Clear();
                    roomUser.Path = PathFinder.FindPath(roomUser, _room.GetGameMap().DiagonalEnabled,
                        _room.GetGameMap(), new Vector2D(roomUser.X, roomUser.Y),
                        new Vector2D(roomUser.GoalX, roomUser.GoalY));

                    if (roomUser.Path.Count > 1)
                    {
                        roomUser.PathStep = 1;
                        roomUser.IsWalking = true;
                        roomUser.PathRecalcNeeded = false;
                    }
                    else
                    {
                        roomUser.PathRecalcNeeded = false;
                        roomUser.Path.Clear();
                    }
                }
                if (!roomUser.IsWalking || roomUser.Freezed)
                {
                    if (roomUser.Statusses.ContainsKey("mv"))
                        roomUser.ClearMovement();
                }
                else
                {
                    if (invalidStep || (roomUser.PathStep >= roomUser.Path.Count) ||
                        (roomUser.GoalX == roomUser.X && roomUser.GoalY == roomUser.Y))
                    {
                        roomUser.IsWalking = false;
                        roomUser.ClearMovement();

                        if (roomUser.IsPet && (roomUser.PetData.Type == 3 || roomUser.PetData.Type == 4) &&
                            roomUser.PetData.WaitingForBreading > 0 &&
                            (roomUser.PetData.BreadingTile.X == roomUser.X &&
                             roomUser.PetData.BreadingTile.Y == roomUser.Y))
                        {
                            roomUser.Freezed = true;
                            _room.GetGameMap().RemoveUserFromMap(roomUser, roomUser.Coordinate);
                            switch (roomUser.PetData.Type)
                            {
                                case 3:
                                    {
                                        if (
                                            _room.GetRoomItemHandler().BreedingTerrier[roomUser.PetData.WaitingForBreading]
                                                .PetsList.Count == 2)
                                        {
                                            var owner =
                                                Azure.GetGame()
                                                    .GetClientManager()
                                                    .GetClientByUserId(roomUser.PetData.OwnerId);
                                            if (owner != null)
                                            {
                                                owner.SendMessage(PetBreeding.GetMessage(
                                                    roomUser.PetData.WaitingForBreading,
                                                    _room.GetRoomItemHandler().BreedingTerrier[
                                                        roomUser.PetData.WaitingForBreading].PetsList[0],
                                                    _room.GetRoomItemHandler().BreedingTerrier[
                                                        roomUser.PetData.WaitingForBreading].PetsList[1]));
                                            }
                                        }
                                    }
                                    break;

                                case 4:
                                    {
                                        if (
                                            _room.GetRoomItemHandler().BreedingBear[roomUser.PetData.WaitingForBreading]
                                                .PetsList.Count == 2)
                                        {
                                            var owner =
                                                Azure.GetGame()
                                                    .GetClientManager()
                                                    .GetClientByUserId(roomUser.PetData.OwnerId);
                                            if (owner != null)
                                            {
                                                owner.SendMessage(PetBreeding.GetMessage(
                                                    roomUser.PetData.WaitingForBreading,
                                                    _room.GetRoomItemHandler().BreedingBear[
                                                        roomUser.PetData.WaitingForBreading].PetsList[0],
                                                    _room.GetRoomItemHandler().BreedingBear[
                                                        roomUser.PetData.WaitingForBreading].PetsList[1]));
                                            }
                                        }
                                    }
                                    break;
                            }
                            UpdateUserStatus(roomUser, false);
                        }
                        else if (roomUser.IsPet && (roomUser.PetData.Type == 3 || roomUser.PetData.Type == 4) &&
                                 roomUser.PetData.WaitingForBreading > 0)
                        {
                            roomUser.Freezed = false;
                            roomUser.PetData.WaitingForBreading = 0;
                            roomUser.PetData.BreadingTile = new Point();
                            UpdateUserStatus(roomUser, false);
                        }

                        roomUser.HandelingBallStatus = 0;

                        if (roomUser.RidingHorse && roomUser.IsPet == false)
                        {
                            var horseRidingPet = GetRoomUserByVirtualId(Convert.ToInt32(roomUser.HorseId));
                            if (horseRidingPet != null)
                            {
                                horseRidingPet.IsWalking = false;
                                var ridingMessage =
                                    new ServerMessage(LibraryParser.OutgoingRequest("UpdateUserStatusMessageComposer"));
                                ridingMessage.AppendInteger(1);
                                horseRidingPet.SerializeStatus(ridingMessage, "");
                                roomUser.GetClient().GetHabbo().CurrentRoom.SendMessage(ridingMessage);
                                horseRidingPet.ClearMovement();
                                UpdateUserStatus(roomUser, false);
                            }
                        }
                        else UpdateUserStatus(roomUser, false);
                    }
                    else
                    {
                        var pathCountTotal = (roomUser.Path.Count - roomUser.PathStep) - 1;
                        var nextStep = roomUser.Path[pathCountTotal];

                        roomUser.PathStep++;

                        var nextX = nextStep.X;
                        var nextY = nextStep.Y;

                        roomUser.RemoveStatus("mv");
                        if (_room.GetGameMap()
                            .IsValidStep3(roomUser, new Vector2D(roomUser.X, roomUser.Y), new Vector2D(nextX, nextY),
                                (roomUser.GoalX == nextX && roomUser.GoalY == nextY), roomUser.AllowOverride,
                                roomUser.GetClient()))
                        {
                            string user = string.Empty, mascote = string.Empty;
                            var nextZ = _room.GetGameMap().SqAbsoluteHeight(nextX, nextY);
                            var newRot = Rotation.Calculate(roomUser.X, roomUser.Y, nextX, nextY, roomUser.IsMoonwalking);

                            roomUser.RotBody = newRot;
                            roomUser.RotHead = newRot;
                            roomUser.SetStep = true;

                            roomUser.SetX = nextX;
                            roomUser.SetY = nextY;
                            roomUser.SetZ = nextZ;

                            if (roomUser.RidingHorse && roomUser.IsPet == false)
                            {
                                user = "mv " + nextX + "," + nextY + "," + TextHandling.GetString(nextZ + 1);
                                roomUser.AddStatus("mv", +nextX + "," + nextY + "," + TextHandling.GetString(nextZ + 1));
                                mascote = "mv " + nextX + "," + nextY + "," + nextZ;
                            }
                            else
                                roomUser.AddStatus("mv", nextX + "," + nextY + "," + TextHandling.GetString(nextZ));

                            var horseRidingPet2 = GetRoomUserByVirtualId(Convert.ToInt32(roomUser.HorseId));

                            if (roomUser.RidingHorse && roomUser.IsPet == false)
                            {
                                if (horseRidingPet2 != null)
                                {
                                    horseRidingPet2.RotBody = newRot;
                                    horseRidingPet2.RotHead = newRot;
                                    horseRidingPet2.SetStep = true;
                                    horseRidingPet2.SetX = nextX;
                                    horseRidingPet2.SetY = nextY;
                                    horseRidingPet2.SetZ = nextZ;

                                    UpdateUserEffect(horseRidingPet2, horseRidingPet2.SetX, horseRidingPet2.SetY);

                                    var ridingMessage =
                                        new ServerMessage(
                                            LibraryParser.OutgoingRequest("UpdateUserStatusMessageComposer"));
                                    ridingMessage.AppendInteger(2);

                                    roomUser.SerializeStatus(ridingMessage, user);
                                    horseRidingPet2.SerializeStatus(ridingMessage, mascote);

                                    _room.SendMessage(ridingMessage);

                                    UpdateUserEffect(roomUser, roomUser.SetX, roomUser.SetY);
                                }
                            }

                            if (_room.GotSoccer()) _room.GetSoccer().OnUserWalk(roomUser);

                            if (roomUser.Statusses.ContainsKey("lay") || roomUser.IsLyingDown)
                            {
                                roomUser.Statusses.Remove("lay");
                                roomUser.IsLyingDown = false;
                                roomUser.UpdateNeeded = true;
                            }

                            if (roomUser.Statusses.ContainsKey("sit") || roomUser.IsSitting)
                            {
                                roomUser.Statusses.Remove("sit");
                                roomUser.IsSitting = false;
                                roomUser.UpdateNeeded = true;
                            }

                            UpdateUserEffect(roomUser, roomUser.SetX, roomUser.SetY);

                            _room.GetGameMap().GameMap[roomUser.X, roomUser.Y] = roomUser.SqState;

                            roomUser.SqState = _room.GetGameMap().GameMap[roomUser.SetX, roomUser.SetY];
                        }
                        else roomUser.PathRecalcNeeded = true;
                    }
                    if (!roomUser.RidingHorse) roomUser.UpdateNeeded = true;
                }

                if (roomUser.IsBot) roomUser.BotAI.OnTimerTick();
                UpdateUserEffect(roomUser, roomUser.X, roomUser.Y);
            }

            if (count == 0) idleCount++;

            lock (_toRemove)
            {
                foreach (var toRemove in _toRemove)
                {
                    var client = Azure.GetGame().GetClientManager().GetClientByUserId(toRemove.HabboId);
                    if (client != null) RemoveUserFromRoom(client, true, false);
                    else RemoveRoomUser(toRemove);
                }
                if (_userCount != count) UpdateUserCount(count);
            }
        }

        /// <summary>
        /// Destroys this instance.
        /// </summary>
        internal void Destroy()
        {
            _room = null;
            UsersByUserName.Clear();
            UsersByUserName = null;
            UsersByUserId.Clear();
            UsersByUserId = null;
            OnUserEnter = null;
            _pets.Clear();
            _bots.Clear();
            _pets = null;
            _bots = null;
            UserList = null;
        }

        /// <summary>
        /// Updates the user effect.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private void UpdateUserEffect(RoomUser user, int x, int y)
        {
            if (user.IsBot)
                return;
            try
            {
                var b = _room.GetGameMap().EffectMap[x, y];
                if (b > 0)
                {
                    if (user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect == 0)
                        user.CurrentItemEffect = ItemEffectType.None;
                    var itemEffectType = ByteToItemEffectEnum.Parse(b);
                    if (itemEffectType == user.CurrentItemEffect)
                        return;
                    switch (itemEffectType)
                    {
                        case ItemEffectType.None:
                            user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(-1);
                            user.CurrentItemEffect = itemEffectType;
                            break;

                        case ItemEffectType.Swim:
                            user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(28);
                            user.CurrentItemEffect = itemEffectType;
                            break;

                        case ItemEffectType.SwimLow:
                            user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(30);
                            user.CurrentItemEffect = itemEffectType;
                            break;

                        case ItemEffectType.SwimHalloween:
                            user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(37);
                            user.CurrentItemEffect = itemEffectType;
                            break;

                        case ItemEffectType.Iceskates:
                            user.GetClient()
                                .GetHabbo()
                                .GetAvatarEffectsInventoryComponent()
                                .ActivateCustomEffect(user.GetClient().GetHabbo().Gender.ToUpper() == "M" ? 38 : 39);
                            user.CurrentItemEffect = ItemEffectType.Iceskates;
                            break;

                        case ItemEffectType.Normalskates:
                            user.GetClient()
                                .GetHabbo()
                                .GetAvatarEffectsInventoryComponent()
                                .ActivateCustomEffect(user.GetClient().GetHabbo().Gender.ToUpper() == "M" ? 55 : 56);
                            user.CurrentItemEffect = itemEffectType;
                            break;

                        case ItemEffectType.SnowBoard:
                            {
                                user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(97);
                                user.CurrentItemEffect = itemEffectType;
                            }
                            break;
                    }
                }
                else
                {
                    if (user.CurrentItemEffect == ItemEffectType.None || b != 0)
                        return;
                    user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(-1);
                    user.CurrentItemEffect = ItemEffectType.None;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Handles the <see cref="E:UserAdd" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <param name="user"></param>
        private void OnUserAdd(RoomUser user)
        {
            try
            {
                if (user == null || user.GetClient() == null || user.GetClient().GetHabbo() == null) return;
                var client = user.GetClient();
                if (client == null || client.GetHabbo() == null || _room == null) return;

                if (!user.IsSpectator)
                {
                    var model = _room.GetGameMap().Model;
                    if (model == null) return;
                    user.SetPos(model.DoorX, model.DoorY, model.DoorZ);
                    user.SetRot(model.DoorOrientation, false);

                    user.AddStatus(_room.CheckRights(client, true) ? "flatctrl 4" : "flatctrl 1", string.Empty);

                    user.CurrentItemEffect = ItemEffectType.None;
                    if (!user.IsBot && client.GetHabbo().IsTeleporting)
                    {
                        client.GetHabbo().IsTeleporting = false;
                        client.GetHabbo().TeleportingRoomId = 0;

                        var item = _room.GetRoomItemHandler().GetItem(client.GetHabbo().TeleporterId);
                        if (item != null)
                        {
                            item.ExtraData = "2";
                            item.UpdateState(false, true);
                            user.SetPos(item.X, item.Y, item.Z);
                            user.SetRot(item.Rot, false);
                            item.InteractingUser2 = client.GetHabbo().Id;
                            item.ExtraData = "0";
                            item.UpdateState(false, true);
                        }
                    }
                    if (!user.IsBot && client.GetHabbo().IsHopping)
                    {
                        client.GetHabbo().IsHopping = false;
                        client.GetHabbo().HopperId = 0;

                        var item2 = _room.GetRoomItemHandler().GetItem(client.GetHabbo().HopperId);
                        if (item2 != null)
                        {
                            item2.ExtraData = "1";
                            item2.UpdateState(false, true);
                            user.SetPos(item2.X, item2.Y, item2.Z);
                            user.SetRot(item2.Rot, false);
                            user.AllowOverride = false;
                            item2.InteractingUser2 = client.GetHabbo().Id;
                            item2.ExtraData = "2";
                            item2.UpdateState(false, true);
                        }
                    }
                    if (!user.IsSpectator)
                    {
                        var serverMessage =
                            new ServerMessage(LibraryParser.OutgoingRequest("SetRoomUserMessageComposer"));
                        serverMessage.AppendInteger(1);
                        user.Serialize(serverMessage, _room.GetGameMap().GotPublicPool);
                        _room.SendMessage(serverMessage);
                    }
                    if (!user.IsBot)
                    {
                        var serverMessage2 = new ServerMessage();
                        serverMessage2.Init(LibraryParser.OutgoingRequest("UpdateUserDataMessageComposer"));
                        serverMessage2.AppendInteger(user.VirtualId);
                        serverMessage2.AppendString(client.GetHabbo().Look);
                        serverMessage2.AppendString(client.GetHabbo().Gender.ToLower());
                        serverMessage2.AppendString(client.GetHabbo().Motto);
                        serverMessage2.AppendInteger(client.GetHabbo().AchievementPoints);
                        _room.SendMessage(serverMessage2);
                    }
                    if (_room.RoomData.Owner != client.GetHabbo().UserName)
                    {
                        Azure.GetGame()
                            .GetQuestManager()
                            .ProgressUserQuest(client, QuestType.SocialVisit, 0u);
                        Azure.GetGame()
                            .GetAchievementManager()
                            .ProgressUserAchievement(client, "ACH_RoomEntry", 1, false);
                    }
                }
                if (client.GetHabbo().GetMessenger() != null) client.GetHabbo().GetMessenger().OnStatusChanged(true);
                client.GetMessageHandler().OnRoomUserAdd();

                //if (client.GetHabbo().HasFuse("fuse_mod")) client.GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(102);
                //if (client.GetHabbo().Rank == Convert.ToUInt32(Azure.GetDbConfig().DbData["ambassador.minrank"])) client.GetHabbo().GetAvatarEffectsInventoryComponent().ActivateCustomEffect(178);

                if (OnUserEnter != null) OnUserEnter(user, null);
                if (_room.GotMusicController() && _room.GotMusicController()) _room.GetRoomMusicController().OnNewUserEnter(user);
                _room.OnUserEnter(user);
            }
            catch (Exception ex)
            {
                Logging.LogCriticalException(ex.ToString());
            }
        }

        /// <summary>
        /// Handles the <see cref="E:Remove" /> event.
        /// </summary>
        /// <param name="user"></param>
        private void OnRemove(RoomUser user)
        {
            try
            {
                if (user == null || user.GetClient() == null) return;
                var client = user.GetClient();
                var list = UserList.Values.Where(current => current.IsBot && !current.IsPet && current.BotAI != null);
                var list2 = new List<RoomUser>();
                foreach (var bot in list)
                {
                    bot.BotAI.OnUserLeaveRoom(client);
                    if (bot.IsPet && bot.PetData.OwnerId == user.UserId &&
                        !_room.CheckRights(client, true, false))
                        list2.Add(bot);
                }
                foreach (
                    var current3 in
                        list2.Where(
                            current3 =>
                                user.GetClient() != null && user.GetClient().GetHabbo() != null &&
                                user.GetClient().GetHabbo().GetInventoryComponent() != null))
                {
                    user.GetClient().GetHabbo().GetInventoryComponent().AddPet(current3.PetData);
                    RemoveBot(current3.VirtualId, false);
                }
                _room.GetGameMap().RemoveUserFromMap(user, new Point(user.X, user.Y));
            }
            catch (Exception ex)
            {
                Logging.LogCriticalException(ex.ToString());
            }
        }

        /// <summary>
        /// Called when [user update status].
        /// </summary>
        public void OnUserUpdateStatus()
        {
            foreach (var current in UserList.Values)
                UpdateUserStatus(current, false);
        }

        /// <summary>
        /// Called when [user update status].
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// </summary>
        public void OnUserUpdateStatus(int x, int y)
        {
            foreach (var current in UserList.Values.Where(current => current.X == x && current.Y == y))
                UpdateUserStatus(current, false);
        }

        /// <summary>
        /// Determines whether the specified user is valid.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if the specified user is valid; otherwise, <c>false</c>.</returns>
        private bool IsValid(RoomUser user)
        {
            return user != null && (user.IsBot ||
                                    (user.GetClient() != null && user.GetClient().GetHabbo() != null &&
                                     user.GetClient().GetHabbo().CurrentRoomId == _room.RoomId));
        }
    }
}