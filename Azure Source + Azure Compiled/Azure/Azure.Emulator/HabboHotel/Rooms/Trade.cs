using Azure.Configuration;
using Azure.HabboHotel.Items;
using Azure.Messages;
using Azure.Messages.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Azure.HabboHotel.Rooms
{
    /// <summary>
    /// Class Trade.
    /// </summary>
    internal class Trade
    {
        /// <summary>
        /// The _users
        /// </summary>
        private readonly TradeUser[] _users;

        /// <summary>
        /// The _room identifier
        /// </summary>
        private readonly uint _roomId;

        /// <summary>
        /// The _one identifier
        /// </summary>
        private readonly uint _oneId;

        /// <summary>
        /// The _two identifier
        /// </summary>
        private readonly uint _twoId;

        /// <summary>
        /// The _trade stage
        /// </summary>
        private int _tradeStage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Trade"/> class.
        /// </summary>
        /// <param name="userOneId">The user one identifier.</param>
        /// <param name="userTwoId">The user two identifier.</param>
        /// <param name="roomId">The room identifier.</param>
        internal Trade(uint userOneId, uint userTwoId, uint roomId)
        {
            this._oneId = userOneId;
            this._twoId = userTwoId;
            this._users = new TradeUser[2];
            this._users[0] = new TradeUser(userOneId, roomId);
            this._users[1] = new TradeUser(userTwoId, roomId);
            this._tradeStage = 1;
            this._roomId = roomId;
            TradeUser[] users = this._users;
            foreach (TradeUser tradeUser in users.Where(tradeUser => !tradeUser.GetRoomUser().Statusses.ContainsKey("trd")))
            {
                tradeUser.GetRoomUser().AddStatus("trd", "");
                tradeUser.GetRoomUser().UpdateNeeded = true;
            }
            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("TradeStartMessageComposer"));
            serverMessage.AppendInteger(userOneId);
            serverMessage.AppendInteger(1);
            serverMessage.AppendInteger(userTwoId);
            serverMessage.AppendInteger(1);
            this.SendMessageToUsers(serverMessage);
        }

        /// <summary>
        /// Gets a value indicating whether [all users accepted].
        /// </summary>
        /// <value><c>true</c> if [all users accepted]; otherwise, <c>false</c>.</value>
        internal bool AllUsersAccepted
        {
            get
            {
                {
                    return this._users.All(t => t == null || t.HasAccepted);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified identifier contains user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if the specified identifier contains user; otherwise, <c>false</c>.</returns>
        internal bool ContainsUser(uint id)
        {
            {
                return this._users.Any(t => t != null && t.UserId == id);
            }
        }

        /// <summary>
        /// Gets the trade user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TradeUser.</returns>
        internal TradeUser GetTradeUser(uint id)
        {
            {
                return this._users.FirstOrDefault(t => t != null && t.UserId == id);
            }
        }

        /// <summary>
        /// Offers the item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="item">The item.</param>
        internal void OfferItem(uint userId, UserItem item)
        {
            TradeUser tradeUser = this.GetTradeUser(userId);
            if (tradeUser == null || item == null || !item.BaseItem.AllowTrade || tradeUser.HasAccepted ||
                this._tradeStage != 1)
            {
                return;
            }
            this.ClearAccepted();
            if (!tradeUser.OfferedItems.Contains(item))
            {
                tradeUser.OfferedItems.Add(item);
            }
            this.UpdateTradeWindow();
        }

        /// <summary>
        /// Takes the back item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="item">The item.</param>
        internal void TakeBackItem(uint userId, UserItem item)
        {
            TradeUser tradeUser = this.GetTradeUser(userId);
            if (tradeUser == null || item == null || tradeUser.HasAccepted || this._tradeStage != 1)
            {
                return;
            }
            this.ClearAccepted();
            tradeUser.OfferedItems.Remove(item);
            this.UpdateTradeWindow();
        }

        /// <summary>
        /// Accepts the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        internal void Accept(uint userId)
        {
            TradeUser tradeUser = this.GetTradeUser(userId);
            if (tradeUser == null || this._tradeStage != 1)
            {
                return;
            }
            tradeUser.HasAccepted = true;
            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("TradeAcceptMessageComposer"));
            serverMessage.AppendInteger(userId);
            serverMessage.AppendInteger(1);
            this.SendMessageToUsers(serverMessage);

            {
                if (!this.AllUsersAccepted)
                {
                    return;
                }
                this.SendMessageToUsers(new ServerMessage(LibraryParser.OutgoingRequest("TradeConfirmationMessageComposer")));
                this._tradeStage++;
                this.ClearAccepted();
            }
        }

        /// <summary>
        /// Unaccepts the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        internal void Unaccept(uint userId)
        {
            TradeUser tradeUser = this.GetTradeUser(userId);
            if (tradeUser == null || this._tradeStage != 1 || this.AllUsersAccepted)
            {
                return;
            }
            tradeUser.HasAccepted = false;
            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("TradeAcceptMessageComposer"));
            serverMessage.AppendInteger(userId);
            serverMessage.AppendInteger(0);
            this.SendMessageToUsers(serverMessage);
        }

        /// <summary>
        /// Completes the trade.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        internal void CompleteTrade(uint userId)
        {
            TradeUser tradeUser = this.GetTradeUser(userId);
            if (tradeUser == null || this._tradeStage != 2)
            {
                return;
            }
            tradeUser.HasAccepted = true;
            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("TradeAcceptMessageComposer"));
            serverMessage.AppendInteger(userId);
            serverMessage.AppendInteger(1);
            this.SendMessageToUsers(serverMessage);
            if (!this.AllUsersAccepted)
            {
                return;
            }
            this._tradeStage = 999;
            this.Finnito();
        }

        /// <summary>
        /// Clears the accepted.
        /// </summary>
        internal void ClearAccepted()
        {
            TradeUser[] users = this._users;
            foreach (TradeUser tradeUser in users)
            {
                tradeUser.HasAccepted = false;
            }
        }

        /// <summary>
        /// Updates the trade window.
        /// </summary>
        internal void UpdateTradeWindow()
        {
            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("TradeUpdateMessageComposer"));

            {
                foreach (TradeUser tradeUser in this._users.Where(tradeUser => tradeUser != null))
                {
                    serverMessage.AppendInteger(tradeUser.UserId);
                    serverMessage.AppendInteger(tradeUser.OfferedItems.Count);
                    foreach (UserItem current in tradeUser.OfferedItems)
                    {
                        serverMessage.AppendInteger(current.Id);
                        serverMessage.AppendString(current.BaseItem.Type.ToString().ToLower());
                        serverMessage.AppendInteger(current.Id);
                        serverMessage.AppendInteger(current.BaseItem.SpriteId);
                        serverMessage.AppendInteger(0);
                        serverMessage.AppendBool(true);
                        serverMessage.AppendInteger(0);
                        serverMessage.AppendString("");
                        serverMessage.AppendInteger(0);
                        serverMessage.AppendInteger(0);
                        serverMessage.AppendInteger(0);
                        if (current.BaseItem.Type == 's')
                        {
                            serverMessage.AppendInteger(0);
                        }
                    }
                }
                this.SendMessageToUsers(serverMessage);
            }
        }

        /// <summary>
        /// Delivers the items.
        /// </summary>
        internal void DeliverItems()
        {
            List<UserItem> offeredItems = this.GetTradeUser(this._oneId).OfferedItems;
            List<UserItem> offeredItems2 = this.GetTradeUser(this._twoId).OfferedItems;
            if (
            offeredItems.Any(
                current =>
                          this.GetTradeUser(this._oneId).GetClient().GetHabbo().GetInventoryComponent().GetItem(current.Id) == null))
            {
                this.GetTradeUser(this._oneId).GetClient().SendNotif("El tradeo ha fallado.");
                this.GetTradeUser(this._twoId).GetClient().SendNotif("El tradeo ha fallado.");
                return;
            }
            if (
            offeredItems2.Any(
                current2 =>
                           this.GetTradeUser(this._twoId).GetClient().GetHabbo().GetInventoryComponent().GetItem(current2.Id) == null))
            {
                this.GetTradeUser(this._oneId).GetClient().SendNotif("El tradeo ha fallado.");
                this.GetTradeUser(this._twoId).GetClient().SendNotif("El tradeo ha fallado.");
                return;
            }
            this.GetTradeUser(this._twoId).GetClient().GetHabbo().GetInventoryComponent().RunDbUpdate();
            this.GetTradeUser(this._oneId).GetClient().GetHabbo().GetInventoryComponent().RunDbUpdate();
            foreach (UserItem current3 in offeredItems)
            {
                this.GetTradeUser(this._oneId).GetClient().GetHabbo().GetInventoryComponent().RemoveItem(current3.Id, false);
                this.GetTradeUser(this._twoId)
                    .GetClient()
                    .GetHabbo()
                    .GetInventoryComponent()
                    .AddNewItem(current3.Id, current3.BaseItemId, current3.ExtraData, current3.GroupId, false, false, 0, 0,
                         current3.SongCode);
                this.GetTradeUser(this._oneId).GetClient().GetHabbo().GetInventoryComponent().RunDbUpdate();
                this.GetTradeUser(this._twoId).GetClient().GetHabbo().GetInventoryComponent().RunDbUpdate();
            }
            foreach (UserItem current4 in offeredItems2)
            {
                this.GetTradeUser(this._twoId).GetClient().GetHabbo().GetInventoryComponent().RemoveItem(current4.Id, false);
                this.GetTradeUser(this._oneId)
                    .GetClient()
                    .GetHabbo()
                    .GetInventoryComponent()
                    .AddNewItem(current4.Id, current4.BaseItemId, current4.ExtraData, current4.GroupId, false, false, 0, 0,
                         current4.SongCode);
                this.GetTradeUser(this._twoId).GetClient().GetHabbo().GetInventoryComponent().RunDbUpdate();
                this.GetTradeUser(this._oneId).GetClient().GetHabbo().GetInventoryComponent().RunDbUpdate();
            }
            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("NewInventoryObjectMessageComposer"));
            serverMessage.AppendInteger(1);
            int i = 1;
            if (offeredItems.Any(current5 => current5.BaseItem.Type.ToString().ToLower() != "s"))
            {
                i = 2;
            }
            serverMessage.AppendInteger(i);
            serverMessage.AppendInteger(offeredItems.Count);
            foreach (UserItem current6 in offeredItems)
            {
                serverMessage.AppendInteger(current6.Id);
            }
            this.GetTradeUser(this._twoId).GetClient().SendMessage(serverMessage);
            var serverMessage2 = new ServerMessage(LibraryParser.OutgoingRequest("NewInventoryObjectMessageComposer"));
            serverMessage2.AppendInteger(1);
            i = 1;
            if (offeredItems2.Any(current7 => current7.BaseItem.Type.ToString().ToLower() != "s"))
            {
                i = 2;
            }
            serverMessage2.AppendInteger(i);
            serverMessage2.AppendInteger(offeredItems2.Count);
            foreach (UserItem current8 in offeredItems2)
            {
                serverMessage2.AppendInteger(current8.Id);
            }
            this.GetTradeUser(this._oneId).GetClient().SendMessage(serverMessage2);
            this.GetTradeUser(this._oneId).GetClient().GetHabbo().GetInventoryComponent().UpdateItems(false);
            this.GetTradeUser(this._twoId).GetClient().GetHabbo().GetInventoryComponent().UpdateItems(false);
        }

        /// <summary>
        /// Closes the trade clean.
        /// </summary>
        internal void CloseTradeClean()
        {
            {
                foreach (
                    TradeUser tradeUser in this._users.Where(tradeUser => tradeUser != null && tradeUser.GetRoomUser() != null))
                {
                    tradeUser.GetRoomUser().RemoveStatus("trd");
                    tradeUser.GetRoomUser().UpdateNeeded = true;
                }
                this.SendMessageToUsers(new ServerMessage(LibraryParser.OutgoingRequest("TradeCompletedMessageComposer")));
                this.GetRoom().ActiveTrades.Remove(this);
            }
        }

        /// <summary>
        /// Closes the trade.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        internal void CloseTrade(uint userId)
        {
            {
                foreach (
                    TradeUser tradeUser in this._users.Where(tradeUser => tradeUser != null && tradeUser.GetRoomUser() != null))
                {
                    tradeUser.GetRoomUser().RemoveStatus("trd");
                    tradeUser.GetRoomUser().UpdateNeeded = true;
                }
                var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("TradeCloseMessageComposer"));
                serverMessage.AppendInteger(userId);
                serverMessage.AppendInteger(0);
                this.SendMessageToUsers(serverMessage);
            }
        }

        /// <summary>
        /// Sends the message to users.
        /// </summary>
        /// <param name="message">The message.</param>
        internal void SendMessageToUsers(ServerMessage message)
        {
            if (this._users == null)
            {
                return;
            }

            {
                foreach (TradeUser tradeUser in this._users.Where(tradeUser => tradeUser != null && tradeUser.GetClient() != null))
                {
                    tradeUser.GetClient().SendMessage(message);
                }
            }
        }

        /// <summary>
        /// Finnitoes this instance.
        /// </summary>
        private void Finnito()
        {
            try
            {
                this.DeliverItems();
                this.CloseTradeClean();
            }
            catch (Exception ex)
            {
                Logging.LogThreadException(ex.ToString(), "Trade task");
            }
        }

        /// <summary>
        /// Gets the room.
        /// </summary>
        /// <returns>Room.</returns>
        private Room GetRoom()
        {
            return Azure.GetGame().GetRoomManager().GetRoom(this._roomId);
        }
    }
}