using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace MemeThroneBot.Commands
{
    [Group("game")]
    public class GameModule : ModuleBase<SocketReactionCommandContext>
    {
        public ViewFactory Views { get; set; }
        public MemingContext DB { get; set; }

        public async Task<GameState> GetGameStateFromContextAsync()
        {
            var task = DB.Games
                .Include(game => game.Players)
                .SingleOrDefaultAsync(game => game.GuildId == Context.Guild.Id && game.ChannelId == Context.Channel.Id);
            var existing = await task;
            return existing;
        }

        [Command("create")]
        [Summary("Creates a game.")]
        public async Task GameCreateAsync()
        {
            var existing = await DB.Games.AsAsyncEnumerable().SingleOrDefaultAsync(game => game.GuildId == Context.Guild.Id);
            if (existing != null)
            {
                await ReplyAsync("Game Already exists on channel");
                return;
            }

            var addResult = await DB.AddAsync(new GameState
            {
                ChannelId = Context.Channel.Id,
                GuildId = Context.Guild.Id,
            });

            var gameState = addResult.Entity;

            var view = await Views.CreateGameView(Context, gameState);

            var gameMessage = await Views.RenderViewAsReplyAsync(Context.Message, view);

            // TODO: render a separate view to a DM

            await gameMessage.AddReactionAsync(new Emoji(KeyMotes.GAME_JOIN));

            addResult.Entity.MessageId = gameMessage.Id;
            await DB.SaveChangesAsync();
        }

        [Command("join")]
        [Summary("Joins a game.")]
        public async Task GameJoinAsync()
        {
            var gameState = await GetGameStateFromContextAsync();

            if (gameState == null)
            {
                await ReplyAsync("Game Doesn't exist");
                return;
            }

            ulong userId = Context.User.Id;
            if (this.Context.IsReaction)
            {
                // noop
            }

            if (!gameState.JoinGame(userId, out string msg))
            {
                await ReplyAsync(msg);
                return;
            }

            var view = await Views.CreateGameView(Context, gameState);

            var gameMessage = await Views.UpdateViewAsync(gameState.MessageReference, view);

            await DB.SaveChangesAsync();
        }
        
        [Command("start")]
        [Summary("Starts a game.")]
        public async Task GameStartAsync()
        {
            var gameState = await GetGameStateFromContextAsync();

            if (gameState == null)
            {
                await ReplyAsync("Game Doesn't exist");
                return;
            }

            ulong userId = Context.User.Id;
            if (this.Context.IsReaction)
            {
                // noop
            }

            if (!gameState.StartGame(out string msg))
            {
                await ReplyAsync(msg);
                return;
            }
            
            var view = await Views.CreateGameView(Context, gameState);

            var gameMessage = await Views.RenderViewAsReplyAsync(gameState.MessageReference, view);

            await DB.SaveChangesAsync();
        }

        [Command("delete")]
        [Summary("Deletes a game.")]
        // TODO: Restrict to admin roles
        public async Task GameDeleteAsync()
        {
            var existing = await DB.Games.AsAsyncEnumerable()
                .SingleOrDefaultAsync(game => game.GuildId == Context.Guild.Id);

            if (existing == null)
            {
                await ReplyAsync("Game Doesn't exist");
                return;
            }

            DB.Remove(existing);
            await DB.SaveChangesAsync();
            await Context.Message.ReplyAsync("Game Deleted!");
            // TODO: delete original message
        }
    }
















    public class PingModule : ModuleBase<SocketReactionCommandContext>
    {
        [Command("ping")]
        public Task SayAsync() => ReplyAsync("pong");

    }

    [Group("get")]
    public class GetModule : ModuleBase<SocketReactionCommandContext>
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