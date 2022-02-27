using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Azure.Writer
{
    public class Writer
    {
        private static bool _mDisabled;

        public static bool DisabledState
        {
            get { return _mDisabled; }
            set { _mDisabled = value; }
        }

        public static void WriteLine(string line, ConsoleColor colour = ConsoleColor.Yellow)
        {
            if (_mDisabled)
                return;
            Console.ForegroundColor = colour;
            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss:fff") + "] " + line);
        }

        public static void LogException(string logText)
        {
            WriteToFile("Logs\\ExceptionsLog.txt", logText + "\r\n\r\n");
            WriteLine("Se ha loggeado una excepción", ConsoleColor.Red);
        }

        public static void LogCriticalException(string logText)
        {
            WriteToFile("Logs\\ExceptionsLog.txt", logText + "\r\n\r\n");
            WriteLine("Error crítico loggeado.", ConsoleColor.Red);
        }

        public static void LogCacheError(string logText)
        {
            WriteToFile("Logs\\ErrorLog.txt", logText + "\r\n\r\n");
            WriteLine("Error loggeado.", ConsoleColor.Red);
        }

        public static void LogMessage(string logText)
        {
            WriteToFile("Logs\\CommonLog.txt", logText + "\r\n\r\n");
            Console.WriteLine(logText);
        }

        public static void LogDDOS(string logText)
        {
            WriteToFile("Logs\\DDosLog.txt", logText + "\r\n\r\n");
            WriteLine(logText, ConsoleColor.Red);
        }

        public static void LogThreadException(string exception, string threadName)
        {
            WriteToFile("Logs\\ErrorLog.txt", string.Concat(new[]
            {
                "Error en thread ",
                threadName,
                ": \r\n",
                exception,
                "\r\n\r\n"
            }));
            WriteLine("Error en Thread " + threadName, ConsoleColor.Red);
        }

        public static void LogQueryError(Exception exception, string query)
        {
            if (query.Contains("FROM xdrcms_minimail WHERE InBin"))
                return;

            WriteToFile("Logs\\MySQLErrors.txt", string.Concat(new object[]
            {
                "Error en query: \r\n",
                query,
                "\r\n",
                exception,
                "\r\n\r\n"
            }));
            WriteLine("Error en query MySQL.", ConsoleColor.Red);
        }

        public static void LogPacketException(string packet, string exception)
        {
            WriteToFile("Logs\\PacketLogError.txt", "Error in packet " + packet + ": \r\n" + exception + "\r\n\r\n");
            WriteLine("A packet error has been logged.", ConsoleColor.Red);
        }

        public static void HandleException(Exception pException, string pLocation)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Concat(new[]
            {
                "Exception logged ",
                DateTime.Now.ToString(),
                " in ",
                pLocation,
                ":"
            }));
            stringBuilder.AppendLine(pException.ToString());
            if (pException.InnerException != null)
            {
                stringBuilder.AppendLine("Inner exception:");
                stringBuilder.AppendLine(pException.InnerException.ToString());
            }
            if (pException.HelpLink != null)
            {
                stringBuilder.AppendLine("Help link:");
                stringBuilder.AppendLine(pException.HelpLink);
            }
            if (pException.Source != null)
            {
                stringBuilder.AppendLine("Source:");
                stringBuilder.AppendLine(pException.Source);
            }
            stringBuilder.AppendLine("Data:");
            foreach (DictionaryEntry dictionaryEntry in pException.Data)
                stringBuilder.AppendLine(string.Concat(new[]
                {
                    "  Key: ",
                    dictionaryEntry.Key,
                    "Value: ",
                    dictionaryEntry.Value
                }));
            stringBuilder.AppendLine("Message:");
            stringBuilder.AppendLine(pException.Message);
            if (pException.StackTrace != null)
            {
                stringBuilder.AppendLine("Stack trace:");
                stringBuilder.AppendLine(pException.StackTrace);
            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            LogException(stringBuilder.ToString());
        }

        public static void DisablePrimaryWriting(bool clearConsole)
        {
            _mDisabled = true;
            /*
            if (clearConsole)
                Console.Clear();
            */
        }

        public static void LogShutdown(StringBuilder builder)
        {
            WriteToFile("Logs\\shutdownlog.txt", builder.ToString());
        }

        private static void WriteToFile(string path, string content)
        {
            try
            {
                File.AppendAllText(path, Environment.NewLine + content, Encoding.ASCII);
            }
            catch
            {
            }
        }
    }
}