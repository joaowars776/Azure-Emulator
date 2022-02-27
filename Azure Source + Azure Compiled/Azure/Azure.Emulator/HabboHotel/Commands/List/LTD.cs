using Azure.Configuration;
using Azure.HabboHotel.GameClients;
using Azure.Messages;
using Azure.Messages.Parsers;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class LTD. This class cannot be inherited.
    /// </summary>
    internal sealed class LTD : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LTD"/> class.
        /// </summary>
        public LTD()
        {
            MinRank = 10;
            Description = "Atualiza catálogo e avisa sobre Novo Raro LTD.";
            Usage = ":ltd";
            MinParams = 0;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            using (var adapter = Azure.GetDatabaseManager().GetQueryReactor())
            {
                FurniDataParser.SetCache();
                Azure.GetGame().GetItemManager().LoadItems(adapter);
                Azure.GetGame().GetCatalog().Initialize(adapter);
                Azure.GetGame().Reloaditems();
                FurniDataParser.Clear();
            }
            Azure.GetGame()
                .GetClientManager()
                .QueueBroadcaseMessage(
                    new ServerMessage(LibraryParser.OutgoingRequest("PublishShopMessageComposer")));
            var message = new ServerMessage(LibraryParser.OutgoingRequest("SuperNotificationMessageComposer"));
            message.AppendString("ninja_promo_LTD");
            message.AppendInteger(4);
            message.AppendString("title");
            message.AppendString("Novo Raro");
            message.AppendString("message");
            message.AppendString("<h1><b>Corre corre!</b></h1><p></p> Um novo Raro de Edição Limitado foi lançado agorinha mesmo! Compre o seu antes que todas as unidades sejam vendidas.");
            message.AppendString("linkUrl");
            message.AppendString("event:catalog/open/ultd_furni");
            message.AppendString("linkTitle");
            message.AppendString("Verificar Raro");

            Azure.GetGame().GetClientManager().QueueBroadcaseMessage(message);
            return true;
        }
    }
}