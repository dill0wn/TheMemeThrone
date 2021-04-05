using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MemeThroneBot.Commands
{
    public class PingModule : ModuleBase<CommandContext>
    {
        [Command("ping")]
        public Task SayAsync() => ReplyAsync("pong");

    }

    [Group("game")]
    public class GameModule : ModuleBase<CommandContext>
    {
        public MemingContext db { get; set; }

        [Command("create")]
        [Summary("Creates a game.")]
        public async Task GameCreateAsync()
        {
            var existing = await db.Games.SingleOrDefaultAsync(game => game.Guild == Context.Guild.Id);
            if (existing != null)
            {
                await ReplyAsync("Game Already exists on channel");
                return;
            }


            await db.AddAsync(new GameState
            {
                Channel = Context.Channel.Id,
                Guild = Context.Guild.Id,
                State = "idle"
            });
            await db.SaveChangesAsync();
            await ReplyAsync("Game Created!");
        }
    }

    [Group("get")]
    public class GetModule : ModuleBase<CommandContext>
    {
        public MemingContext db { get; set; }

        [Command("caption")]
        [Summary("Echoes a message.")]
        public async Task GetCaptionAsync()
        {
            var caption = await db.CaptionCards.AsAsyncEnumerable().FirstOrDefaultAsync();

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
            var meme = await db.MemeCards.AsAsyncEnumerable().FirstOrDefaultAsync();

            Console.WriteLine($"get meme {meme}");
            if (meme == null)
            {
                await ReplyAsync("No memes availble");
                return;
            }

            var embed = new EmbedBuilder()
                    .WithTitle("Waiting on Meme...");
            var message = await ReplyAsync(embed: embed.Build());

            message = await Context.Channel.GetMessageAsync(message.Id, CacheMode.AllowDownload) as IUserMessage;

            await Task.WhenAll(
                message.AddReactionsAsync(new IEmote[]{
                    new Emoji("1️⃣"),
                    new Emoji("2️⃣"),
                    new Emoji("3️⃣"),
                }),

                message.ModifyAsync((m) =>
                {
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
                    m.Embed = embed.Build();
                })
            );

            Console.WriteLine($"Embeded image to {message.Channel.Id}, {message.Author.Id}, {message.GetJumpUrl()}");
        }
    }
}