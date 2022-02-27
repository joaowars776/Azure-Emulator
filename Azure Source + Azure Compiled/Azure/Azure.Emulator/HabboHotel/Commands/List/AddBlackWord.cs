using Azure.HabboHotel.GameClients;

namespace Azure.HabboHotel.Commands.List
{
    /// <summary>
    /// Class Enable. This class cannot be inherited.
    /// </summary>
    internal sealed class AddBlackWord : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddBlackWord"/> class.
        /// </summary>
        public AddBlackWord()
        {
            MinRank = 7;
            Description = "Adds a word to filter list.";
            Usage = ":addblackword type(hotel|insult|all) word";
            MinParams = 2;
        }

        public override bool Execute(GameClient session, string[] pms)
        {
            var type = pms[0];
            var word = pms[1];

            if (string.IsNullOrEmpty(word))
            {
                session.SendWhisper("Palabra inválida.");
                return true;
            }
            Security.BlackWordsManager.AddBlackWord(type, word);
            return true;
        }
    }
}