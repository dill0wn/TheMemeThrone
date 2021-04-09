using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MemeThroneBot
{
    public class ViewFactory
    {
        private readonly DiscordSocketClient client;
        public ViewFactory(DiscordSocketClient client)
        {
            this.client = client;
        }

        public async Task<IGameView> CreateGameView(ICommandContext context, GameState gameState)
        {
            var view = new GameLobbyView();
            await view.BuildAsync(context, gameState);
            return view;
        }

        public async Task<IUserMessage> RenderViewAsReplyAsync(MessageReference messageReference, IGameView view)
        {
            var channel = client.GetGuild(messageReference.GuildId.Value).GetTextChannel(messageReference.ChannelId);
            var gameMessage = await channel.SendMessageAsync(text: view.Text, embed: view.EmbedBuilder.Build(), messageReference: messageReference);
            return gameMessage;
        }

        public async Task<IUserMessage> RenderViewAsReplyAsync(IUserMessage message, IGameView view)
        {
            var gameMessage = await message.ReplyAsync(text: view.Text, embed: view.EmbedBuilder.Build());
            return gameMessage;
        }

        public async Task<IUserMessage> UpdateViewAsync(MessageReference messageReference, IGameView view)
        {
            var message = await client
                .GetGuild(messageReference.GuildId.Value)
                .GetTextChannel(messageReference.ChannelId)
                .GetMessageAsync(messageReference.MessageId.Value) as IUserMessage;
            return await UpdateViewAsync(message, view);
        }

        public async Task<IUserMessage> UpdateViewAsync(IUserMessage message, IGameView view)
        {
            await message.ModifyAsync(m =>
            {
                m.Content = view.Text;
                m.Embed = view.EmbedBuilder.Build();
            });
            return message;
        }
    }

    public interface IGameView
    {
        string Text { get; }
        EmbedBuilder EmbedBuilder { get; }

        Task BuildAsync(ICommandContext context, GameState gameState);
    }

    public class GameLobbyView : IGameView
    {
        public string Text { get; set; }
        public EmbedBuilder EmbedBuilder { get; set; }

        public GameLobbyView() { }

        public async Task BuildAsync(ICommandContext context, GameState gameState)
        {
            string playerString = "";
            if (gameState.Players.Count == 0)
            {
                playerString = "None";
            }
            else
            {
                playerString = string.Join("\n", gameState.Players.Select(/* async */ p =>
                {
                    // var user = await context.Guild.GetUserAsync(p.User);
                    return $"- <@{p.UserId}>";
                }));
            }

            EmbedBuilder = new EmbedBuilder()
                .WithTitle("Who Will Claim The Meme Throne?!")
                .WithDescription(string.Join("\n\n", new string[]{
                    "Play a game, have fun.",
                    $"React with {KeyMotes.GAME_JOIN} to join!",
                    "You can join at any time"
                }))
                .WithFields(
                    // new EmbedFieldBuilder
                    // {
                    //     Name = "Created By",
                    //     Value = "Some Nerd", // TODO: game ownership?
                    // },
                    new EmbedFieldBuilder
                    {
                        Name = "Players",
                        Value = playerString,
                    }
                );
            Console.WriteLine("Rendered view");

            await Task.CompletedTask;
        }
    }
}