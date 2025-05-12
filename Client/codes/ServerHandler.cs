using Godot;
using Google.Protobuf;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public partial class ServerHandler : Node
{
    public CancellationTokenSource cts = new CancellationTokenSource();
    TcpClient tcpClient;
    NetworkStream stream;
    const int PORT = 6542;

    public Action<string> FailedToConnect;
    public Action Connected;
    public Action<CreateGameResponse> GameCreateResponseAction;
    public Action<JoinGameResponse> JoinGameResponseAction;
    public Action<StartGame> GameStartedAction;

    public override void _Ready()
    {

    }

    public async Task StartListeningAsync()
    {
        GD.Print("Started listening the server messages.");

        while (!cts.IsCancellationRequested)
        {
            try
            {
                var message = await ReadMessageAsync();

                if (message.CreateGameResponse != null)
                {
                    GameCreateResponseAction?.Invoke(message.CreateGameResponse);
                } else if (message.JoinGameResponse != null)
                {
                    JoinGameResponseAction?.Invoke(message.JoinGameResponse);
                } else if (message.StartGame != null)
                {
                    GameStartedAction?.Invoke(message.StartGame);
                }

                // Diğer mesaj türlerini burada kontrol edin...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Listening error: {ex.Message}");
                break;
            }
        }
    }

    public async Task ConnectAsync()
    {
        try
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync("13.61.144.226", PORT);

            stream = tcpClient.GetStream();

            Connected?.Invoke();

            _ = StartListeningAsync();
        }
        catch (DisconnectException ex)
        {
            GD.Print($"Successfully disconnected {ex.Message}");
        }
        catch (Exception ex)
        {
            GD.Print($"{ex.Message}");
            FailedToConnect?.Invoke(ex.Message);
        }
    }

    private async Task WriteMessageAsync(GameMessage message)
    {
        byte[] payload = message.ToByteArray();
        byte[] length = BitConverter.GetBytes(payload.Length);

        await stream.WriteAsync(length, 0, length.Length);
        await stream.WriteAsync(payload, 0, payload.Length);
    }

    private async Task<GameMessage> ReadMessageAsync()
    {
        byte[] lengthBytes = new byte[4];
        await stream.ReadAsync(lengthBytes, 0, 4);
        int length = BitConverter.ToInt32(lengthBytes, 0);

        byte[] payload = new byte[length];
        await stream.ReadAsync(payload, 0, length);

        return GameMessage.Parser.ParseFrom(payload);
    }

    public async Task SendCreateGameRequest()
    {
        var request = new CreateGameRequest { Id = Guid.NewGuid().ToString() };
        var message = new GameMessage { CreateGameRequest = request };

        await WriteMessageAsync(message);
    }

    public async Task SendJoinGameRequest(string gameCode)
    {
        var request = new JoinGameRequest
        {
            Id = Guid.NewGuid().ToString(),
            GameCode = gameCode
        };

        var message = new GameMessage { JoinGameRequest = request };
        await WriteMessageAsync(message);
    }
}

public class DisconnectException : System.Exception
{
    public DisconnectException() { }
    public DisconnectException(string message) : base(message) { }
    public DisconnectException(string message, System.Exception inner) : base(message, inner) { }
}
