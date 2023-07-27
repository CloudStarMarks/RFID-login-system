using System.IO.Pipes;
using Serilog;

namespace BGService
{
    class PipeHandler
    {
        public static async Task<int> ReceiveDataAsync(string pipe_name)
        {
            using (var pipeServer = new NamedPipeServerStream(pipe_name, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
            {
                // Wait for a client to connect asynchronously.
                await pipeServer.WaitForConnectionAsync();

                // When a client connects, continue with further actions.
                int value = readCodeFromPipe(pipeServer);
                await pipeServer.DisposeAsync();
                return value;
            }
        }

        private static int readCodeFromPipe(NamedPipeServerStream pipeServer)
        {
            while (true)
            {
                int value = -1;
                try
                {
                    value = pipeServer.ReadByte();
                }
                catch (Exception ex)
                {
                    BGService.logger.Error("An error occured during reading value from pipe", ex.ToString());
                    break;
                }
                return value;
            }
            return -1;
        }
    }
}