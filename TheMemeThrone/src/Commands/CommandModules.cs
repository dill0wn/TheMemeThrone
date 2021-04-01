using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
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

        [Command("meme")]
        [Summary("Posts a Meme.")]
        public async Task GetMemeAsync()
        {
            var meme = await context.MemeCards.AsAsyncEnumerable().FirstOrDefaultAsync();

            Console.WriteLine($"get meme {meme}");
            if (meme == null)
            {
                await ReplyAsync("No memes availble");
                return;
            }

            var embed = new EmbedBuilder()
                .WithFields(
                    new EmbedFieldBuilder { Name = "Option 1️⃣", Value = "Bar1", IsInline = true },
                    new EmbedFieldBuilder { Name = "Option 2️⃣", Value = "Bar2", IsInline = true },
                    new EmbedFieldBuilder { Name = "Option 3️⃣", Value = "Bar3", IsInline = false }
                )
                .WithTitle("Got Meme")
                .WithDescription("This is a long message with some rich text.")
                .WithColor(0x0000ff)
                .WithImageUrl(meme.Url);
            var message = await ReplyAsync(embed: embed.Build());
            await message.AddReactionsAsync(new IEmote[]{
                new Emoji("1️⃣"),
                new Emoji("2️⃣"),
                new Emoji("3️⃣"),
            });


            Console.WriteLine($"Embeded image to {message.Channel.Id}, {message.Author.Id}, {message.GetJumpUrl()}");
        }
    }
}