using System.Collections.Generic;

namespace Azure.HabboHotel.Items
{
    /// <summary>
    /// Class Item.
    /// </summary>
    internal class Item
    {
        /// <summary>
        /// The sprite identifier
        /// </summary>
        internal int SpriteId;

        /// <summary>
        /// The public name
        /// </summary>
        internal string PublicName;

        /// <summary>
        /// The name
        /// </summary>
        internal string Name;

        /// <summary>
        /// The type
        /// </summary>
        internal char Type;

        /// <summary>
        /// The width
        /// </summary>
        internal int Width;

        /// <summary>
        /// The length
        /// </summary>
        internal int Length;

        /// <summary>
        /// The height
        /// </summary>
        internal double Height;

        /// <summary>
        /// The stackable
        /// </summary>
        internal bool Stackable;

        /// <summary>
        /// The walkable
        /// </summary>
        internal bool Walkable;

        /// <summary>
        /// The is seat
        /// </summary>
        internal bool IsSeat;

        /// <summary>
        /// The allow recycle
        /// </summary>
        internal bool AllowRecycle;

        /// <summary>
        /// The allow trade
        /// </summary>
        internal bool AllowTrade;

        /// <summary>
        /// The allow marketplace sell
        /// </summary>
        internal bool AllowMarketplaceSell;

        /// <summary>
        /// The allow gift
        /// </summary>
        internal bool AllowGift;

        /// <summary>
        /// The allow inventory stack
        /// </summary>
        internal bool AllowInventoryStack;

        /// <summary>
        /// The subscriber only
        /// </summary>
        internal bool SubscriberOnly;

        /// <summary>
        /// The stack multipler
        /// </summary>
        internal bool StackMultipler;

        /// <summary>
        /// The toggle height
        /// </summary>
        internal double[] ToggleHeight;

        /// <summary>
        /// The interaction type
        /// </summary>
        internal Interaction InteractionType;

        /// <summary>
        /// The vending ids
        /// </summary>
        internal List<int> VendingIds;

        /// <summary>
        /// The modes
        /// </summary>
        internal uint Modes;

        /// <summary>
        /// The effect identifier
        /// </summary>
        internal int EffectId;

        /// <summary>
        /// The flat identifier
        /// </summary>
        internal int FlatId;

        /// <summary>
        /// The is group item
        /// </summary>
        internal bool IsGroupItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sprite">The sprite.</param>
        /// <param name="publicName">Name of the public.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="width">The width.</param>
        /// <param name="length">The length.</param>
        /// <param name="height">The height.</param>
        /// <param name="stackable">if set to <c>true</c> [stackable].</param>
        /// <param name="walkable">if set to <c>true</c> [walkable].</param>
        /// <param name="isSeat">if set to <c>true</c> [is seat].</param>
        /// <param name="allowRecycle">if set to <c>true</c> [allow recycle].</param>
        /// <param name="allowTrade">if set to <c>true</c> [allow trade].</param>
        /// <param name="allowMarketplaceSell">if set to <c>true</c> [allow marketplace sell].</param>
        /// <param name="allowGift">if set to <c>true</c> [allow gift].</param>
        /// <param name="allowInventoryStack">if set to <c>true</c> [allow inventory stack].</param>
        /// <param name="interactionType">Type of the interaction.</param>
        /// <param name="modes">The modes.</param>
        /// <param name="vendingIds">The vending ids.</param>
        /// <param name="sub">if set to <c>true</c> [sub].</param>
        /// <param name="effect">The effect.</param>
        /// <param name="stackMultiple">if set to <c>true</c> [stack multiple].</param>
        /// <param name="toggle">The toggle.</param>
        /// <param name="flatId">The flat identifier.</param>
        internal Item(uint id, short sprite, string publicName, string name, char type, int width, int length,
                      double height, bool stackable, bool walkable, bool isSeat, bool allowRecycle, bool allowTrade,
                      bool allowMarketplaceSell, bool allowGift, bool allowInventoryStack,
                      Interaction interactionType,
                      uint modes, string vendingIds, bool sub, int effect, bool stackMultiple, double[] toggle,
                      int flatId)
        {
            ItemId = id;
            SpriteId = sprite;
            PublicName = publicName;
            Name = name;
            Type = type;
            Width = width;
            Length = length;
            Height = height;
            Stackable = stackable;
            Walkable = walkable;
            IsSeat = isSeat;
            AllowRecycle = allowRecycle;
            AllowTrade = allowTrade;
            AllowMarketplaceSell = allowMarketplaceSell;
            AllowGift = allowGift;
            AllowInventoryStack = allowInventoryStack;
            InteractionType = interactionType;
            Modes = modes;
            VendingIds = new List<int>();
            SubscriberOnly = sub;
            EffectId = effect;
            StackMultipler = stackMultiple;
            ToggleHeight = toggle;
            FlatId = flatId;
            IsGroupItem = Name.ToLower().ContainsAny("gld_", "guild_", "grp");

            if (vendingIds.Contains(",")) foreach (var s in vendingIds.Split(',')) VendingIds.Add(int.Parse(s));
            else if (!vendingIds.Equals(string.Empty) && int.Parse(vendingIds) > 0) VendingIds.Add(int.Parse(vendingIds));
        }

        /// <summary>
        /// Gets the item identifier.
        /// </summary>
        /// <value>The item identifier.</value>
        internal uint ItemId { get; private set; }

        public static void Save(uint id, bool stackable, bool allowTrade, double[] height, uint modes)
        {
            using (var queryReacter = Azure.GetDatabaseManager().GetQueryReactor())
            {
                queryReacter.SetQuery("UPDATE LOW_PRIORITY catalog_furnis SET stack_height = @height, can_stack = @stack, allow_trade = @trade, interaction_modes_count = @modes WHERE id = " + id);
                queryReacter.AddParameter("height", string.Join(";", height).Replace(',', '.'));
                queryReacter.AddParameter("stack", stackable ? "1" : "0");
                queryReacter.AddParameter("trade", allowTrade ? "1" : "0");
                queryReacter.AddParameter("modes", modes);
                queryReacter.RunQuery();
            }
        }
    }
}