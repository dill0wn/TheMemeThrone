using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MemeThroneBot.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MemeThroneBot
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commandService;
        private ServiceProvider services;
        private CommandHandler commandHandler;

        static void Main(string[] args) =>
            new Program().MainAsync().GetAwaiter().GetResult();

        Program()
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
                .AddDbContext<MemingContext>()
                .AddSingleton(client)
                .AddSingleton(commandService)
                .AddSingleton<CommandListeners>()
                .BuildServiceProvider();

            commandHandler = ActivatorUtilities.CreateInstance<CommandHandler>(services);
        }

        public async Task MainAsync()
        {
            await EnsureDataset();

            await commandHandler.InstallCommandsAsync();

            await client.LoginAsync(TokenType.Bot, File.ReadAllText("token.txt"));
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task EnsureDataset()
        {
            using (var db = new MemingContext())
            {
                await db.Database.MigrateAsync();

                var meme = await db.MemeCards.AsAsyncEnumerable().FirstOrDefaultAsync();
                if (meme == null)
                {
                    Console.WriteLine("Adding default MemeCard");
                    await db.AddAsync(new MemeCard { Url = "https://i.kym-cdn.com/entries/icons/original/000/022/134/elmo.jpg" });
                    await db.SaveChangesAsync();
                }
                
                var caption = await db.CaptionCards.AsAsyncEnumerable().FirstOrDefaultAsync();
                if (caption == null)
                {
                    Console.WriteLine("Adding default CaptionCard");
                    await db.AddAsync(new CaptionCard { Text = "   Come with me if you want to live." });
                    await db.SaveChangesAsync();
                }
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
