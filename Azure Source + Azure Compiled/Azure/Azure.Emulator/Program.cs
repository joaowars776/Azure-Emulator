using Azure.Configuration;
using Renci.SshNet;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Azure
{
    /// <summary>
    /// Class Program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The sc close
        /// </summary>
        public const int ScClose = 61536;

        /// <summary>
        /// The username
        /// </summary>
        private static string username = "root", password = string.Empty;

        /// <summary>
        /// Main Void of Azure.Emulator
        /// </summary>
        /// <param name="args">The arguments.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;

            Console.Clear();
            StartConsoleWindow();
            CheckHosts();

            Out.WriteLine("Hello Please Provide your AzureNET Account to Continue..", "Azure.Authentication");
#if (DEBUG)
            StartEverything();
#else
            StartAuthentication();
#endif
            while (Azure.IsLive)
            {
                Console.CursorVisible = true;
                if (!Logging.DisabledState)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine();
                    Console.Write("{0}@azure> ", username);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                ConsoleCommandHandling.InvokeCommand(Console.ReadLine());
            }
        }

        /// <summary>
        /// Initialize the Azure Environment
        /// </summary>
        public static void InitEnvironment()
        {
            if (Azure.IsLive)
                return;

            Console.CursorVisible = false;
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
            Azure.Initialize();
        }

        /// <summary>
        /// Deletes the menu.
        /// </summary>
        /// <param name="hMenu">The h menu.</param>
        /// <param name="nPosition">The n position.</param>
        /// <param name="wFlags">The w flags.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        /// <summary>
        /// Start's the Console Window
        /// </summary>

        private static void CheckHosts()
        {
            if (
                File.ReadAllText(Environment.SystemDirectory + "\\drivers\\etc\\hosts")
                    .ToLower()
                    .Contains("azure.sulake.me"))
            {
                Out.WriteLine("Your Host File Have Azure.Sulake.Me... (Press Any key to Exit)", "[Azure.Security]",
                    ConsoleColor.Red);
                Console.ReadKey();
                Environment.Exit(1);
            }

            if (
                !NetworkInterface.GetAllNetworkInterfaces().Any(
                    iface =>
                        iface.OperationalStatus == OperationalStatus.Up &&
                        iface.NetworkInterfaceType == NetworkInterfaceType.Loopback &&
                        iface.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase)))
                return;

            Out.WriteLine("Your Interface is LoopBack.. (Press Any key to Exit)", "Azure.Security",
                ConsoleColor.Red);
            Console.ReadKey();
            Environment.Exit(1);
        }

        /// <summary>
        /// Starts the authentication.
        /// </summary>
        private static void StartAuthentication()
        {
            Out.Write("Please Type your Username (Case Sensitive)  ", "Azure.Authentication");
            username = Convert.ToString(Azure.ReadLineMasked());
            Out.Write("Okay, Thanks! Please Insert now your Password (Case Sensitive)  ", "Azure.Authentication");
            password = Convert.ToString(Azure.ReadLineMasked());
            Out.WriteLine("Okay, Thanks! We Going to Make the Connection Now!", "Azure.Authentication");

            try
            {
                using (var client = new SshClient("azure.sulake.me", 21, username, password))
                {
                    client.Connect();
                    bool c = false;
                    c = client.IsConnected;
                    client.Disconnect();
                    client.Dispose();

                    if (c)
                    {
                        Out.WriteLine("Nice! Your Authentication Was Succesfully! Starting Now the Emulator..",
                            "Azure.Authentication");
                        StartEverything();
                        return;
                    }
                    Out.WriteLine(
                        "Sorry Authentication Was Unsuccesfully! Please Try Again! If U want to EXIT you can Close the Environment (Press Any Key to Continue).",
                        "Azure.Authentication", ConsoleColor.DarkRed);
                    Console.ReadKey();

                    StartAuthentication();
                }
            }
            catch
            {
                Out.WriteLine("Unexpected Error! Maybe you Putted Wrong Credentials! Press Any Key to Close..", "Azure.Authentication", ConsoleColor.Red);
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Starts the everything.
        /// </summary>
        private static void StartEverything()
        {
            Console.Clear();
            StartConsoleWindow();
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), 61536, 0);
            InitEnvironment();
        }

        /// <summary>
        /// Starts the console window.
        /// </summary>
        public static void StartConsoleWindow()
        {
            Console.Clear();
            Console.SetWindowSize(Console.LargestWindowWidth > 149 ? 150 : Console.WindowWidth, Console.LargestWindowHeight > 49 ? 50 : Console.WindowHeight);
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.WriteLine(@"     " + @"                                            |         |              ");
            Console.WriteLine(@"     " + @",---.,---,.   .,---.,---.    ,---.,-.-..   .|    ,---.|--- ,---.,---.");
            Console.WriteLine(@"     " + @",---| .-' |   ||    |---'    |---'| | ||   ||    ,---||    |   ||    ");
            Console.WriteLine(@"     " + @"`---^'---'`---'`    `---'    `---'` ' '`---'`---'`---^`---'`---'`    ");
            Console.WriteLine();
            Console.WriteLine(@"     " + @"  BUILD 190 RELEASE 63B CRYPTO BOTH SIDE");
            Console.WriteLine(@"     " + @"  .NET Framework 4.6     C# 6 Roslyn");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine(
                Console.LargestWindowWidth > 149
                ? @"---------------------------------------------------------------------------------------------------------------------------------------------------"
                : @"-------------------------------------------------------------------------");
        }

        /// <summary>
        /// Mies the handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Logging.DisablePrimaryWriting(true);
            var ex = (Exception)args.ExceptionObject;
            Logging.LogCriticalException(string.Format("SYSTEM CRITICAL EXCEPTION: {0}", ex));
        }

        /// <summary>
        /// Gets the system menu.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="bRevert">if set to <c>true</c> [b revert].</param>
        /// <returns>IntPtr.</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// Gets the console window.
        /// </summary>
        /// <returns>IntPtr.</returns>
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
    }
}