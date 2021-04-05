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
            await view.Init(context, gameState);
            return view;
        }
    }

    public interface IGameView
    {
        string Message { get; }
        EmbedBuilder Embed { get; }
    }

    public class GameLobbyView : IGameView
    {
        public string Message { get; set; }
        public EmbedBuilder Embed { get; set; }

        public GameLobbyView() { }


        public async Task Init(ICommandContext context, GameState gameState)
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
        }
    }
}