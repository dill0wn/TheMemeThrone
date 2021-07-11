using System.Linq;
using Discord;
using MemeThroneBot;

namespace MemeThroneBot
{
    public class ViewHelpers
    {
        public static string PlayerList(GameState gameState)
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
                    // return $"- <@{p.UserId}>";
                    return $"- {UserLink(p.UserId)}";
                }));
            }

            return playerString;
        }

        public static string UserLink(IUser user)
        {
            return UserLink(user.Id);
        }

        public static string UserLink(ulong userId)
        {
            return $"<@{userId}>";
        }
    }
}