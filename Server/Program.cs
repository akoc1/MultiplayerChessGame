namespace Server
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Server server = new Server();

            await server.Start();

            Console.ReadLine();
        }
    }
}
