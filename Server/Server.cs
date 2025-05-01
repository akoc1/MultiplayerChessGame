using Server.Enums;
using Server.Helpers;
using Server.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Server
    {
        private List<GodotClient> connectedClients;
        private TcpListener _tcpListener;
        private const int PORT = 6542;

        public Server()
        {
            _tcpListener = new TcpListener(IPAddress.Any, PORT);
            connectedClients = new List<GodotClient>();
        }

        public async Task Start()
        {
            _tcpListener.Start();
            ConsoleHelper.WriteLine($"Chess server started listening on: {PORT}", MessageType.Default);

            while (true)
            {
                var client = await _tcpListener.AcceptTcpClientAsync();

                GodotClient godotClient = new GodotClient(client, Guid.NewGuid().ToString());

                ConsoleHelper.WriteLine($"Client {godotClient.Id} connected to the server", MessageType.Success);

                connectedClients.Add(godotClient);

                await HandleClientAsync(godotClient);
            }
        }

        private async Task HandleClientAsync(GodotClient client)
        {
            NetworkStream clientStream = client.TcpClient.GetStream();

            string? clientMessage;

            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int byteRead = await clientStream.ReadAsync(buffer, 0, buffer.Length);

                    // check if client has disconnected from server
                    if (byteRead == 0) break;

                    clientMessage = Encoding.UTF8.GetString(buffer, 0, byteRead);
                    ConsoleHelper.WriteLine($"Message from {client.Id}: {clientMessage}", MessageType.Info);

                    byte[] response = Encoding.UTF8.GetBytes("message sent");
                    await clientStream.WriteAsync(response, 0, response.Length);
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteLine($"Error: {ex.Message}", MessageType.Error);
            }
            finally
            {
                // Client disconnected from server
                clientStream.Close();
                connectedClients.Remove(client);
            }
        }
    }
}
