using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

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
            var existing = await db.Games.AsAsyncEnumerable().SingleOrDefaultAsync(game => game.GuildId == Context.Guild.Id);
            if (existing != null)
            {
                await ReplyAsync("Game Already exists on channel");
                return;
            }

            var addResult = await db.AddAsync(new GameState
            {
                ChannelId = Context.Channel.Id,
                GuildId = Context.Guild.Id,
            });

            var gameState = addResult.Entity;

            var view = await ViewFactory.CreateGameView(Context, gameState);
            var msgRef = new MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id);
            var gameMessage = await ReplyAsync(message: view.Message, embed: view.Embed.Build(), messageReference: msgRef);
            addResult.Entity.MessageId = gameMessage.Id;
            await db.SaveChangesAsync();
        }

        [Command("join")]
        [Summary("Joins a game.")]
        public async Task GameJoinAsync()
        {
            var existing = await db.Games
                .Include(game => game.Players)
                .SingleOrDefaultAsync(game => game.GuildId == Context.Guild.Id && game.ChannelId == Context.Channel.Id);

            if (existing == null)
            {
                await ReplyAsync("Game Doesn't exist");
                return;
            }

            if (!existing.IsJoinable(Context.User.Id, out string msg))
            {
                await ReplyAsync(msg);
                return;
            }

            existing.Players.Add(new PlayerState
            {
                UserId = Context.User.Id,
            });
            await db.SaveChangesAsync();
            await ReplyAsync("You Joined!");
        }

        [Command("delete")]
        [Summary("Deletes a game.")]
        // TODO: Restrict to admin roles
        public async Task GameDeleteAsync()
        {
            var existing = await db.Games.AsAsyncEnumerable()
                .SingleOrDefaultAsync(game => game.GuildId == Context.Guild.Id);

            if (existing == null)
            {
                await ReplyAsync("Game Doesn't exist");
                return;
            }

            db.Remove(existing);
            await db.SaveChangesAsync();
            await ReplyAsync("Game Deleted!");
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