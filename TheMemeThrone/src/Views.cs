using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MemeThroneBot
{

    public interface IGameView
    {
        string Text { get; }
        EmbedBuilder EmbedBuilder { get; }

        Task BuildAsync(ICommandContext context, GameState gameState);
    }

    public abstract class BaseGameView : IGameView
    {
        public string Text { get; set; }
        public EmbedBuilder EmbedBuilder { get; protected set; } = new EmbedBuilder();

        public abstract Task BuildAsync(ICommandContext context, GameState gameState);
    }

    public class GameLobbyView : BaseGameView
    {
        public override async Task BuildAsync(ICommandContext context, GameState gameState)
        {
            EmbedBuilder
                .WithTitle("Who Will Claim The Meme Throne?!")
                .WithDescription(string.Join("\n\n", new string[]{
                    "Play a game, have fun.",
                    $"React with {KeyMotes.GAME_JOIN} to join!",
                    "You can join at any time"
                }))
                .WithFields(
                    new EmbedFieldBuilder
                    {
                        Name = "Players",
                        Value = ViewHelpers.PlayerList(gameState),
                    }
                );
            Console.WriteLine("Rendered view");

            await Task.CompletedTask;
        }
    }

    public class TurnPublicView : BaseGameView
    {
        public override async Task BuildAsync(ICommandContext context, GameState gameState)
        {
            var currentPlayer = gameState.CurrentPlayer;

            // TODO: either get services/contexts in here, or have that info in GameState ready to be plucked.
            // var meme = await context.MemeCards.AsAsyncEnumerable().FirstOrDefaultAsync();

            var url = "https://i.kym-cdn.com/entries/icons/original/000/022/134/elmo.jpg";

            EmbedBuilder
                .WithTitle($"It is {ViewHelpers.UserLink(currentPlayer.UserId)}'s turn.")
                .WithDescription(string.Join("\n\n", new string[]{
                    "Waiting on players...",
                    "3/5 players have selected their captions",
                    $"React with {KeyMotes.GAME_JOIN} to join the next round",
                }))
                .WithImageUrl(url)
                // .WithImageUrl(meme.Url)
                .WithFields(
                    new EmbedFieldBuilder
                    {
                        Name = "Players",
                        Value = ViewHelpers.PlayerList(gameState),
                    }
                );

            await Task.CompletedTask;
        }
    }

    // public class TurnActivePlayerView : BaseGameView
    // {

    // }

    // public class TurnCaptionPlayerView : BaseGameView
    // {

    // }
}