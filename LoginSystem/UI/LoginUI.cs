using System.IO.Pipes;
using Serilog;

namespace LoginUI
{
    class LoginUI
    {
        private static ServerHandler sh;
        // private static LoginScreen loginScreen;
        public static int usageRecordID = -1;
        // Set Global to communicate within sessions
        private const string PIPE_NAME = @"\\.\pipe\Global\LoginSystem_UI";
        public static ILogger logger;
        private static string LOG_FOLDER = @"/LoginSystem/log/LoginUI";
        static LoginUI()
        {
            string logFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + LOG_FOLDER;
            logger = new LoggerConfiguration()
                .WriteTo.File($"{logFolder}/{{Date}}.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            sh = new ServerHandler("http://127.0.0.1:5000/", "MyComputer");
            // loginScreen = new LoginScreen(sh);
        }

        public static async Task usageRecordID_ReportAsync()
        {
            await sendDataAsync(PIPE_NAME, usageRecordID);
            // loginScreen.Close();
        }

        private static async Task sendDataAsync(string pipe_name, int data)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", PIPE_NAME, PipeDirection.Out, PipeOptions.Asynchronous);
            await pipeClient.ConnectAsync();
            byte[] dataArr = BitConverter.GetBytes(data);
            await pipeClient.WriteAsync(dataArr, 0, 4);
            await pipeClient.DisposeAsync();
        }

        [STAThread]
        static void Main()
        {
            // Application.EnableVisualStyles();
            // // Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(loginScreen);
            LoginUI.usageRecordID_ReportAsync().Wait();
        }

    }
}
