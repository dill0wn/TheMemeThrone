
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace MemeThroneBot.Commands
{
    public class CommandListeners
    {
        private readonly IServiceProvider services;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;

        public readonly Dictionary<string, string> emojiMap = new Dictionary<string, string>{
            { KeyMotes.GAME_JOIN, "game join" }
        };

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
            if (client.CurrentUser.Id == reaction.UserId)
            {
                Console.WriteLine("CommandListeners HandleReactionAdded -- ignoring because from me");
                return;
            }

            if (!reaction.User.IsSpecified)
            {
                Console.WriteLine("CommandListeners HandleReactionAdded -- user not specified");
            }
            else if (reaction.User.Value.IsBot)
            {
                Console.WriteLine("CommandListeners HandleReactionAdded -- ignoring because bot");
                return;
            }
            if (emojiMap.TryGetValue(reaction.Emote.Name, out string cmd))
            {
                Console.WriteLine($"CommandListeners HandleReactionAdded -- attempting {cmd}");
                var message = await msgCache.GetOrDownloadAsync();
                var commandContext = new CommandContext(this.client, message);

                using (var scope = services.CreateScope())
                {
                    var reactionContext = scope.ServiceProvider.GetService<ReactionContext>();
                    reactionContext.Init(reaction);
                    var result = await commands.ExecuteAsync(commandContext, cmd, scope.ServiceProvider);
                }
            }
            else
            {
                Console.WriteLine("CommandListeners HandleReactionAdded -- skipping");
            }
        }

        private async Task HandleReactionRemoved(Cacheable<IUserMessage, ulong> msgCache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            Console.WriteLine("CommandListeners HandleReactionRemoved");
            await Task.CompletedTask;
        }
    }
}