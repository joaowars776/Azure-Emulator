using Azure.Configuration;
using Azure.Encryption;
using Azure.Encryption.Hurlant.Crypto.Prng;
using Azure.HabboHotel.GameClients;
using Azure.HabboHotel.Quests.Composer;
using Azure.HabboHotel.Rooms;
using Azure.Messages.Parsers;
using Ionic.Zlib;
using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Azure.Messages.Handlers
{
    /// <summary>
    /// Class GameClientMessageHandler.
    /// </summary>
    partial class GameClientMessageHandler
    {
        /// <summary>
        /// The current loading room
        /// </summary>
        internal Room CurrentLoadingRoom;

        /// <summary>
        /// The session
        /// </summary>
        protected GameClient Session;

        /// <summary>
        /// The request
        /// </summary>
        protected ClientMessage Request;

        /// <summary>
        /// The response
        /// </summary>
        protected ServerMessage Response;

        /// <summary>
        /// The _photo data
        /// </summary>
        private string _photoData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameClientMessageHandler"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        internal GameClientMessageHandler(GameClient session)
        {
            Session = session;
            Response = new ServerMessage();
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns>GameClient.</returns>
        internal GameClient GetSession()
        {
            return Session;
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <returns>ServerMessage.</returns>
        internal ServerMessage GetResponse()
        {
            return Response;
        }

        /// <summary>
        /// Destroys this instance.
        /// </summary>
        internal void Destroy()
        {
            Session = null;
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        internal void HandleRequest(ClientMessage request)
        {
            Request = request;
            LibraryParser.HandlePacket(this, request);
        }

        /// <summary>
        /// Sends the response.
        /// </summary>
        internal void SendResponse()
        {
            if (Response != null && Response.Id > 0 && Session.GetConnection() != null)
                Session.GetConnection().SendData(Response.GetReversedBytes());
        }

        /// <summary>
        /// Adds the staff pick.
        /// </summary>
        internal void AddStaffPick()
        {
            this.Session.SendNotif(Azure.GetLanguage().GetVar("addstaffpick_error_1"));
        }

        /// <summary>
        /// Gets the client version message event.
        /// </summary>
        internal void GetClientVersionMessageEvent()
        {
            var release = Request.GetString();
            if (release.Contains("201409222303-304766480"))
            {
                Session.GetHabbo().ReleaseName = "304766480";
                Console.WriteLine("[Handled] Release Id: RELEASE63-201409222303-304766480");
            }
            else if (release.Contains("201411201226-580134750"))
            {
                Session.GetHabbo().ReleaseName = "304766480";
                Console.WriteLine("[Handled] Release Id: RELEASE63-201411201226-580134750");
            }
            else
                LibraryParser.ReleaseName = "Undefined Release";
        }

        /// <summary>
        /// Pongs this instance.
        /// </summary>
        internal void Pong()
        {
            Session.TimePingedReceived = DateTime.Now;
        }

        /// <summary>
        /// Disconnects the event.
        /// </summary>
        internal void DisconnectEvent()
        {
            Session.Disconnect("close window");
        }

        /// <summary>
        /// Latencies the test.
        /// </summary>
        internal void LatencyTest()
        {
            if (Session == null)
                return;
            Session.TimePingedReceived = DateTime.Now;
            GetResponse().Init(LibraryParser.OutgoingRequest("LatencyTestResponseMessageComposer"));
            GetResponse().AppendInteger(Request.GetIntegerFromString());
            SendResponse();
            Azure.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_AllTimeHotelPresence", 1);
        }

        /// <summary>
        /// Fuckyous this instance.
        /// </summary>
        internal void Fuckyou()
        {
        }

        /// <summary>
        /// Initializes the crypto.
        /// </summary>
        internal void InitCrypto()
        {
            if (LibraryParser.Config["Crypto.Enabled"] == "false")
            {
                Response.Init(LibraryParser.OutgoingRequest("InitCryptoMessageComposer"));
                Response.AppendString("Azure");
                Response.AppendString("Disabled Crypto");
                SendResponse();
                return;
            }
            Response.Init(LibraryParser.OutgoingRequest("InitCryptoMessageComposer"));
            Response.AppendString(Handler.GetRsaDiffieHellmanPrimeKey());
            Response.AppendString(Handler.GetRsaDiffieHellmanGeneratorKey());
            SendResponse();
        }

        /// <summary>
        /// Secrets the key.
        /// </summary>
        internal void SecretKey()
        {
            var cipherKey = Request.GetString();
            var sharedKey = Handler.CalculateDiffieHellmanSharedKey(cipherKey);

            if (LibraryParser.Config["Crypto.Enabled"] == "false")
            {
                Response.Init(LibraryParser.OutgoingRequest("SecretKeyMessageComposer"));
                Response.AppendString("Crypto disabled");
                Response.AppendBool(false); //Rc4 clientside.
                SendResponse();
                return;
            }
            if (sharedKey != 0)
            {
                Response.Init(LibraryParser.OutgoingRequest("SecretKeyMessageComposer"));
                Response.AppendString(Handler.GetRsaDiffieHellmanPublicKey());
                Response.AppendBool(ExtraSettings.CryptoClientSide);
                SendResponse();

                var data = sharedKey.ToByteArray();

                if (data[data.Length - 1] == 0)
                    Array.Resize(ref data, data.Length - 1);

                Array.Reverse(data, 0, data.Length);

                Session.GetConnection().ARC4ServerSide = new ARC4(data);
                if (ExtraSettings.CryptoClientSide)
                    Session.GetConnection().ARC4ClientSide = new ARC4(data);
            }
            else
                Session.Disconnect("crypto error");
        }

        /// <summary>
        /// Machines the identifier.
        /// </summary>
        internal void MachineId()
        {
            Request.GetString();
            var machineId = Request.GetString();
            Session.MachineId = machineId;
        }

        /// <summary>
        /// Logins the with ticket.
        /// </summary>
        internal void LoginWithTicket()
        {
            if (Session == null || Session.GetHabbo() != null)
                return;
            Session.TryLogin(Request.GetString());
            if (Session != null)
                Session.TimePingedReceived = DateTime.Now;
        }

        /// <summary>
        /// Informations the retrieve.
        /// </summary>
        internal void InfoRetrieve()
        {
            if (Session == null || Session.GetHabbo() == null)
                return;
            var habbo = Session.GetHabbo();
            Response.Init(LibraryParser.OutgoingRequest("UserObjectMessageComposer"));
            Response.AppendInteger(habbo.Id);
            Response.AppendString(habbo.UserName);
            Response.AppendString(habbo.Look);
            Response.AppendString(habbo.Gender.ToUpper());
            Response.AppendString(habbo.Motto);
            Response.AppendString("");
            Response.AppendBool(false);
            Response.AppendInteger(habbo.Respect);
            Response.AppendInteger(habbo.DailyRespectPoints);
            Response.AppendInteger(habbo.DailyPetRespectPoints);
            Response.AppendBool(true);
            Response.AppendString(habbo.LastOnline.ToString(CultureInfo.InvariantCulture));
            Response.AppendBool(habbo.CanChangeName);
            Response.AppendBool(false);
            SendResponse();
            Response.Init(LibraryParser.OutgoingRequest("BuildersClubMembershipMessageComposer"));
            Response.AppendInteger(Session.GetHabbo().BuildersExpire);
            Response.AppendInteger(Session.GetHabbo().BuildersItemsMax);
            Response.AppendInteger(2);
            SendResponse();
            var tradeLocked = Session.GetHabbo().CheckTrading();
            var canUseFloorEditor = (ExtraSettings.EVERYONE_USE_FLOOR || Session.GetHabbo().VIP || Session.GetHabbo().Rank >= 1);
            Response.Init(LibraryParser.OutgoingRequest("SendPerkAllowancesMessageComposer"));
            Response.AppendInteger(11);
            Response.AppendString("BUILDER_AT_WORK");
            Response.AppendString("");
            Response.AppendBool(canUseFloorEditor);
            Response.AppendString("VOTE_IN_COMPETITIONS");
            Response.AppendString("requirement.unfulfilled.helper_level_2");
            Response.AppendBool(false);
            Response.AppendString("USE_GUIDE_TOOL");
            Response.AppendString((Session.GetHabbo().TalentStatus == "helper" &&
                                   Session.GetHabbo().CurrentTalentLevel >= 4) ||
                                  (Session.GetHabbo().Rank >= 4)
                                    ? ""
                                    : "requirement.unfulfilled.helper_level_4");
            Response.AppendBool((Session.GetHabbo().TalentStatus == "helper" &&
                                 Session.GetHabbo().CurrentTalentLevel >= 4) ||
                                (Session.GetHabbo().Rank >= 4));
            Response.AppendString("JUDGE_CHAT_REVIEWS");
            Response.AppendString("requirement.unfulfilled.helper_level_6");
            Response.AppendBool(false);
            Response.AppendString("NAVIGATOR_ROOM_THUMBNAIL_CAMERA");
            Response.AppendString("");
            Response.AppendBool(true);
            Response.AppendString("CALL_ON_HELPERS");
            Response.AppendString("");
            Response.AppendBool(true);
            Response.AppendString("CITIZEN");
            Response.AppendString("");
            Response.AppendBool(Session.GetHabbo().TalentStatus == "helper" ||
                                Session.GetHabbo().CurrentTalentLevel >= 4);
            Response.AppendString("MOUSE_ZOOM");
            Response.AppendString("");
            Response.AppendBool(false);
            Response.AppendString("TRADE");
            Response.AppendString(tradeLocked ? "" : "requirement.unfulfilled.no_trade_lock");
            Response.AppendBool(tradeLocked);
            Response.AppendString("CAMERA");
            Response.AppendString("");
            Response.AppendBool(ExtraSettings.ENABLE_BETA_CAMERA);
            Response.AppendString("NAVIGATOR_PHASE_TWO_2014");
            Response.AppendString("");
            Response.AppendBool(Session.GetHabbo().NewNavigator || ExtraSettings.NAVIGATOR_NEW_ENABLED);
            SendResponse();

            Session.GetHabbo().InitMessenger();

            GetResponse().Init(LibraryParser.OutgoingRequest("CitizenshipStatusMessageComposer"));
            GetResponse().AppendString("citizenship");
            GetResponse().AppendInteger(1);
            GetResponse().AppendInteger(4);
            SendResponse();

            GetResponse().Init(LibraryParser.OutgoingRequest("GameCenterGamesListMessageComposer"));
            GetResponse().AppendInteger(1);
            GetResponse().AppendInteger(18);
            GetResponse().AppendString("elisa_habbo_stories");
            GetResponse().AppendString("000000");
            GetResponse().AppendString("ffffff");
            GetResponse().AppendString("");
            GetResponse().AppendString("");
            SendResponse();
            GetResponse().Init(LibraryParser.OutgoingRequest("AchievementPointsMessageComposer"));
            GetResponse().AppendInteger(Session.GetHabbo().AchievementPoints);
            SendResponse();
            GetResponse().Init(LibraryParser.OutgoingRequest("FigureSetIdsMessageComposer"));
            Session.GetHabbo()._clothingManager.Serialize(GetResponse());
            SendResponse();
            /*Response.Init(LibraryParser.OutgoingRequest("NewbieStatusMessageComposer"));
            Response.AppendInteger(0);// 2 = new - 1 = nothing - 0 = not new
            SendResponse();*/
            Session.SendMessage(Azure.GetGame().GetNavigator().SerializePromotionCategories());
            if (Azure.GetGame().GetTargetedOfferManager().CurrentOffer != null)
            {
                Azure.GetGame().GetTargetedOfferManager().CurrentOffer.GenerateMessage(GetResponse());
                SendResponse();
            }
            if (Session.GetHabbo().CurrentQuestId != 0)
            {
                var quest = Azure.GetGame().GetQuestManager().GetQuest(Session.GetHabbo().CurrentQuestId);
                Session.SendMessage(QuestStartedComposer.Compose(Session, quest));
            }
        }

        /// <summary>
        /// Habboes the camera.
        /// </summary>
        internal void HabboCamera()
        {
            //string one = this.Request.GetString();
            /*var two = */
            _photoData =
                "{\"t\":1392233862000,\"u\":\"52fbcd84e4b09ba9e304112e\",\"m\":\"Happy 2014 Valentine's day <3\",\"s\":53517374,\"w\":\"http://localhost/supersecret/c_images/photos/komok.png\"}";
            //" + Azure.GetRandomNumber(0, int.MaxValue) + " -hhes-" + Azure.GetRandomNumber(0, 99) + RandomString(2) + Azure.GetRandomNumber(0, 9) + RandomString(2) + Azure.GetRandomNumber(0, 99) + RandomString(1) + Azure.GetRandomNumber(0, 9) + RandomString(3) + Azure.GetRandomNumber(0, 999) + RandomString(2) + Azure.GetRandomNumber(0, 9999) + RandomString(1) + Azure.GetRandomNumber(0, 99) + RandomString(3) + Azure.GetRandomNumber(0, 999) + ".png\"}";// http://localhost/supersecret/c_images/photos/komok.png\"}";
        }
        /// <summary>
        /// Called when [click].
        /// </summary>
        internal void OnClick()
        {
            // System by Komok
            // Powered by Azure

            var section = Request.GetString();
            var subSection = Request.GetString();
            var action = Request.GetString();

            switch (section.ToLower())
            {
                case "stories":
                    {
                        switch (subSection.ToLower())
                        {
                            case "camera":
                                {
                                    switch (action.ToLower())
                                    {
                                        case "stories.photo.purchase.attempt":
                                            {
                                                HabboCamera();
                                                //Console.WriteLine("Buying photo... || Photo Data: " + _photoData);
                                                var item = Session.GetHabbo()
                                                    .GetInventoryComponent()
                                                    .AddNewItem(0, Azure.GetGame().GetItemManager().PhotoId, _photoData, 0, true, false, 0, 0);
                                                Session.GetHabbo().GetInventoryComponent().UpdateItems(false);

                                                Session.GetHabbo().Credits -= 3;
                                                Session.GetHabbo().UpdateCreditsBalance();
                                                Session.GetHabbo().GetInventoryComponent().SendNewItems(item.Id);
                                                Session.SendNotif("You received this item! -> " + item.BaseItem.Name +
                                                                  ", you have now -3 credits, total -> " +
                                                                  Session.GetHabbo().Credits);
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Gets the friends count.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Int32.</returns>
        private static int GetFriendsCount(uint userId)
        {
            int result;
            using (var queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
            {
                queryReactor.SetQuery(
                    "SELECT COUNT(*) FROM messenger_friendships WHERE user_one_id = @id OR user_two_id = @id;");
                queryReactor.AddParameter("id", userId);
                result = queryReactor.GetInteger();
            }
            return result;
        }

        /// <summary>
        /// Targeteds the offer buy.
        /// </summary>
        internal void PurchaseTargetedOffer()
        {
            int offerId = Request.GetInteger();
            int quantity = Request.GetInteger();
            var offer = Azure.GetGame().GetTargetedOfferManager().CurrentOffer;
            if (offer == null) return;
            if (Session.GetHabbo().Credits < offer.CostCredits * quantity) return;
            else if (Session.GetHabbo().ActivityPoints < offer.CostDuckets * quantity) return;
            else if (Session.GetHabbo().BelCredits < offer.CostBelcredits * quantity) return;
            foreach (string Product in offer.Products)
            {
                var item = Azure.GetGame().GetItemManager().GetItemByName(Product);
                if (item == null) continue;
                Azure.GetGame().GetCatalog().DeliverItems(Session, item, quantity, string.Empty, 0, 0, string.Empty);
            }
            Session.GetHabbo().Credits -= offer.CostCredits * quantity;
            Session.GetHabbo().ActivityPoints -= offer.CostDuckets * quantity;
            Session.GetHabbo().BelCredits -= offer.CostBelcredits * quantity;
            Session.GetHabbo().UpdateCreditsBalance();
            Session.GetHabbo().UpdateSeasonalCurrencyBalance();
            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
        }

        /// <summary>
        /// Goes the name of to room by.
        /// </summary>
        internal void GoToRoomByName()
        {
            string name = Request.GetString();
            switch (name)
            {
                case "random_friending_room":
                    var roomFwdFriending = new ServerMessage(LibraryParser.OutgoingRequest("RoomForwardMessageComposer"));


                    int result;
                    using (var queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
                    {
                        queryReactor.SetQuery(
                            "SELECT id, users_now, users_max, state FROM rooms_data WHERE users_now >= 0 AND state = 'open' ORDER BY users_now DESC LIMIT 20");
                        var table = queryReactor.GetTable();
                        List<int> Rooms = new List<int>();
                        foreach (DataRow dataRow in table.Rows)
                        {


                            int room_id = Convert.ToInt32((dataRow[0]));
                            int users_now = (int)dataRow[1];
                            int users_max = (int)dataRow[2];
                            if (users_now < users_max)
                            {
                                Rooms.Add(room_id);
                            }
                        }
                        Random rnd = new Random();
                        var result_rnd = Convert.ToInt32(rnd.Next(Rooms.Count));
                        result = Rooms[result_rnd];
                    }
                    roomFwdFriending.AppendInteger(Convert.ToInt32(result));
                    Session.SendMessage(roomFwdFriending);
                    break;
            }
        }

        /// <summary>
        /// Gets the uc panel.
        /// </summary>
        internal void GetUCPanel()
        {
            string name = Request.GetString();
            switch (name)
            {
                case "new":

                    break;
            }
        }

        /// <summary>
        /// Gets the uc panel hotel.
        /// </summary>
        internal void GetUCPanelHotel()
        {
            int id = Request.GetInteger();
        }

        /// <summary>
        /// Saves the room thumbnail.
        /// </summary>
        internal void SaveRoomThumbnail()
        {
            int count = Request.GetInteger();
            byte[] bytes = Request.GetBytes(count);
            var outData = ZlibStream.UncompressBuffer(bytes);
            //Console.WriteLine();
            Console.WriteLine(Encoding.Default.GetString(outData));
        }
    }
}