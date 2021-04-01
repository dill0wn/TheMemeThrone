using System;
using System.Linq;
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

    [Group("get")]
    public class GetModule : ModuleBase<SocketCommandContext>
    {
        public MemingContext context { get; set; }

        [Command("caption")]
        [Summary("Echoes a message.")]
        public async Task GetCaptionAsync()
        {
            var caption = await context.CaptionCards.AsAsyncEnumerable().FirstOrDefaultAsync();

            Console.WriteLine($"get caption {caption}");
            if (caption == null)
            {
                await ReplyAsync("No captions availble");
            }
            else
            {
                await ReplyAsync(caption.Text);
            }
        }
    }
}