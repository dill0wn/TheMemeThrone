using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MemeThroneBot
{
    public class SocketReactionCommandContext : ICommandContext
    {
        public IDiscordClient Client { get;  protected set; }

        public IGuild Guild { get;  protected set; }

        public IMessageChannel Channel { get;  protected set; }

        public IUser User { get; protected set; }
        public ulong UserId { get;  protected set; }

        public IUserMessage Message { get;  protected set; }

        public IReaction Reaction { get;  protected set; }

        public bool IsReaction => Reaction != null;

        public SocketReactionCommandContext(DiscordSocketClient client, IUserMessage message, SocketReaction reaction = null)
        {
            Message = message;
            Client = client;

            if (message is Discord.Rest.RestUserMessage)
            {
                var guild = client.GetGuild(Message.Reference.GuildId.Value);
                Guild = guild;
                Channel = guild.GetTextChannel(Message.Reference.ChannelId);
            }
            else if (message is SocketUserMessage)
            {
                Channel = (message as SocketUserMessage).Channel;
                Guild = (Channel as SocketTextChannel).Guild;
            }

            if (reaction != null)
            {
                Reaction = reaction;
                UserId = reaction.UserId;

                if (reaction.User.IsSpecified)
                {
                    User = reaction.User.Value;
                }
                else
                {
                    User = client.GetUser(reaction.UserId);
                    // User = await Guild.GetUserAsync(reaction.UserId);
                }
            }
            else
            {
                User = message.Author;
                Reaction = null;
            }
        }

        // public static async Task<SocketReactionCommandContext> FromReactionAsync(DiscordSocketClient client, IUserMessage message, SocketReaction reaction)
        // {
        //     if (reaction.User.IsSpecified)
        //     {
        //         User = reaction.User.Value;
        //     }
        //     else
        //     {
        //         User = client.GetUser(reaction.UserId);
        //     }
        //     return new SocketReactionCommandContext(client, message, reaction);
        // }
    }
}