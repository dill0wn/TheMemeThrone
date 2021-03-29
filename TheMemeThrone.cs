using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MemeThroneBot
{
    class TheMemeThrone
    {
        private DiscordSocketClient client;
        private CommandService commandService;
        private ServiceProvider services;
        private CommandHandler commandHandler;

        static void Main(string[] args) =>
            new TheMemeThrone().MainAsync().GetAwaiter().GetResult();

        TheMemeThrone()
        {
            client = new DiscordSocketClient();

            client.Log += Log;
            client.JoinedGuild += JoinedGuild;
            client.GuildAvailable += JoinedGuild;

            commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug,

                CaseSensitiveCommands = false,
            });
            
            services = new ServiceCollection()
                // .AddDbContext<MemingContext>()
                .AddSingleton(client)
                .AddSingleton(commandService)
                .BuildServiceProvider();

            commandHandler = ActivatorUtilities.CreateInstance<CommandHandler>(services);
        }

        public async Task MainAsync()
        {
            await commandHandler.InstallCommandsAsync();

            await client.LoginAsync(TokenType.Bot, File.ReadAllText("token.txt"));
            await client.StartAsync();

            await DbTest();

            await Task.Delay(-1);
        }

        private async Task DbTest()
        {
            using (var db = new MemingContext())
            {
                await db.AddAsync(new MemeCard { Url = "https://i.imgur.com/2Rd4iMl.mp4" });
                await db.SaveChangesAsync();

                Console.WriteLine($"Created a Meme");

                var blog = await db.MemeCards
                    .AsAsyncEnumerable()
                    .OrderBy(b => b.MemeCardId)
                    .FirstOrDefaultAsync();

                Console.WriteLine($"Found a meme: {blog}");

                blog.Url = "https://www.reddit.com";
                await db.SaveChangesAsync();
                Console.WriteLine($"Modified a meme: {blog}");

                db.Remove(blog);
                await db.SaveChangesAsync();
                Console.WriteLine($"Deleted the meme.");
            }
        }

        private async Task JoinedGuild(SocketGuild guild)
        {
            Console.WriteLine($"Guild Event {guild}");
            await guild.DefaultChannel.SendMessageAsync("Hello World!");
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine($"[MemeThrone.Log] {msg.ToString()}");
            await Task.CompletedTask;
        }
    }
}
