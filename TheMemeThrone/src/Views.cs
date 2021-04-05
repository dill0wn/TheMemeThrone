using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MemeThroneBot
{
    public class ViewFactory
    {
        public static async Task<IGameView> CreateGameView(ICommandContext context, GameState gameState)
        {
            var view = new GameLobbyView();
            await view.BuildAsync(context, gameState);
            return view;
        }

        // TODO: don't feel great about this structure.
        public static async Task<IUserMessage> ReplyAsync(ICommandContext context, IGameView view)
        {
            // var msgRef = new MessageReference(context.Message.Id, context.Channel.Id, context.Guild.Id);
            var gameMessage = await context.Message.ReplyAsync(text: view.Message, embed: view.Embed.Build());
            return gameMessage;
        }
    }

    public interface IGameView
    {
        string Message { get; }
        EmbedBuilder Embed { get; }

        Task BuildAsync(ICommandContext context, GameState gameState);
    }

    public class GameLobbyView : IGameView
    {
        public string Message { get; set; }
        public EmbedBuilder Embed { get; set; }

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

            Embed = new EmbedBuilder()
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