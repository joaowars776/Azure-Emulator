using Azure.Messages;
using Azure.Messages.Parsers;
using System.Collections.Generic;
using System.Data;

namespace Azure.HabboHotel.Catalogs
{
    class TargetedOfferManager
    {
        internal TargetedOffer CurrentOffer;
        public TargetedOfferManager()
        {
            LoadOffer();
        }
        public void LoadOffer()
        {
            CurrentOffer = null;
            DataRow row;
            using (var queryReactor = Azure.GetDatabaseManager().GetQueryReactor())
            {
                queryReactor.SetQuery("SELECT * FROM catalog_targetedoffers WHERE enabled = '1' LIMIT 1");
                row = queryReactor.GetRow();
                if (row == null) return;
                CurrentOffer = new TargetedOffer((int)row["id"], (string)row["identifier"], (int)row["cost_credits"], (int)row["cost_duckets"], (int)row["cost_belcredits"], (int)row["purchase_limit"], (int)row["expiration_time"], (string)row["title"], (string)row["description"], (string)row["image"], (string)row["products"]);
            }
        }
    }
    class TargetedOffer
    {
        internal int Id;
        internal string Identifier;
        internal int CostCredits, CostDuckets, CostBelcredits;
        internal int PurchaseLimit;
        internal int ExpirationTime;
        internal string Title, Description, Image;
        internal string[] Products;
        public TargetedOffer(int id, string identifier, int costCredits, int costDuckets, int costBelcredits, int purchaseLimit, int expirationTime, string title, string description, string image, string products)
        {
            Id = id;
            Identifier = identifier;
            CostCredits = costCredits;
            CostDuckets = costDuckets;
            CostBelcredits = costBelcredits;
            PurchaseLimit = purchaseLimit;
            ExpirationTime = expirationTime;
            Title = title;
            Description = description;
            Image = image;
            Products = products.Split(';');
        }
        internal void GenerateMessage(ServerMessage message)
        {
            message.Init(LibraryParser.OutgoingRequest("TargetedOfferMessageComposer"));
            message.AppendInteger(1);//show
            message.AppendInteger(Id);
            message.AppendString(Identifier);
            message.AppendString(Identifier);
            message.AppendInteger(CostCredits);
            if (CostBelcredits > 0)
            {
                message.AppendInteger(CostBelcredits);
                message.AppendInteger(105);
            }
            else
            {
                message.AppendInteger(CostDuckets);
                message.AppendInteger(0);
            }
            message.AppendInteger(PurchaseLimit);
            var TimeLeft = ExpirationTime - Azure.GetUnixTimeStamp();
            message.AppendInteger(TimeLeft);
            message.AppendString(Title);
            message.AppendString(Description);
            message.AppendString(Image);
            message.StartArray();
            foreach(string Product in Products)
            {
                message.AppendString(Product);
                message.SaveArray();
            }
            message.EndArray();
        }
    }
}