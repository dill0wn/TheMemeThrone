using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MemeThroneBot
{
    public class SocketReactionCommandContext : ICommandContext
    {
        public IDiscordClient Client { get; }

        public IGuild Guild { get; }

        public IMessageChannel Channel { get; }

        public IUser User { get; }

        public IUserMessage Message { get; }

        public IReaction Reaction { get; }

        public bool IsReaction => Reaction != null;

        public SocketReactionCommandContext(DiscordSocketClient client, IUserMessage message, SocketReaction reaction = null)
        {
            Message = message;
            Client = client;

            if (reaction != null)
            {
                User = reaction.User.Value;
                Reaction = reaction;
            }
            else
            {
                User = message.Author;
                Reaction = null;
            }

            var guild = client.GetGuild(Message.Reference.GuildId.Value);
            Guild = guild;
            Channel = guild.GetTextChannel(Message.Reference.ChannelId);
        }
    }
}