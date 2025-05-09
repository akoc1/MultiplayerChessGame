using Google.Protobuf;
using Server.Enums;
using Server.Helpers;
using Server.Models;
using Server.Utility;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ChessServer
    {
        private List<GodotClient> connectedClients = new List<GodotClient>();
        private static List<Game> Games = new List<Game>();
        private TcpListener _tcpListener;
        private const int PORT = 6542;

        public ChessServer()
        {
            _tcpListener = new TcpListener(IPAddress.Any, PORT);
        }

        public async Task Start()
        {
            _tcpListener.Start();
            ConsoleHelper.WriteLine($"Chess server started listening on port {PORT}", MessageType.Default);

            while (true)
            {
                var client = await _tcpListener.AcceptTcpClientAsync();

                GodotClient godotClient = new GodotClient(client, Guid.NewGuid().ToString());

                ConsoleHelper.WriteLine($"Client {godotClient.Id} connected to the server", MessageType.Success);

                connectedClients.Add(godotClient);

                HandleClientAsync(godotClient);
            }
        }

        private static async void HandleClientAsync(GodotClient client)
        {
            NetworkStream clientStream = client.TcpClient.GetStream();

            try
            {
                while (true)
                {
                    byte[] payload = await ReadMessageAsync(clientStream);
                    if (payload == null) break;

                    var message = GameMessage.Parser.ParseFrom(payload);

                    if (message.CreateGameRequest != null)
                    {
                        await HandleCreateGameAsync(client, clientStream);
                    }
                    else if (message.JoinGameRequest != null)
                    {
                        await HandleJoinGameAsync(client, clientStream, message.JoinGameRequest.GameCode);
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteLine($"Error: {ex.Message}", MessageType.Error);
            }
            finally
            {
                clientStream.Close();
            }
        }


        private static async Task<byte[]> ReadMessageAsync(NetworkStream stream)
        {
            byte[] lengthBytes = new byte[4];
            int bytesRead = await stream.ReadAsync(lengthBytes, 0, 4);
            if (bytesRead == 0) return null;

            int messageLength = BitConverter.ToInt32(lengthBytes, 0);
            byte[] payload = new byte[messageLength];

            int totalBytesRead = 0;
            while (totalBytesRead < messageLength)
            {
                int read = await stream.ReadAsync(payload, totalBytesRead, messageLength - totalBytesRead);
                if (read == 0) break;
                totalBytesRead += read;
            }

            return payload;
        }

        private static async Task WriteMessageAsync(NetworkStream stream, GameMessage message)
        {
            byte[] payload = message.ToByteArray();
            byte[] length = BitConverter.GetBytes(payload.Length);

            await stream.WriteAsync(length, 0, 4);
            await stream.WriteAsync(payload, 0, payload.Length);
        }

        private static async Task HandleCreateGameAsync(GodotClient client, NetworkStream stream)
        {
            ConsoleHelper.WriteLine($"{client.Id} has requested to create a game", MessageType.Info);

            bool isClientAlreadyInGame = Games.Any(game => game.Players.Contains(client));

            GameMessage result = new GameMessage();

            if (isClientAlreadyInGame)
            {
                var existingGameCode = Games.FirstOrDefault(game => game.Players.Contains(client)).GameCode;

                result = new GameMessage
                {
                    CreateGameResponse = new CreateGameResponse
                    {
                        Success = false,
                        Result = CreateGameResponse.Types.ResponseResult.AlreadyInGame,
                        GameCode = existingGameCode
                    }
                };

                ConsoleHelper.WriteLine($"{client.Id} is already in game {existingGameCode}.", MessageType.Warning);
            } else
            {
                string gameCode = GameCodeGenerator.Generate();

                Game newGame = new Game() { GameCode = gameCode };
                newGame.Players.Add(client);

                Games.Add(newGame);

                result = new GameMessage
                {
                    CreateGameResponse = new CreateGameResponse
                    {
                        Success = true,
                        GameCode = gameCode
                    }
                };

                ConsoleHelper.WriteLine($"{client.Id} created a game successfully ({gameCode})!", MessageType.Success);
            }

            await WriteMessageAsync(stream, result);
        }

        private static async Task HandleJoinGameAsync(GodotClient client, NetworkStream stream, string gameCode)
        {
            ConsoleHelper.WriteLine($"{client.Id} has requested to join game: {gameCode}", MessageType.Info);

            var existingGame = Games.FirstOrDefault(game => game.GameCode == gameCode);
            
            var response = new GameMessage()
            {
                JoinGameResponse = new JoinGameResponse
                {

                }
            };

            bool success = false;
            bool isGameFull = false;

            if (existingGame == null)
            {
                ConsoleHelper.WriteLine($"{client.Id} can't connect because {gameCode} doesn't exist!", MessageType.Error);

                response.JoinGameResponse.Result = JoinGameResponse.Types.ResponseResult.DoesNotExist;
            }
            else
            {
                isGameFull = existingGame.IsFull();

                if (isGameFull)
                {
                    ConsoleHelper.WriteLine($"Requested game is full, can't connect {client.Id} to the game.", MessageType.Warning);

                    response.JoinGameResponse.Result = JoinGameResponse.Types.ResponseResult.GameIsFull;
                }
                else
                {
                    var alreadyInGame = existingGame.Players.Contains(client);

                    if (alreadyInGame)
                    {
                        ConsoleHelper.WriteLine($"Player ({client.Id}) is already in game -> {gameCode}", MessageType.Warning);
                    } else
                    {
                        ConsoleHelper.WriteLine($"{client.Id} joined the game -> {gameCode}", MessageType.Success);
                        success = true;

                        existingGame.Players.Add(client);

                        if (existingGame.IsFull())
                        {
                            existingGame.StartGame();

                            AnnounceGameStartAsync(existingGame.Players[0], existingGame.Players[1]);

                            ConsoleHelper.WriteLine($"Game {existingGame.GameCode} is starting...", MessageType.Info);

                            return;

                            // Announce that the game is started. Clients,
                            // will be redirected to the game page
                        }
                    }
                }
            }

            response.JoinGameResponse.Success = success;

            await WriteMessageAsync(stream, response);
        }

        private static async void AnnounceGameStartAsync(GodotClient playerOne, GodotClient playerTwo)
        {
            // Randomization of white or black piece
            //Random rand = new Random();

            //int result = rand.Next(1, 3);

            //if (result == 2)
            //{
                
            //}

            var response = new GameMessage()
            {
                StartGame = new StartGame
                {
                    PlayerBlack = string.Empty,
                    PlayerWhite = string.Empty
                }
            };

            await WriteMessageAsync(playerOne.TcpClient.GetStream(), response);
            await WriteMessageAsync(playerTwo.TcpClient.GetStream(), response);
        }
    }
}
