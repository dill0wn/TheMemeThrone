
using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MemeThroneBot.Commands
{
    public class CommandListeners
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;

        // public MemingContext db { get; set; }

        public CommandListeners(CommandService commands, DiscordSocketClient client)
        {
            this.client = client;
            this.commands = commands;
            Console.WriteLine("CommandListeners initialized");

            client.ReactionAdded += this.HandleReactionAdded;
            client.ReactionRemoved += this.HandleReactionRemoved;
        }

        private Task HandleReactionAdded(Cacheable<IUserMessage, ulong> user, ISocketMessageChannel channel, SocketReaction reaction)
        {
            Console.WriteLine("CommandListeners HandleReactionAdded");
            return Task.CompletedTask;
        }

        private Task HandleReactionRemoved(Cacheable<IUserMessage, ulong> user, ISocketMessageChannel channel, SocketReaction reaction)
        {
            Console.WriteLine("CommandListeners HandleReactionRemoved");
            return Task.CompletedTask;
        }
    }
}