using System.IO.Pipes;

namespace LoginUI
{
    class LoginUI
    {
        private static ServerHandler sh;
        private static LoginScreen loginScreen;
        public static int usageRecordID = -1;
        private static string PIPE_NAME;
        static LoginUI()
        {
            sh = new ServerHandler("http://127.0.0.1:5000/", "MyComputer");
            loginScreen = new LoginScreen(sh);
            PIPE_NAME = Environment.GetEnvironmentVariable("pipe_name")!;
            PIPE_NAME = "LoginSystem_UI";
        }

        public static async Task usageRecordID_loginAsync()
        {
            if(usageRecordID == -1) return;
            await sendDataAsync(PIPE_NAME, usageRecordID);
            loginScreen.Close();
        }

        public static async Task noReport_LoginAsync()
        {
            await sendDataAsync(PIPE_NAME, -1);
            loginScreen.Close();
        }

        private static async Task sendDataAsync(string pipe_name, int data)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", PIPE_NAME, PipeDirection.Out, PipeOptions.Asynchronous);
            await pipeClient.ConnectAsync();
            pipeClient.WriteByte(Convert.ToByte(data));
            await pipeClient.DisposeAsync();
        }

        // XXX: Testing
        [STAThread]
        // static async Task Main()
        static void Main()
        {
            Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(loginScreen);

            // Console.WriteLine("Sending");
            // await sendData(PIPE_NAME, 33);
            // Console.WriteLine("Sent");
        }

    }
}
