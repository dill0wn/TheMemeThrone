using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MemeThroneBot
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
            client.JoinedGuild += JoinedGuild;
            client.GuildAvailable += JoinedGuild;

            var token = File.ReadAllText("token.txt");

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await DbTest();
            Console.WriteLine($"Db Test Complete...");

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
            // return Task.CompletedTask;
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            await Task.CompletedTask;
        }
    }
}
