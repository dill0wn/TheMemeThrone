using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace MemeThroneBot.Commands
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public Task SayAsync() => ReplyAsync("pong");
        
    }

    public class SayModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync(
            [Remainder]
            [Summary("The text to echo")]
            string echo
            )
        {
            Console.WriteLine($"say {echo}");
            return ReplyAsync(echo);
        }
    }
}