using Azure.HabboHotel.GameClients;
using System;
using System.Collections.Generic;

namespace Azure.HabboHotel.Guides
{
    /// <summary>
    /// Class GuideManager.
    /// </summary>
    internal class GuideManager
    {
        /// <summary>
        /// The en cours
        /// </summary>
        public Dictionary<uint, GameClient> EnCours = new Dictionary<uint, GameClient>();

        //internal int HelpersCount = 0;
        //internal int GuardiansCount = 0;
        /// <summary>
        /// The guides on duty
        /// </summary>
        internal List<GameClient> GuidesOnDuty = new List<GameClient>();
        internal List<GameClient> HelpersOnDuty = new List<GameClient>();
        internal List<GameClient> GuardiansOnDuty = new List<GameClient>();

        /// <summary>
        /// Gets or sets the guides count.
        /// </summary>
        /// <value>The guides count.</value>
        public int GuidesCount
        {
            get
            {
                return this.GuidesOnDuty.Count;
            }
            set
            {
            }
        }

        public int HelpersCount
        {
            get
            {
                return this.HelpersOnDuty.Count;
            }
            set
            {
            }
        }

        public int GuardiansCount
        {
            get
            {
                return this.GuardiansOnDuty.Count;
            }
            set
            {
            }
        }


        /// <summary>
        /// Gets the random guide.
        /// </summary>
        /// <returns>GameClient.</returns>
        public GameClient GetRandomGuide()
        {
            var random = new Random();
            return this.GuidesOnDuty[random.Next(0, this.GuidesCount - 1)];
        }

        /// <summary>
        /// Adds the guide.
        /// </summary>
        /// <param name="guide">The guide.</param>
        public void AddGuide(GameClient guide)
        {
            switch(guide.GetHabbo().DutyLevel)
            {
                case 1:
                    if (!this.GuidesOnDuty.Contains(guide))
                        this.GuidesOnDuty.Add(guide);
                    break;
                case 2:
                    if (!this.HelpersOnDuty.Contains(guide))
                        this.HelpersOnDuty.Add(guide);
                    break;
                case 3:
                    if (!this.GuardiansOnDuty.Contains(guide))
                        this.GuardiansOnDuty.Add(guide);
                    break;
                default:
                    if (!this.GuidesOnDuty.Contains(guide))
                        this.GuidesOnDuty.Add(guide);
                    break;
            }         
        }

        /// <summary>
        /// Removes the guide.
        /// </summary>
        /// <param name="guide">The guide.</param>
        public void RemoveGuide(GameClient guide)
        {
            switch (guide.GetHabbo().DutyLevel)
            {
                case 1:
                    if (this.GuidesOnDuty.Contains(guide))
                        this.GuidesOnDuty.Remove(guide);
                    break;
                case 2:
                    if (this.HelpersOnDuty.Contains(guide))
                        this.HelpersOnDuty.Remove(guide);
                    break;
                case 3:
                    if (this.GuardiansOnDuty.Contains(guide))
                        this.GuardiansOnDuty.Remove(guide);
                    break;
                default:
                    if (this.GuidesOnDuty.Contains(guide))
                        this.GuidesOnDuty.Remove(guide);
                    break;
            }            
        }
    }
}