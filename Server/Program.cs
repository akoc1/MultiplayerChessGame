namespace Server
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            ChessServer chessServer = new ChessServer();

            await chessServer.Start();

            Console.ReadLine();
        }
    }
}
