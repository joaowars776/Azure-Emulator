using Azure.HabboHotel.GameClients;
using Azure.Messages.Parsers;
using System;

namespace Azure.Messages.Handlers
{
    /// <summary>
    /// Class GameClientMessageHandler.
    /// </summary>
    partial class GameClientMessageHandler
    {
        /// <summary>
        /// Calls the guide.
        /// </summary>
        internal void CallGuide()
        {
            Request.GetBool(); //false
            var userId = Request.GetIntegerFromString();
            var message = Request.GetString();
            var guideManager = Azure.GetGame().GetGuideManager();
            if (guideManager.GuidesCount <= 0)
            {
                Response.Init(LibraryParser.OutgoingRequest("OnGuideSessionError")); //onGuideSessionError
                Response.AppendInteger(0); //Errorcode
                SendResponse();
                return;
            }
            var guide = guideManager.GetRandomGuide();
            // Message pour la personne qui demande
            var onGuideSessionAttached = new ServerMessage(LibraryParser.OutgoingRequest("OnGuideSessionAttachedMessageComposer"));
            onGuideSessionAttached.AppendBool(false); //false
            onGuideSessionAttached.AppendInteger(userId);
            onGuideSessionAttached.AppendString(message);
            onGuideSessionAttached.AppendInteger(30); //Temps moyen
            Session.SendMessage(onGuideSessionAttached);
            // Message pour le guide
            var onGuideSessionAttached2 = new ServerMessage(LibraryParser.OutgoingRequest("OnGuideSessionAttachedMessageComposer"));
            onGuideSessionAttached2.AppendBool(true); //false
            onGuideSessionAttached2.AppendInteger(userId);
            onGuideSessionAttached2.AppendString(message);
            onGuideSessionAttached2.AppendInteger(15); //Temps moyen
            guide.SendMessage(onGuideSessionAttached2);
            guide.GetHabbo().GuideOtherUser = Session;
            Session.GetHabbo().GuideOtherUser = guide;
        }

        /// <summary>
        /// Answers the guide request.
        /// </summary>
        internal void AnswerGuideRequest()
        {
            var state = Request.GetBool();
            // Accept button : true
            // Reject button : false
            if (!state)
                return;
            var requester = Session.GetHabbo().GuideOtherUser;
            var message = new ServerMessage(LibraryParser.OutgoingRequest("OnGuideSessionStartedMessageComposer"));
            message.AppendInteger(requester.GetHabbo().Id); //userid
            message.AppendString(requester.GetHabbo().UserName); //Username
            message.AppendString(requester.GetHabbo().Look); //look 1
            message.AppendInteger(Session.GetHabbo().Id); //Id du guide ?
            message.AppendString(Session.GetHabbo().UserName);
            message.AppendString(Session.GetHabbo().Look);
            requester.SendMessage(message);
            Session.SendMessage(message);
        }

        ///TODO: IMPORTANT
        /// <summary>
        /// Cancels the call guide.
        /// </summary>
        internal void CancelCallGuide()
        {
            Response.Init(3485); ///BUG: IMPORTANT 
            SendResponse();
        }

        /// <summary>
        /// Opens the guide tool.
        /// </summary>
        internal void OpenGuideTool()
        {
            var guideManager = Azure.GetGame().GetGuideManager();
            var onDuty = Request.GetBool();

            Request.GetBool(); // guide
            Request.GetBool(); // helper
            Request.GetBool(); // guardian

            if (onDuty)
            {
                guideManager.AddGuide(Session);
            }              
            else
            {
                guideManager.RemoveGuide(Session);
            }
                
            Session.GetHabbo().OnDuty = onDuty;
            Response.Init(LibraryParser.OutgoingRequest("HelperToolConfigurationMessageComposer"));
            Response.AppendBool(onDuty); // on duty
            Response.AppendInteger(guideManager.GuidesCount); // guides
            Response.AppendInteger(guideManager.HelpersCount); // helpers
            Response.AppendInteger(guideManager.GuardiansCount); // guardians
            SendResponse();
        }

        /// <summary>
        /// Invites to room.
        /// </summary>
        internal void InviteToRoom()
        {
            var requester = Session.GetHabbo().GuideOtherUser;
            var room = Session.GetHabbo().CurrentRoom;
            var message = new ServerMessage(LibraryParser.OutgoingRequest("OnGuideSessionInvitedToGuideRoomMessageComposer"));
            //onGuideSessionInvitedToGuideRoom
            if (room == null)
            {
                message.AppendInteger(0); //id de l'appart
                message.AppendString("");
            }
            else
            {
                message.AppendInteger(room.RoomId); //id de l'appart
                message.AppendString(room.RoomData.Name);
            }
            requester.SendMessage(message);
            Session.SendMessage(message);
        }

        /// <summary>
        /// Visits the room.
        /// </summary>
        internal void VisitRoom()
        {
            var requester = Session.GetHabbo().GuideOtherUser;
            var message = new ServerMessage(LibraryParser.OutgoingRequest("GuideSessionRequesterRoomMessageComposer"));
            message.AppendInteger(requester.GetHabbo().CurrentRoomId);
            requester.SendMessage(message);
            Session.SendMessage(message);
        }

        /// <summary>
        /// Guides the speak.
        /// </summary>
        internal void GuideSpeak()
        {
            var message = Request.GetString();
            var requester = Session.GetHabbo().GuideOtherUser;
            var messageC = new ServerMessage(LibraryParser.OutgoingRequest("OnGuideSessionMsgMessageComposer"));
            //onGuideSessionMessage
            messageC.AppendString(message);
            messageC.AppendInteger(Session.GetHabbo().Id);
            requester.SendMessage(messageC);
            Session.SendMessage(messageC);
        }

        /// <summary>
        /// Closes the guide request.
        /// </summary>
        internal void CloseGuideRequest()
        {
            var requester = Session.GetHabbo().GuideOtherUser;
            var message = new ServerMessage(LibraryParser.OutgoingRequest("OnGuideSessionDetachedMessageComposer"));
            //onGuideSessionEnded
            message.AppendInteger(2); //0,1,2
            /* 0 : Erreur
             * 1 : c'est la personne qui demande qui a fermé
             * 2 : C'est le guide qui a fermé */
            requester.SendMessage(message);
            Session.SendMessage(message);
            requester.GetHabbo().GuideOtherUser = null;
            Session.GetHabbo().GuideOtherUser = null;
        }

        /// <summary>
        /// Guides the feedback.
        /// </summary>
        internal void GuideFeedback()
        {
            Request.GetBool(); // feedback
            //var guide = session.GetHabbo().GuideOtherUser;
            var message = new ServerMessage(LibraryParser.OutgoingRequest("OnGuideSessionDetachedMessageComposer"));
            //onGuideSessionEnded
            //requester.SendMessage(Message);
            Session.SendMessage(message);
        }

        /// <summary>
        /// Ambassadors the alert.
        /// </summary>
        internal void AmbassadorAlert()
        {
            if (Session.GetHabbo().Rank < Convert.ToUInt32(Azure.GetDbConfig().DbData["ambassador.minrank"])) return;
            uint userId = Request.GetUInteger();
            GameClient user = Azure.GetGame().GetClientManager().GetClientByUserId(userId);
            if (user == null) return;
            user.SendNotif("${notification.ambassador.alert.warning.message}", "${notification.ambassador.alert.warning.title}");
        }
    }
}