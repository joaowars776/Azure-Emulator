using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Azure.Security
{
    /// <summary>
    /// Struct BlackWord
    /// </summary>
    internal struct BlackWord
    {
        /// <summary>
        /// The word
        /// </summary>
        public string Word;

        /// <summary>
        /// The type
        /// </summary>
        public BlackWordType Type;

        public BlackWordTypeSettings TypeSettings { get { return BlackWordsManager.GetSettings(Type); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlackWord"/> struct.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="type">The type.</param>
        public BlackWord(string word, BlackWordType type)
        {
            Word = word;
            Type = type;
        }
    }

    /// <summary>
    /// Struct BlackWordTypeSettings
    /// </summary>
    internal struct BlackWordTypeSettings
    {
        /// <summary>
        /// The filter
        /// </summary>
        public string Filter, Alert, ImageAlert;

        /// <summary>
        /// The maximum advices
        /// </summary>
        public uint MaxAdvices;

        /// <summary>
        /// The automatic ban
        /// </summary>
        public bool AutoBan, ShowMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlackWordTypeSettings"/> struct.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="alert">The alert.</param>
        /// <param name="maxAdvices">The maximum advices.</param>
        /// <param name="imageAlert">The image alert.</param>
        /// <param name="autoBan">if set to <c>true</c> [automatic ban].</param>
        public BlackWordTypeSettings(string filter, string alert, uint maxAdvices, string imageAlert, bool autoBan, bool showMessage)
        {
            Filter = filter;
            Alert = alert;
            MaxAdvices = maxAdvices;
            ImageAlert = imageAlert;
            AutoBan = autoBan;
            ShowMessage = showMessage;
        }
    }

    /// <summary>
    /// Enum BlackWordType
    /// </summary>
    internal enum BlackWordType
    {
        /// <summary>
        /// The hotel
        /// </summary>
        Hotel,

        /// <summary>
        /// The insult
        /// </summary>
        Insult,

        /// <summary>
        /// All
        /// </summary>
        All
    }

    /// <summary>
    /// Class BlackWordsManager.
    /// </summary>
    internal static class BlackWordsManager
    {
        /// <summary>
        /// The words
        /// </summary>
        private static readonly List<BlackWord> Words = new List<BlackWord>();

        /// <summary>
        /// The replaces
        /// </summary>
        private static readonly Dictionary<BlackWordType, BlackWordTypeSettings> Replaces =
            new Dictionary<BlackWordType, BlackWordTypeSettings>();

        private static readonly BlackWord Empty = new BlackWord(string.Empty, BlackWordType.All);

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public static void Load()
        {
            using (var adapter = Azure.GetDatabaseManager().GetQueryReactor())
            {
                adapter.SetQuery("SELECT * FROM server_blackwords");
                var table = adapter.GetTable();
                if (table == null) return;

                foreach (DataRow row in table.Rows)
                {
                    var word = row["word"].ToString();
                    var typeStr = row["type"].ToString();


                    AddPrivateBlackWord(typeStr, word);
                }
            }

            Out.WriteLine("Loaded " + Words.Count + " BlackWords", "Azure.Security.BlackWords");
            Console.WriteLine();
        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        public static void Reload()
        {
            Words.Clear();
            Replaces.Clear();

            Load();
        }

        public static void AddBlackWord(string typeStr, string word)
        {
            BlackWordType type;
            if (!BlackWordType.TryParse(typeStr, true, out type)) return;

            if (Words.Any(wordStruct => wordStruct.Type == type && word.Contains(wordStruct.Word))) return;

            using (var adapter = Azure.GetDatabaseManager().GetQueryReactor())
            {
                adapter.SetQuery("INSERT INTO server_blackwords VALUES (null, @word, @type)");
                adapter.AddParameter("word", word);
                adapter.AddParameter("type", typeStr);
                adapter.RunQuery();
            }

            AddPrivateBlackWord(typeStr, word);
        }

        public static void DeleteBlackWord(string typeStr, string word)
        {
            BlackWordType type;
            if (!BlackWordType.TryParse(typeStr, true, out type)) return;

            var wordStruct = Words.FirstOrDefault(wordS => wordS.Type == type && word.Contains(wordS.Word));
            if (string.IsNullOrEmpty(wordStruct.Word)) return;

            Words.Remove(wordStruct);
            using (var adapter = Azure.GetDatabaseManager().GetQueryReactor())
            {
                adapter.SetQuery("DELETE FROM server_blackwords WHERE word = @word AND type = @type");
                adapter.AddParameter("word", word);
                adapter.AddParameter("type", typeStr);
                adapter.RunQuery();
            }
        }

        static void AddPrivateBlackWord(string typeStr, string word)
        {
            BlackWordType type;
            switch (typeStr)
            {
                case "hotel":
                    type = BlackWordType.Hotel;
                    break;

                case "insult":
                    type = BlackWordType.Insult;
                    break;

                case "all":
                    Out.WriteLine("Word type [all] it's reserved for system. Word: " + word,
                        "Azure.Security.BlackWords", ConsoleColor.DarkRed);
                    return;
                default:
                    Out.WriteLine("Undefined type [" + typeStr + "] of word: " + word,
                        "Azure.Security.BlackWords", ConsoleColor.DarkRed);
                    return;
            }

            Words.Add(new BlackWord(word, type));

            if (Replaces.ContainsKey(type)) return;

            string filter = Filter.Default,
                   alert = "User [{0}] with Id: {1} has said a blackword. Word: {2}. Type: {3}. Message: {4}",
                   imageAlert = "bobba";
            var maxAdvices = 7u;
            bool autoBan = true, showMessage = true;
            if (File.Exists("Settings\\BlackWords\\" + typeStr + ".ini"))
            {
                foreach (var array in File.ReadAllLines("Settings\\BlackWords\\" + typeStr + ".ini")
                    .Where(line => !line.StartsWith("#") || !line.StartsWith("//") || line.Contains("="))
                    .Select(line => line.Split('=')))
                {
                    if (array[0] == "filterType") filter = array[1];
                    if (array[0] == "maxAdvices") maxAdvices = uint.Parse(array[1]);
                    if (array[0] == "alertImage") imageAlert = array[1];
                    if (array[0] == "autoBan") autoBan = array[1] == "true";
                    if (array[0] == "showMessage") showMessage = array[1] == "true";
                }
            }

            if (File.Exists("Settings\\BlackWords\\" + typeStr + ".alert.txt")) alert = File.ReadAllText("Settings\\BlackWords\\" + typeStr + ".alert.txt");
            Replaces.Add(type, new BlackWordTypeSettings(filter, alert, maxAdvices, imageAlert, autoBan, showMessage));
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>BlackWordTypeSettings.</returns>
        public static BlackWordTypeSettings GetSettings(BlackWordType type)
        {
            return Replaces[type];
        }

        /// <summary>
        /// Checks the specified string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="type">The type.</param>
        /// <param name="word">The word.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Check(string str, BlackWordType type, out BlackWord word)
        {
            word = Empty;
            if (!Replaces.ContainsKey(type)) return false;

            var data = Replaces[type];

            str = Filter.Replace(data.Filter, str);

            var wordFirst = Words.FirstOrDefault(wordStruct => wordStruct.Type == type && str.Contains(wordStruct.Word));

            word = wordFirst;
            return !string.IsNullOrEmpty(wordFirst.Word);
        }
    }
}