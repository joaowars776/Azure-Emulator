using System.Collections.Generic;

namespace Azure.HabboHotel.Catalogs
{
    /// <summary>
    /// Class GiftWrappers.
    /// </summary>
    internal static class GiftWrappers
    {
        /// <summary>
        /// The gift wrappers list
        /// </summary>
        public static List<short> GiftWrappersList = new List<short>();

        /// <summary>
        /// The old gift wrappers
        /// </summary>
        public static List<short> OldGiftWrappers = new List<short>();

        /// <summary>
        /// Adds the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public static void Add(short id)
        {
            GiftWrappersList.Add(id);
        }

        /// <summary>
        /// Adds the old.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public static void AddOld(short id)
        {
            OldGiftWrappers.Add(id);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            GiftWrappersList.Clear();
            OldGiftWrappers.Clear();
        }
    }
}