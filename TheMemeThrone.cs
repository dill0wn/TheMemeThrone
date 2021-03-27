using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace MemeGameBot
{
    class TheMemeThrone
    {
        private DiscordSocketClient client;
        
        static void Main(string[] args) =>
            new TheMemeThrone().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            client = new DiscordSocketClient();

            client.Log += Log;

            var token = File.ReadAllText(Path.Combine("config", "token.txt"));

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();


            client.JoinedGuild += JoinedGuild;
            client.GuildAvailable += JoinedGuild;

            await Task.Delay(-1);
        }

        private Task JoinedGuild(SocketGuild guild)
        {
            Console.WriteLine($"Guild Event {guild}");
            return guild.DefaultChannel.SendMessageAsync("Hello World!");
            // return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
