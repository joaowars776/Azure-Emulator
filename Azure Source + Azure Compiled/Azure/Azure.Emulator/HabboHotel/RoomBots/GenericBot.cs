using System;
using System.Linq;
using System.Threading;
using Azure.HabboHotel.GameClients;
using Azure.HabboHotel.Rooms;

namespace Azure.HabboHotel.RoomBots
{
    /// <summary>
    /// Class GenericBot.
    /// </summary>
    internal class GenericBot : BotAI
    {
        /// <summary>
        /// The random
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// The _id
        /// </summary>
        private readonly int _id;

        /// <summary>
        /// The _virtual identifier
        /// </summary>
        private readonly int _virtualId;

        /// <summary>
        /// The _is bartender
        /// </summary>
        private readonly bool _isBartender;

        /// <summary>
        /// The _action count
        /// </summary>
        private int _actionCount;

        /// <summary>
        /// The _speech interval
        /// </summary>
        private int _speechInterval;

        /// <summary>
        /// The _chat timer
        /// </summary>
        private Timer _chatTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericBot"/> class.
        /// </summary>
        /// <param name="roomBot">The room bot.</param>
        /// <param name="virtualId">The virtual identifier.</param>
        /// <param name="botId">The bot identifier.</param>
        /// <param name="type">The type.</param>
        /// <param name="isBartender">if set to <c>true</c> [is bartender].</param>
        /// <param name="speechInterval">The
        /// interval.</param>
        internal GenericBot(RoomBot roomBot, int virtualId, int botId, AIType type, bool isBartender, int speechInterval)
        {
            _id = botId;
            _virtualId = virtualId;
            _isBartender = isBartender;
            _speechInterval = speechInterval < 2 ? 2000 : speechInterval * 1000;

            // Get random speach
            if (roomBot != null && roomBot.AutomaticChat && roomBot.RandomSpeech != null && roomBot.RandomSpeech.Any()) _chatTimer = new Timer(ChatTimerTick, null, _speechInterval, _speechInterval);
            _actionCount = Random.Next(10, 30 + virtualId);
        }

        /// <summary>
        /// Modifieds this instance.
        /// </summary>
        internal override void Modified()
        {
            if (GetBotData() == null) return;
            if (!GetBotData().AutomaticChat || GetBotData().RandomSpeech == null || !GetBotData().RandomSpeech.Any())
            {
                StopTimerTick();
                return;
            }
            _speechInterval = GetBotData().SpeechInterval < 2 ? 2000 : GetBotData().SpeechInterval * 1000;

            if (_chatTimer == null)
            {
                _chatTimer = new Timer(ChatTimerTick, null, _speechInterval, _speechInterval);
                return;
            }
            _chatTimer.Change(_speechInterval, _speechInterval);
        }

        /// <summary>
        /// Called when [timer tick].
        /// </summary>
        internal override void OnTimerTick()
        {
            if (GetBotData() == null) return;

            if (_actionCount > 0)
            {
                _actionCount--;
                return;
            }
            _actionCount = Random.Next(5, 45);

            switch (GetBotData().WalkingMode.ToLower())
            {
                case "freeroam":
                {
                    var randomPoint = GetRoom().GetGameMap().GetRandomWalkableSquare();
                    if (randomPoint.X == 0 || randomPoint.Y == 0) return;

                    GetRoomUser().MoveTo(randomPoint.X, randomPoint.Y);
                    break;
                }
                case "specified_range":
                {
                    var list = GetRoom().GetGameMap().WalkableList.ToList();
                    if (!list.Any()) return;

                    var randomNumber = new Random(DateTime.Now.Millisecond + _virtualId ^ 2).Next(0, list.Count - 1);
                    GetRoomUser().MoveTo(list[randomNumber].X, list[randomNumber].Y);
                    break;
                }
            }
        }

        /// <summary>
        /// Called when [self enter room].
        /// </summary>
        internal override void OnSelfEnterRoom()
        {
        }

        /// <summary>
        /// Called when [self leave room].
        /// </summary>
        /// <param name="kicked">if set to <c>true</c> [kicked].</param>
        internal override void OnSelfLeaveRoom(bool kicked)
        {
        }

        /// <summary>
        /// Called when [user enter room].
        /// </summary>
        /// <param name="user">The user.</param>
        internal override void OnUserEnterRoom(RoomUser user)
        {
        }

        /// <summary>
        /// Called when [user leave room].
        /// </summary>
        /// <param name="client">The client.</param>
        internal override void OnUserLeaveRoom(GameClient client)
        {
        }

        /// <summary>
        /// Called when [user say].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="message">The message.</param>
        internal override void OnUserSay(RoomUser user, string message)
        {
            if (Gamemap.TileDistance(GetRoomUser().X, GetRoomUser().Y, user.X, user.Y) > 16) return;

            if (!_isBartender) return;

            try
            {
                message = message.Substring(1);
            }
            catch
            {
                return;
            }
            switch (message.ToLower())
            {
                case "vem":
                case "vem aqui":
                case "segue":
                case "aqui":
                case "corre":
                    GetRoomUser().Chat(null, "Espere, eu só tenho duas pernas", false, 0, 0);
                    GetRoomUser().MoveTo(user.SquareInFront);
                    return;

                case "servir":
                case "serve":
                    if (GetRoom().CheckRights(user.GetClient()))
                    {
                        foreach (var current in GetRoom().GetRoomUserManager().GetRoomUsers()) current.CarryItem(Random.Next(1, 38));
                        GetRoomUser().Chat(null, "Prontinho! Todos do quarto estão servidos", false, 0, 0);
                        return;
                    }
                    return;

                case "água":
                case "suco":
                    GetRoomUser().Chat(null, "Prontinho! Cuidado morrer engasgado", false, 0, 0);
                    user.CarryItem(Random.Next(1, 3));
                    return;

                case "sorvete":
                    GetRoomUser().Chat(null, "Cuidado congelar o cérebro! ª", false, 0, 0);
                    user.CarryItem(4);
                    return;

                case "rosa":
                    GetRoomUser().Chat(null, "Uma rosa para outra rosa.", false, 0, 0);
                    user.CarryItem(Random.Next(1000, 1002));
                    return;

                case "girassol":
                    GetRoomUser().Chat(null, "Ele não gira igual o sol!", false, 0, 0);
                    user.CarryItem(1002);
                    return;

                case "flor":
                case "planta":
                    GetRoomUser().Chat(null, "Uma flor maravilhousa e diwa para você", false, 0, 0);
                    if (Random.Next(1, 3) == 2)
                    {
                        user.CarryItem(Random.Next(1019, 1024));
                        return;
                    }
                    user.CarryItem(Random.Next(1006, 1010));
                    return;

                case "legumes":
                    GetRoomUser().Chat(null, "Comer para ficar fortinho! Servido com sucesso!", false, 0, 0);
                    user.CarryItem(3);
                    return;

                case "rainha":
                    GetRoomUser().Chat(null, "Ouvi rainha? Única rainha aqui é a Majestade Wulles ƒ", false, 0, 0);
                    return;

                case "café":
                case "cafe":
                    GetRoomUser().Chat(null, "Quase igual do Starbuckets", false, 0, 0);
                    user.CarryItem(Random.Next(11, 18));
                    return;

                case "fruta":
                    GetRoomUser().Chat(null, "Prontinho!", false, 0, 0);
                    user.CarryItem(Random.Next(36, 40));
                    return;

                case "laranja":
                    GetRoomUser().Chat(null, "Uma laranja bem laranja!", false, 0, 0);
                    user.CarryItem(38);
                    return;

                case "maça":
                    GetRoomUser().Chat(null, "Imagine você engasga e morre, seria loko tio!", false, 0, 0);
                    user.CarryItem(37);
                    return;

                case "refri":
                case "coca":
                    GetRoomUser().Chat(null, "Coca cola, pepsi cola —", false, 0, 0);
                    user.CarryItem(19);
                    return;

                case "pera":
                    GetRoomUser().Chat(null, "Pronto!", false, 0, 0);
                    user.CarryItem(36);
                    return;

                case "puta":
                case "puto":
                case "biscate":
                case "viada":
                case "vaca":
                case "idiota":
                    GetRoomUser().Chat(null, "Você me maguou! Não me trate assim...", true, 0, 0);
                    return;

                case "linda":
                case "gostosa":
                case "perfeita":
                    GetRoomUser().Chat(null, "Não fui configurada para amar!", false, 0, 0);
                    return;

                case "wual":
                    GetRoomUser().Chat(null, "Não use a merda do xjoao!", true, 0, 0);
                    return;
                case "wulles":
                    GetRoomUser().Chat(null, "15 99136 2909", true, 0, 0);
                    return;
            }
            GetRoomUser().Chat(null, "Estou aqui para lhe satisfazer.. Sem malícias !", false, 0, 0);
        }

        /// <summary>
        /// Called when [user shout].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="message">The message.</param>
        internal override void OnUserShout(RoomUser user, string message)
        {
            if (_isBartender)
            {
                GetRoomUser()
                    .Chat(null, "Não gosto mais de você, você está muito folgado!", false, 0, 0);
            }
        }

        /// <summary>
        /// Stops the timer tick.
        /// </summary>
        private void StopTimerTick()
        {
            if (_chatTimer == null) return;
            _chatTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _chatTimer.Dispose();
            _chatTimer = null;
        }

        /// <summary>
        /// Chats the timer tick.
        /// </summary>
        /// <param name="o">The o.</param>
        private void ChatTimerTick(object o)
        {
            if (GetBotData() == null || GetRoomUser() == null || GetBotData().WasPicked || GetBotData().RandomSpeech == null ||
                !GetBotData().RandomSpeech.Any())
            {
                StopTimerTick();
                return;
            }

            if(GetRoom() != null && GetRoom().MutedBots)
                return;

            var randomSpeech = GetBotData().GetRandomSpeech(GetBotData().MixPhrases);

            try
            {
                switch (randomSpeech)
                {
                    case ":sit":
                    {
                        var user = GetRoomUser();
                        if (user.RotBody % 2 != 0) user.RotBody--;

                        user.Z = GetRoom().GetGameMap().SqAbsoluteHeight(user.X, user.Y);
                        if (!user.Statusses.ContainsKey("sit"))
                        {
                            user.UpdateNeeded = true;
                            user.Statusses.Add("sit", "0.55");
                        }
                        user.IsSitting = true;
                        return;
                    }
                    case ":stand":
                    {
                        var user = GetRoomUser();
                        if (user.IsSitting)
                        {
                            user.Statusses.Remove("sit");
                            user.IsSitting = false;
                            user.UpdateNeeded = true;
                        }
                        else if (user.IsLyingDown)
                        {
                            user.Statusses.Remove("lay");
                            user.IsLyingDown = false;
                            user.UpdateNeeded = true;
                        }
                        return;
                    }
                }

                if (GetRoom() != null)
                {
                    randomSpeech = randomSpeech.Replace("%user_count%",
                        GetRoom().GetRoomUserManager().GetRoomUserCount().ToString());
                    randomSpeech = randomSpeech.Replace("%item_count%",
                        GetRoom().GetRoomItemHandler().TotalItems.ToString());
                    randomSpeech = randomSpeech.Replace("%floor_item_count%",
                        GetRoom().GetRoomItemHandler().FloorItems.Keys.Count.ToString());
                    randomSpeech = randomSpeech.Replace("%wall_item_count%",
                        GetRoom().GetRoomItemHandler().WallItems.Keys.Count.ToString());

                    if (GetRoom().RoomData != null)
                    {
                        randomSpeech = randomSpeech.Replace("%roomname%", GetRoom().RoomData.Name);
                        randomSpeech = randomSpeech.Replace("%owner%", GetRoom().RoomData.Owner);
                    }
                }
                if (GetBotData() != null) randomSpeech = randomSpeech.Replace("%name%", GetBotData().Name);

                GetRoomUser().Chat(null, randomSpeech, false, 0, 0);
            }
            catch (Exception e)
            {
                Writer.Writer.LogException(e.ToString());
            }
        }
    }
}