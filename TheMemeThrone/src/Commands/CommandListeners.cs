
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MemeThroneBot.Commands
{
    public class CommandListeners
    {
        private readonly IServiceProvider services;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;

        // public MemingContext db { get; set; }

        public CommandListeners(IServiceProvider services, CommandService commands, DiscordSocketClient client)
        {
            this.services = services;
            this.client = client;
            this.commands = commands;
            Console.WriteLine("CommandListeners initialized");

            client.ReactionAdded += this.HandleReactionAdded;
            client.ReactionRemoved += this.HandleReactionRemoved;
        }

        private async Task HandleReactionAdded(Cacheable<IUserMessage, ulong> msgCache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            Console.WriteLine("CommandListeners HandleReactionAdded");
            var message = await msgCache.GetOrDownloadAsync();
            var commandContext = new CommandContext(this.client, message);
            var result = await commands.ExecuteAsync(commandContext, "ping", this.services);
            Console.WriteLine("CommandListeners executed command {0}", result);
        }

        private async Task HandleReactionRemoved(Cacheable<IUserMessage, ulong> msgCache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            Console.WriteLine("CommandListeners HandleReactionRemoved");
            await Task.CompletedTask;
        }
    }
}